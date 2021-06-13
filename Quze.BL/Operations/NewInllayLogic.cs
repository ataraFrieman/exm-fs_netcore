using Microsoft.Extensions.Configuration;
using Quze.BL.Operations;
using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quze.BL.Operations
{
    public class NewInllayLogic
    {
        const int LifeSavingPriority = 1;
        OperationConflictLogic operationConflictLogic;
        EditOperationLogic editOperationLogic;
        OpeartionsLogic opeartionsLogic;
        RescheduleStore RS;

        public NewInllayLogic(QuzeContext ctx,IConfiguration configuration)
        {
            operationConflictLogic = new OperationConflictLogic(ctx);
            editOperationLogic = new EditOperationLogic(ctx);
            opeartionsLogic = new OpeartionsLogic(ctx, configuration);
            RS = new RescheduleStore(ctx);
        }
        public NewInllayLogic(QuzeContext ctx)
        {
            operationConflictLogic = new OperationConflictLogic(ctx);
            editOperationLogic = new EditOperationLogic(ctx);
            RS = new RescheduleStore(ctx);
        }

        public async Task<OperationsResponse> RescheduleAsync(OperationsResponse operationQueue,DateTime beginTime, List<Equipment> equipments)
        {
            List<Appointment> AppointmentList =operationQueue.OperationsList.Where(ap => ap.Operation.Status != 7 && !ap.IsDeleted).ToList();
            List<Operation> operationList = AppointmentList.Select(a => a.Operation).ToList();
            if (operationList == null || operationList == null)
                return null;
            DateTime BeginTime =beginTime;
            List<Operation> LifeSavingOperations = operationList.Where(o => o.Priority == LifeSavingPriority).ToList();
            List<Operation> RegularOperations = operationList.Where(o => o.Priority != LifeSavingPriority).ToList();
            List<Operation> NewOperationList;
            LifeSavingOperations = LifeSavingOperations.OrderBy(o => (o.SurgeryOrigEndTime - o.AnesthesiaOrigBeginTime).TotalSeconds).ToList();
            RegularOperations = RegularOperations.OrderBy(o => (o.SurgeryOrigEndTime - o.AnesthesiaOrigBeginTime).TotalSeconds).ToList();
            NewOperationList = LifeSavingOperations;
            NewOperationList.AddRange(RegularOperations);
            NewOperationList =InitTimeAfterRescheduling(NewOperationList, BeginTime);

            NewOperationList = operationConflictLogic.ResolveConflicts(NewOperationList, BeginTime);
            //NewOperationList = editOperationLogic.UpdateEquipmentForAppointment(NewOperationList, equipments);
            foreach (var item in AppointmentList)
            {
                var operation = NewOperationList.FirstOrDefault(o => o.Id == item.OperationId);
                item.Operation = operation;
                item.BeginTime = operation.AnesthesiaBeginTime;
                item.EndTime = operation.SurgeryEndTime;
            }
            AppointmentList=AppointmentList.OrderBy(x => x.BeginTime).ToList();
            foreach (var item in AppointmentList)
            {
                var operationRecord = new OperationRecord();
                operationRecord.LocationCode = item.Operation.RoomId;
                operationRecord.BeginTime = item.BeginTime;
                operationRecord.SurgeonId = item.Operation.Surgeon.IdentityNumber;
                operationRecord.AnesthesiologistId = item.Operation.Anesthesiologist.IdentityNumber;
                operationRecord.SurgicalNursId = item.Operation.Nurse.IdentityNumber;
                operationRecord.HostingDepartmentId = item.Operation.HostingDepartment.Code;
                operationRecord.SurgicalDepartmentId = item.Operation.SurgicalDepartment.Code;
                var app = await opeartionsLogic.GetMLDataAPIRequestAsync(item, AppointmentList, item.Fellow, item.Operation, item.ServiceType, operationQueue.ServiceQueue.BeginTime, operationRecord, 'S').ConfigureAwait(false);
            }

            //for (int i = 0; i < AppointmentList.Count; i++)
            //{
            //    var item = AppointmentList[i];
            //    var operation = NewOperationList.FirstOrDefault(o => o.Id == item.OperationId);
            //    var operationRecord = new OperationRecord();
            //    operationRecord.LocationCode = item.Operation.RoomId;
            //    operationRecord.BeginTime = item.BeginTime;
            //    operationRecord.SurgeonId = item.Operation.Surgeon.IdentityNumber;
            //    operationRecord.AnesthesiologistId = item.Operation.Anesthesiologist.IdentityNumber;
            //    operationRecord.SurgicalNursId = item.Operation.Nurse.IdentityNumber;
            //    operationRecord.HostingDepartmentId = item.Operation.HostingDepartment.Code;
            //    operationRecord.SurgicalDepartmentId = item.Operation.SurgicalDepartment.Code;


            //    item = await opeartionsLogic.GetMLDataAPIRequestAsync(item, AppointmentList, item.Fellow, item.Operation, item.ServiceType, operationRecord, 'S').ConfigureAwait(false);
            //    item.Operation = operation;
            //    item.BeginTime = operation.AnesthesiaBeginTime;
            //    item.EndTime = operation.SurgeryEndTime;
            //    AppointmentList[i] = item;
            //}

            return new OperationsResponse()
            {
                ConflictList = null,
                OperationsList = AppointmentList,
                ServiceQueue =operationQueue.ServiceQueue
            };

        }


        public List<Operation> InitTimeAfterRescheduling(List<Operation> newOperationList, DateTime beginTime, char state = 'S')
        {
            Dictionary<int, List<Operation>> OpeartionsByRooms = new Dictionary<int, List<Operation>>();
            int[] Rooms = newOperationList.Select(o => o.RoomId).Distinct().ToArray();//every room with operations
            for (int i = 0; i < Rooms.Length; i++)
            {
                OpeartionsByRooms.Add(Rooms[i], newOperationList.Where(o => o.RoomId == Rooms[i]).ToList());
                OpeartionsByRooms[Rooms[i]] = OpeartionsByRooms[Rooms[i]].OrderBy(x => x.Priority).ToList();
            }
            foreach (var item in OpeartionsByRooms)
            {
                var room = item.Value;
                DateTime time = beginTime;
                int timrTurnOver = 0;
                bool isRoomFirstCase;
                if (state == 'S')
                    for (int j = 0; j < room.Count; j++)
                    {
                        int AnesthesiaDuration = (int)(room[j].AnesthesiaOrigEndTime - room[j].AnesthesiaOrigBeginTime).TotalSeconds;
                        int SurgeryDuration = (int)(room[j].SurgeryOrigEndTime - room[j].SurgeryOrigBeginTime).TotalSeconds;
                        isRoomFirstCase = j == 0 ? true : false;
                        //timrTurnOver = opeartionsLogic.CalcDelay(item.Value[j].Delay.Value, isRoomFirstCase);
                        item.Value[j].AnesthesiaBeginTime = time;//.AddMinutes(timrTurnOver);
                        item.Value[j].SurgeryBeginTime = time;//.AddMinutes(timrTurnOver);
                        //time = time.AddSeconds(timrTurnOver);
                        time = time.AddSeconds(AnesthesiaDuration);
                        item.Value[j].AnesthesiaEndTime = time;
                        item.Value[j].SurgeryEndTime = time;
                    }
                else
                    for (int j = 0; j < room.Count; j++)
                    {
                        isRoomFirstCase = j == 0 ? true : false;
                        //timrTurnOver = opeartionsLogic.CalcDelay(item.Value[j].Delay.Value, isRoomFirstCase);
                        item.Value[j].AnesthesiaOrigBeginTime = time;//.AddMinutes(timrTurnOver);
                        item.Value[j].SurgeryOrigBeginTime = time;//.AddMinutes(timrTurnOver);
                        //time = time.AddSeconds(timrTurnOver);
                        time = time.AddSeconds((int)room[j].OperationDuration*60);
                        item.Value[j].AnesthesiaOrigEndTime = time;
                        item.Value[j].SurgeryOrigEndTime = time;
                    }
            }
            List<Operation> OperationsList = new List<Operation>();
            foreach (var item in OpeartionsByRooms)
            {
                var room = item.Value;
                if (room != null && room.Count > 0)
                    OperationsList.AddRange(room);
            }
            return OperationsList;
        }

    }
}
public class ReschedualRequest
{
    public OperationsResponse operationQueue { get; set; }
    public DateTime BeginTime { get; set; }
    public List<Equipment> Equipments { get; set; }
}