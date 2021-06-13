using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quze.BL.Operations
{
    public class OperationConflictLogic
    {
        public QuzeContext context { get; set; }
        OpeartionsLogic opeartionsLogic;
        public OperationConflictLogic(QuzeContext ctx)
        {
            context = ctx;
            //opeartionsLogic = new OpeartionsLogic(ctx);
        }

        public Operation FindEarlyestAppointment(Operation o1, Operation o2, string type)
        {
            switch (type)
            {
                case "Anesthesia":
                    return o1.AnesthesiaOrigBeginTime < o2.AnesthesiaOrigBeginTime ? o1 : o2;
                case "Clean":
                    return o1.CleanOrigBeginTime < o2.CleanOrigBeginTime ? o1 : o2;
                case "surgery":
                default:
                    return o1.SurgeryOrigBeginTime < o2.SurgeryOrigBeginTime ? o1 : o2;
            }
        }

        public Operation FindLatest(Operation o1, Operation o2, string type)
        {
            switch (type)
            {
                case "Anesthesia":
                    return o1.AnesthesiaOrigBeginTime > o2.AnesthesiaOrigBeginTime ? o1 : o2;
                case "Clean":
                    return o1.CleanOrigBeginTime > o2.CleanOrigBeginTime ? o1 : o2;
                case "surgery":
                default:
                    return o1.SurgeryOrigBeginTime > o2.SurgeryOrigBeginTime ? o1 : o2;
            }

        }

        public Operation FindEarlyestAppointmentSchedule(Operation o1, Operation o2, string type)
        {
            switch (type)
            {
                case "Anesthesia":
                    return o1.AnesthesiaBeginTime < o2.AnesthesiaBeginTime ? o1 : o2;
                case "Clean":
                    return o1.CleanBeginTime < o2.CleanBeginTime ? o1 : o2;
                case "surgery":
                default:
                    return o1.SurgeryBeginTime < o2.SurgeryBeginTime ? o1 : o2;
            }
        }

        public Operation FindLatestSchedule(Operation o1, Operation o2, string type)
        {
            switch (type)
            {
                case "Anesthesia":
                    return o1.AnesthesiaBeginTime > o2.AnesthesiaBeginTime ? o1 : o2;
                case "Clean":
                    return o1.CleanBeginTime > o2.CleanBeginTime ? o1 : o2;
                case "surgery":
                default:
                    return o1.SurgeryBeginTime > o2.SurgeryBeginTime ? o1 : o2;
            }

        }
        public int CompareAppointmentsDuration(DateTime beginTime1, DateTime endTime1, DateTime beginTime2, DateTime endTime2)
        {
            DateTime EarliestEndTime = endTime1 > endTime2 ? endTime2 : endTime1;
            if (beginTime2 < EarliestEndTime)
                return (int)(EarliestEndTime - beginTime2).TotalSeconds;
            else
                return 0;
        }

        public Conflict CreateConflict(Appointment app1, Appointment newApp, string conflictType, DateTime conflictBT, int duration)
        {
            DateTime ET = conflictBT.AddSeconds(duration);
            Conflict AppointmentConflict = new Conflict()
            {
                AppointmentAId = app1.Id,
                AppointmentA = app1,
                AppointmentBId = newApp.Id,
                AppointmentB = newApp,
                Type = conflictType,
                ConflictBeginTime = conflictBT,
                ConflictEndTime = ET,
                ServiceQueueId = app1.ServiceQueueId
            };
            return AppointmentConflict;
        }

        public List<Conflict> FindIntersects(Appointment newOperationApp, List<Appointment> operationsList)
        {
            List<Conflict> conflicts = new List<Conflict>();
            foreach (Appointment item in operationsList)
            {
                if (item.Operation.Status != 7 && item.Operation.Status != 8)
                {
                    //Anesthesia  Conflict
                    int AnesthesiaConflictDuration = 0;
                    if (item.Operation.Anesthesiologist.IdentityNumber == newOperationApp.Operation.Anesthesiologist.IdentityNumber)
                    {
                        var LatestAnesthesia = FindLatest(newOperationApp.Operation, item.Operation, "Anesthesia");
                        var EarlyestAnesthesia = FindEarlyestAppointment(newOperationApp.Operation, item.Operation, "Anesthesia");
                        AnesthesiaConflictDuration = CompareAppointmentsDuration(EarlyestAnesthesia.AnesthesiaOrigBeginTime, EarlyestAnesthesia.AnesthesiaOrigEndTime, LatestAnesthesia.AnesthesiaOrigBeginTime, LatestAnesthesia.AnesthesiaOrigEndTime);
                        if (AnesthesiaConflictDuration > 0)
                            conflicts.Add(CreateConflict(item, newOperationApp, "Anesthesia", LatestAnesthesia.AnesthesiaOrigBeginTime, AnesthesiaConflictDuration));
                    }

                    //nurse  Conflict
                    if (item.Operation.Nurse.IsNotNull() && newOperationApp.Operation.Nurse.IsNotNull())
                    {
                        AnesthesiaConflictDuration = 0;
                        if (item.Operation.Nurse.IdentityNumber == newOperationApp.Operation.Nurse.IdentityNumber)
                        {
                            var LatestAnesthesia = FindLatest(newOperationApp.Operation, item.Operation, "Anesthesia");
                            var EarlyestAnesthesia = FindEarlyestAppointment(newOperationApp.Operation, item.Operation, "Anesthesia");
                            AnesthesiaConflictDuration = CompareAppointmentsDuration(EarlyestAnesthesia.AnesthesiaOrigBeginTime, EarlyestAnesthesia.AnesthesiaOrigEndTime, LatestAnesthesia.AnesthesiaOrigBeginTime, LatestAnesthesia.AnesthesiaOrigEndTime);
                            if (AnesthesiaConflictDuration > 0)
                                conflicts.Add(CreateConflict(item, newOperationApp, "Anesthesia", LatestAnesthesia.AnesthesiaOrigBeginTime, AnesthesiaConflictDuration));
                        }
                    }

                    //surgery  Conflict
                    int OperationConflictDuration = 0;
                    if (item.Operation.Surgeon.IdentityNumber == newOperationApp.Operation.Surgeon.IdentityNumber)
                    {
                        var LatestOperation = FindLatest(newOperationApp.Operation, item.Operation, "surgery");
                        var EarlyestOperation = FindEarlyestAppointment(newOperationApp.Operation, item.Operation, "surgery");
                        OperationConflictDuration = CompareAppointmentsDuration(EarlyestOperation.SurgeryOrigBeginTime, EarlyestOperation.SurgeryOrigEndTime, LatestOperation.SurgeryOrigBeginTime, LatestOperation.SurgeryOrigEndTime);
                        if (OperationConflictDuration > 0)
                            conflicts.Add(CreateConflict(item, newOperationApp, "surgery", LatestOperation.SurgeryOrigBeginTime, OperationConflictDuration));
                    }
                    //Room  Conflict
                    int RoomConflictDuration = 0;
                    if (item.Operation.RoomId == newOperationApp.Operation.RoomId)
                    {
                        var LatestOperation = FindLatest(newOperationApp.Operation, item.Operation, "surgery");
                        var EarlyestOperation = FindEarlyestAppointment(newOperationApp.Operation, item.Operation, "surgery");

                        if (LatestOperation.AnesthesiaOrigBeginTime < EarlyestOperation.SurgeryOrigEndTime)
                        {
                            RoomConflictDuration = CompareAppointmentsDuration(EarlyestOperation.AnesthesiaOrigBeginTime, EarlyestOperation.SurgeryOrigEndTime, LatestOperation.AnesthesiaOrigBeginTime, LatestOperation.SurgeryOrigEndTime);
                            if (RoomConflictDuration > 0)
                                conflicts.Add(CreateConflict(item, newOperationApp, "Anesthesia", newOperationApp.Operation.AnesthesiaOrigBeginTime, RoomConflictDuration));
                        }
                    }

                }

            }
            return conflicts;
        }


        public List<Operation> ResolveConflicts(List<Operation> newOperationList, DateTime beginTime)
        {
            OperationQueueStore operationsSt = new OperationQueueStore(context);
            newOperationList = CreateSortListByPriority(newOperationList);//lst order by priority and duration


            for (int i = 0; i < newOperationList.Count; i++)
            {
                bool isRoomFirstCase = false;
                if (i == 0)
                    isRoomFirstCase = true;
                var CurrOperation = newOperationList[i];
                for (int j = 0; j < i; j++)
                {
                    //surgery Conflict
                    int OperationConflictDuration = 0;
                    if (newOperationList[i].Surgeon.IdentityNumber == newOperationList[j].Surgeon.IdentityNumber)
                    {
                        var LatestOperation = FindLatestSchedule(newOperationList[i], newOperationList[j], "surgery");
                        var EarlyestOperation = FindEarlyestAppointmentSchedule(newOperationList[i], newOperationList[j], "surgery");
                        OperationConflictDuration = CompareAppointmentsDuration(EarlyestOperation.SurgeryBeginTime, EarlyestOperation.SurgeryEndTime, LatestOperation.SurgeryBeginTime, LatestOperation.SurgeryEndTime);
                        if (OperationConflictDuration > 0)
                        {
                            TimeSpan T = CurrOperation.AnesthesiaEndTime - CurrOperation.AnesthesiaBeginTime;
                            int AnesthesiaDuration = (int)(T.TotalMinutes);
                            CurrOperation = ChangeOperationTime(newOperationList[j].SurgeryEndTime, CurrOperation, isRoomFirstCase);
                        }
                    }

                    //Room  Conflict
                    if (newOperationList[i].RoomId == newOperationList[j].RoomId)
                    {
                        if (newOperationList[i].AnesthesiaBeginTime < newOperationList[j].SurgeryEndTime)
                            CurrOperation = ChangeOperationTime(newOperationList[j].SurgeryEndTime, CurrOperation, isRoomFirstCase);
                    }

                }
            }
            return newOperationList;
        }

        public Operation ChangeOperationTime(DateTime beginTime, Operation operation, bool isRoomFirstCase)
        {
            TimeSpan T = operation.SurgeryEndTime - operation.SurgeryBeginTime;
            int SurgeryDuration = (int)(T.TotalMinutes);
            operation.AnesthesiaBeginTime = beginTime;
            operation.SurgeryBeginTime = beginTime;
            operation.AnesthesiaEndTime = beginTime.AddMinutes(SurgeryDuration);
            operation.SurgeryEndTime = beginTime.AddMinutes(SurgeryDuration);
            
            return operation;
        }
        public int CalcDelay(int delay, bool isRoomFirstCase)
        {
            int timeTurnOver = 0;
            if (isRoomFirstCase)
            {
                if (delay > 30)
                    timeTurnOver = delay;
            }
            else
            {
                if (delay < 15)
                    timeTurnOver = 15;
                else
                    timeTurnOver = delay;
            }
            return timeTurnOver;
        }
        public List<Operation> CreateSortListByPriority(List<Operation> operationList)
        {
            Dictionary<int, List<Operation>> OpeartionsByPriority = new Dictionary<int, List<Operation>>();
            //int[] priority = operationList.Select(o => o.Priority).Distinct().ToArray();
            for (int i = 1; i <= 3; i++)
            {
                //dictonery by key priority value list operations in this priority
                OpeartionsByPriority.Add(i, operationList.Where(o => o.Priority == i).ToList());
                OpeartionsByPriority[i].OrderBy(x => x.AnesthesiaBeginTime);//order by begin time
            }
            List<Operation> OperationsList = new List<Operation>();
            foreach (var item in OpeartionsByPriority)
            {
                var operationsPriority = item.Value;
                if (operationsPriority != null && operationsPriority.Count > 0)
                    OperationsList.AddRange(operationsPriority);
            }
            return OperationsList;
        }

        public void RemoveConflictByAppointmentId(int appointmentId,List<Conflict> conflictsList)
        {
            conflictsList.RemoveAll(conflict => conflict.AppointmentAId == appointmentId || conflict.AppointmentBId == appointmentId);
        }


    }
}
