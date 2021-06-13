using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using Quze.Models;
using Quze.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Quze.BL.Operations
{
    public class ReadyEqpOperation
    {
        public Dictionary<int, bool> EquipmentsStatuses { get; set; }
        public Appointment Appointment { get; set; } = null;
    }
    public class ResponseReadyEqpOperation
    {
        public Appointment Appointment { get; set; } = null;
        public List<int> UnableEquipments { get; set; }
    }
    public class EditOperationLogic
    {
        public QuzeContext Context { get; set; }
        public OperationQueueStore operationQueueStore { get; set; }
        private AllocationOfEquipmentStore allocationOfEquipmentStore { get; set; }
        private EquipmentAppointmentRequestStore equipmentAppointmentRequestStore { get; set; }
        private OpeartionsLogic opeartionsLogic { get; set; }
        private AppointmentStore appointmentStore { get; set; }
        private NewInllayLogic newInllayLogic { get; set; }
        private OperationConflictLogic operationConflictLogic { get; set; }

        public EditOperationLogic(QuzeContext ctx)
        {
            Context = ctx;
            allocationOfEquipmentStore = new AllocationOfEquipmentStore(ctx);
            operationQueueStore = new OperationQueueStore(ctx);
            equipmentAppointmentRequestStore = new EquipmentAppointmentRequestStore(ctx);
            appointmentStore = new AppointmentStore(ctx);
            opeartionsLogic = new OpeartionsLogic(ctx);
            operationConflictLogic = new OperationConflictLogic(ctx);
        }
        public EditOperationLogic(QuzeContext ctx, IConfiguration configuration)
        {
            Context = ctx;
            opeartionsLogic = new OpeartionsLogic(ctx, configuration);
            appointmentStore = new AppointmentStore(ctx);
            newInllayLogic = new NewInllayLogic(Context, configuration);
            operationConflictLogic = new OperationConflictLogic(ctx);
        }

        //put true in isdelet colum 
        public OperationsResponse DeleteOperation(OperationsResponse operationQueue, char state, Appointment appointment)
        {
            operationQueueStore.DeleteOperation(appointment.Id);//delete in db
            operationQueue.OperationsList.Remove(appointment);
            if (operationQueue.ConflictList.IsNotNull())
                operationQueue.ConflictList = opeartionsLogic.CreateConflictsByOprationList(operationQueue.OperationsList);
            return operationQueue;
        }
        //add or remove Equipment if take add if return remove
        public ResponseReadyEqpOperation UpdateRadyEquipmentOperation(ReadyEqpOperation readyEqpOperation)
        {
            List<int> unableEquipments = new List<int>();
            Operation operation;
            foreach (var eqp in readyEqpOperation.EquipmentsStatuses)
            {
                AllocationOfEquipment allocationOfEquipment = readyEqpOperation.Appointment.AllocationOfEquipment.Find(allocationEqp => allocationEqp.EqpId == eqp.Key);
                if (allocationOfEquipment.IsNotNull())//exist
                {
                    if (!eqp.Value)//change to not taken
                    {
                        allocationOfEquipmentStore.RemoveAllocationOfEquipment(eqp.Key, allocationOfEquipment);//delete from db
                        readyEqpOperation.Appointment.AllocationOfEquipment.Remove(allocationOfEquipment);//remove from allocationOfEquipment list in this apointment
                    }
                }
                else//was'nt taken
                {
                    if (eqp.Value)//change to take equipment
                    {
                        //add to db
                        AllocationOfEquipment allocationOfEqp = allocationOfEquipmentStore.AddAllocationOfEquipment(eqp.Key, readyEqpOperation.Appointment);
                        if (allocationOfEqp.IsNotNull())//enable to take this equipment
                            readyEqpOperation.Appointment.AllocationOfEquipment.Add(allocationOfEqp);//add from allocationOfEquipment list in this apointment
                        else//unable
                            unableEquipments.Add(eqp.Key);
                    }
                }
            }
            if (unableEquipments.Count == 0)//ready equipment!!!
                operation = operationQueueStore.updateReadyEquipmentOperation(readyEqpOperation.Appointment.OperationId ?? 0, "Enabled");
            else
                operation = operationQueueStore.updateReadyEquipmentOperation(readyEqpOperation.Appointment.OperationId ?? 0, "Error");
            readyEqpOperation.Appointment.Operation = operation;
            ResponseReadyEqpOperation responseReadyEqpOperation = new ResponseReadyEqpOperation()
            {
                Appointment = readyEqpOperation.Appointment,
                UnableEquipments = unableEquipments
            };
            return responseReadyEqpOperation;
        }

        public List<EquipmentAppointmentRequest> AddEquipmentAppointmentRequest(ReadyEqpOperation readyEqpOperation)
        {
            List<EquipmentAppointmentRequest> equipmentsAppointments = equipmentAppointmentRequestStore.GetEquipmentsAppointments(readyEqpOperation.Appointment.Operation.Id);
            Dictionary<int, bool> EquipmentsStatuses = readyEqpOperation.EquipmentsStatuses;//list for new equipmentsAppointment
            List<EquipmentAppointmentRequest> addEquipmentsAppointments = new List<EquipmentAppointmentRequest>();
            if (equipmentsAppointments != null && equipmentsAppointments.Count > 0)
            {
                //all exist update if appointment dont want this eqp
                foreach (EquipmentAppointmentRequest equipment in equipmentsAppointments)
                {
                    if (!readyEqpOperation.EquipmentsStatuses[equipment.EqpId])//appointment don;t need this eqp
                    {
                        equipmentAppointmentRequestStore.RemoveEquipmentAppointment(equipment);
                        readyEqpOperation.Appointment.Operation.EquipmentAppointmentRequest.Remove(equipment);
                    }
                    EquipmentsStatuses.Remove(equipment.EqpId);
                }
            }
            foreach (var eqp in EquipmentsStatuses)
                if (eqp.Value)//add
                    addEquipmentsAppointments.Add(equipmentAppointmentRequestStore.CreateEquipmentAppointmentRequest(eqp.Key, readyEqpOperation.Appointment.Operation.Id));

            if (addEquipmentsAppointments != null && addEquipmentsAppointments.Count > 0)
            {
                equipmentAppointmentRequestStore.AddEquipmentAppointment(addEquipmentsAppointments);
                readyEqpOperation.Appointment.Operation.EquipmentAppointmentRequest.AddRange(addEquipmentsAppointments);
            }
            return readyEqpOperation.Appointment.Operation.EquipmentAppointmentRequest;
        }

        public List<Operation> UpdateEquipmentForAppointment(List<Operation> operations, List<Equipment> equipments)
        {
            Dictionary<int, int> EqipmentsAmount = new Dictionary<int, int>();
            foreach (var eq in equipments)
                EqipmentsAmount[eq.Id] = eq.MaximumAmount ?? 0;
            foreach (Operation operation in operations)
            {
                bool flag = false;
                if (operation.EquipmentAppointmentRequest.IsNotNull() && operation.EquipmentAppointmentRequest.Count > 0)
                {
                    foreach (var eqp in operation.EquipmentAppointmentRequest)
                    {
                        if (EqipmentsAmount[eqp.EqpId] == 0 || operation.Status == 8)//no left equipment
                        {
                            flag = true;
                            operation.NotEnabledEquipmentsAppointmentRequest.Add(eqp.EqpId);
                        }
                        else
                            EqipmentsAmount[eqp.EqpId]--;
                    }
                }
                if (flag)
                    operation.IsEqpReady = "Error";
                else
                    operation.IsEqpReady = "Enabled";
            }
            return operations;
        }

        public async Task<OperationsResponse> EditOperationAsync(OperationRecord operationRecord, OperationsResponse operatinQueue, int organizationId, Appointment oldAppointment, char state, List<Equipment> equipments, string sqlDMConnection1)
        {
            var conflictsList = operatinQueue.ConflictList;
            var OperationLiStToCreateConflicts = new List<Appointment>();
            DateTime schdulTime = operationRecord.BeginTime;
            Appointment appointment = new Appointment(), oldAppointmentResult = new Appointment();
            //conflictsList = operatinQueue.ConflictList;
            OperationsResponse operationsResponse = new OperationsResponse();
            bool isEditOperationBetweenTwoOperations = false, move = false;
            if (state == 'S')
                operationRecord.BeginTime = oldAppointment.Operation.AnesthesiaOrigBeginTime;
            try
            {
                List<Appointment> appointmentsByRoom = opeartionsLogic.GetAllOperationsByRoom(operatinQueue.OperationsList, operationRecord.LocationCode);

                var currentAppointmentIndex = appointmentsByRoom.FindIndex(app => app.Id == oldAppointment.Id);
                opeartionsLogic.GetconflictsAndOperationFromOperationQueue(operatinQueue.ConflictList, operatinQueue.OperationsList);//fill in operatioLogic apointments and conflicts
                appointment = await opeartionsLogic.CreateOpeartionByOprationRecordAsync(operationRecord, operatinQueue.ServiceQueue.Id, operatinQueue.ServiceQueue.BeginTime, organizationId, state, null);
                //the room location and begin time not change and duration change Execution moved all operation in this room
                if (oldAppointment.Operation.RoomId == operationRecord.LocationCode && oldAppointment.BeginTime == schdulTime)
                    move = true;
                else
                {
                    if (state != 'S')
                    {
                        if (oldAppointment.Operation.RoomId == operationRecord.LocationCode)//change beginTime
                        {
                            if (appointmentsByRoom.Count > 1)
                            {
                                var appByRoomHelp = appointmentsByRoom.Where(app => app.Id != oldAppointment.Id).ToList();
                                isEditOperationBetweenTwoOperations = CheakEditOperationBetweenTwoOperations(appByRoomHelp, appointment);
                            }
                            else
                                isEditOperationBetweenTwoOperations = true;//room not have more operation
                        }
                        else if (appointmentsByRoom.Count > 0)
                            isEditOperationBetweenTwoOperations = CheakEditOperationBetweenTwoOperations(appointmentsByRoom, appointment);
                        else
                            isEditOperationBetweenTwoOperations = true;//room not have operation
                    }

                }

                if (move || isEditOperationBetweenTwoOperations)//can move all operation or push him between two operations
                {
                    oldAppointmentResult = appointmentStore.RemoveAppointment(oldAppointment.Id);
                    operationConflictLogic.RemoveConflictByAppointmentId(oldAppointment.Id, operatinQueue.ConflictList);
                    Context.SaveChanges();
                    operatinQueue.OperationsList.Remove(oldAppointmentResult);//remove from operationList
                    if (!isEditOperationBetweenTwoOperations)
                        operatinQueue.OperationsList.Add(appointment);//add new appointment   
                    opeartionsLogic.FillOperationServiseProviders(appointment, organizationId);
                    opeartionsLogic.FiilOperationDepartments(appointment);
                    if (operatinQueue.OperationsList.Count > 0 && state != 'S')
                    {
                        List<Conflict> conflicts = new List<Conflict>();
                        OperationLiStToCreateConflicts = operatinQueue.OperationsList.Where(op => op.Id != appointment.Id).ToList();
                        conflicts = opeartionsLogic.CreateConflictsByOpration(OperationLiStToCreateConflicts, appointment);
                        conflictsList.AddRange(conflicts);
                    }
                }
                else
                {
                    opeartionsLogic.FillOperationServiseProviders(oldAppointment, organizationId);
                    opeartionsLogic.FiilOperationDepartments(oldAppointment);
                    operatinQueue.OperationsList.Remove(appointment);
                }
                operationsResponse.ConflictList = conflictsList;
                operationsResponse.ServiceQueue = operatinQueue.ServiceQueue;
                operationsResponse.OperationsList = operatinQueue.OperationsList;
                //if (state == 'S')
                //   return newInllayLogic.Reschedule(operationsResponse, schdulTime, equipments);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return operationsResponse;
        }

        private bool IsMoveOperationsInRoomId(List<Appointment> appointmentsByRoom, Appointment appointment, int currentAppointmentIndex, DateTime serviceQEndTime)
        {
            DateTime operationsEndTime = new DateTime();
            List<Appointment> AppointmentsChang = new List<Appointment>();//all appointments that change time

            int i;
            //appointments change with one after him cheak
            if (appointment.EndTime > appointmentsByRoom[currentAppointmentIndex + 1].BeginTime)
            {
                var newOperation = appointmentsByRoom[currentAppointmentIndex + 1];
                operationsEndTime = appointment.EndTime.AddSeconds((int)newOperation.Operation.Duration.Value * 60);//.AddSeconds((int)(newOperation.Operation.CleanOrigEndTime - newOperation.Operation.CleanOrigBeginTime).TotalSeconds);
            }
            try
            {
                //another appointments in this room
                for (i = currentAppointmentIndex + 1; i < appointmentsByRoom.Count - 1; i++)
                {
                    if (operationsEndTime > appointmentsByRoom[i + 1].BeginTime)
                    {
                        var newOperation = appointmentsByRoom[i + 1];
                        operationsEndTime = operationsEndTime.AddSeconds((int)newOperation.Operation.Duration.Value * 60);//.AddSeconds((int)(newOperation.Operation.CleanOrigEndTime - newOperation.Operation.CleanOrigBeginTime).TotalSeconds);
                    }
                    else
                        break;
                }
                //move out of range serviceQ
                if ((operationsEndTime - serviceQEndTime).TotalMinutes > 180 && i == appointmentsByRoom.Count - 1)
                    return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        //move all operation in orig change if it possible
        public bool MoveOperationsInRoomId(List<Appointment> appointmentsByRoom, Appointment appointment, int currentAppointmentIndex, int turnOver, DateTime serviceQEndTime = default(DateTime), char state = 'O')
        {
            DateTime operationsEndTime;
            int i, delay;
            List<Appointment> AppointmentsChang = new List<Appointment>();//all appointments that change time
            //ok to move or not-פונקציה זאת בודקת זליגה של ניתוחים מעבר לסיום המשמרת
            //bool isMoveOperationsInRoomId = IsMoveOperationsInRoomId(appointmentsByRoom, appointment, currentAppointmentIndex, serviceQEndTime);

            if (true)//if (isMoveOperationsInRoomId)
            {
                if (appointmentsByRoom[currentAppointmentIndex + 1].IsNotNull() && turnOver > 15)//appointmentsByRoom[i]=end 10 appointmentsByRoom[i + 1]=start 11 appointmentsByRoom[i + 1].Operation.Delay=20
                    delay = turnOver;
                else
                    delay = 15;
                //appointments change with one after him cheak
                if (appointment.EndTime.AddMinutes(delay) > appointmentsByRoom[currentAppointmentIndex + 1].BeginTime)//c=9:15 n=9:00
                {
                    var newOperation = appointmentsByRoom[currentAppointmentIndex + 1];//after= 9:00
                    //update times after move
                    appointmentsByRoom[currentAppointmentIndex + 1].Operation = FillOrigTimesOperation(appointment.EndTime.AddMinutes(delay), appointmentsByRoom[currentAppointmentIndex + 1].Operation.Duration.Value, appointmentsByRoom[currentAppointmentIndex + 1].Operation);
                    appointmentsByRoom[currentAppointmentIndex + 1].BeginTime = appointmentsByRoom[currentAppointmentIndex + 1].Operation.AnesthesiaOrigBeginTime;
                    appointmentsByRoom[currentAppointmentIndex + 1].EndTime = appointmentsByRoom[currentAppointmentIndex + 1].Operation.SurgeryOrigEndTime;
                    AppointmentsChang.Add(appointmentsByRoom[currentAppointmentIndex + 1]);
                }
                try
                {
                    //another appointments in this room
                    for (i = currentAppointmentIndex + 1; i < appointmentsByRoom.Count - 1; i++)
                    {
                        if (appointmentsByRoom[i + 1].IsNotNull() && appointmentsByRoom[i + 1].Operation.Delay >= 15)//appointmentsByRoom[i]=end 10 appointmentsByRoom[i + 1]=start 11 appointmentsByRoom[i + 1].Operation.Delay=20
                            delay = appointmentsByRoom[i + 1].Operation.Delay.Value;//20 
                        else
                            delay = 15;
                        if (appointmentsByRoom[i].EndTime.AddMinutes(delay) > appointmentsByRoom[i + 1].BeginTime)//10:15-after move+20 minutes of delay after him>11:false
                        {
                            var newOperation = appointmentsByRoom[i + 1];//after
                            operationsEndTime = appointmentsByRoom[i].EndTime.AddMinutes(newOperation.Operation.Duration.Value);//.AddSeconds((int)(newOperation.Operation.CleanOrigEndTime - newOperation.Operation.CleanOrigBeginTime).TotalSeconds);
                            appointmentsByRoom[i + 1].Operation = FillOrigTimesOperation(appointmentsByRoom[i].EndTime.AddMinutes(delay), appointmentsByRoom[i + 1].Operation.Duration.Value, appointmentsByRoom[i + 1].Operation);
                            appointmentsByRoom[i + 1].BeginTime = appointmentsByRoom[i + 1].Operation.AnesthesiaOrigBeginTime;
                            appointmentsByRoom[i + 1].EndTime = appointmentsByRoom[i + 1].Operation.SurgeryOrigEndTime;
                            AppointmentsChang.Add(appointmentsByRoom[i + 1]);
                        }
                        else
                            break;
                    }
                    //move out of range serviceQ
                    //if (appointmentsByRoom[appointmentsByRoom.Count - 1].Operation.CleanOrigEndTime > serviceQEndTime && i == appointmentsByRoom.Count - 1)
                    //    return false;

                    if (AppointmentsChang.Count > 0)
                        appointmentStore.UpdateAppointmentsOrigTimes(AppointmentsChang);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return true;
            }
            return false;
        }

        private Operation FillOrigTimesOperation(DateTime anesthesiaOrigBeginTime, int oldOperationDuration, Operation newOperation)
        {

            DateTime time = anesthesiaOrigBeginTime;
            newOperation.AnesthesiaOrigBeginTime = time;
            newOperation.SurgeryOrigBeginTime = time;
            //int cleanDuration = (int)(newOperation.CleanOrigEndTime - newOperation.CleanOrigBeginTime).TotalSeconds;
            time = time.AddSeconds((int)oldOperationDuration * 60);
            newOperation.AnesthesiaOrigEndTime = time;
            newOperation.SurgeryOrigEndTime = time;
            return newOperation;
        }

        private Operation FillOrigTimesOperationReschedule(DateTime anesthesiaSchrduleBeginTime, int oldOperationDuration, Operation newOperation)
        {

            DateTime time = anesthesiaSchrduleBeginTime;
            newOperation.AnesthesiaBeginTime = time;
            newOperation.SurgeryBeginTime = time;
            //int AnesthesiaDuration = (int)(operation.SurgeryOrigBeginTime - operation.AnesthesiaOrigBeginTime).TotalSeconds;
            time = time.AddSeconds((int)oldOperationDuration * 60);
            newOperation.AnesthesiaEndTime = time;
            newOperation.SurgeryEndTime = time;
            return newOperation;
        }

        private bool CheakEditOperationBetweenTwoOperations(List<Appointment> appointmentsByRoom, Appointment appointment)
        {
            Appointment lastAppointment = new Appointment();
            Appointment earlyAppointment = new Appointment();
            if (appointmentsByRoom.IsNotNull() && appointmentsByRoom.Count > 0)//this room have operations
            {
                var earlyAppointments = appointmentsByRoom.Where(app => app.BeginTime <= appointment.BeginTime).OrderBy(app => app.BeginTime).ToList();
                if (earlyAppointments.IsNotNull() && earlyAppointments.Count > 0)
                {
                    earlyAppointment = earlyAppointments[earlyAppointments.Count - 1];//last of operation that is The closest to this appoinment
                    if (earlyAppointment.BeginTime == appointment.BeginTime)
                        return false;
                    //begin time between last of operation that is The closest to this appoinment (just in orig)
                    if (appointment.BeginTime > earlyAppointment.BeginTime && appointment.BeginTime < earlyAppointment.EndTime)
                        return false;
                    //end time between last of operation that is The closest to this appoinment (just in orig)
                    if (appointment.EndTime > earlyAppointment.BeginTime && appointment.EndTime < earlyAppointment.EndTime)
                        return false;
                }
                else//all operation begin after new operation
                {
                    lastAppointment = appointmentsByRoom[0];//reuied to be firsdt in the room
                    if (appointment.EndTime > lastAppointment.BeginTime)//new operation is over on operation after him
                        return false;
                }
                if (earlyAppointments.IsNull() || earlyAppointments.Count == 0)//if this room not have operation before and the operation after not broblem
                    return true;
                else//operation before and after
                {
                    var lastAppointments = appointmentsByRoom.Where(app => app.BeginTime > appointment.BeginTime).OrderBy(app => app.BeginTime).ToList();//all operation after
                    if (lastAppointments.IsNotNull() && lastAppointments.Count > 0)
                    {
                        lastAppointment = lastAppointments[0]; //One operation after another
                                                               //can't be between two operation
                        if (!(appointment.BeginTime >= earlyAppointment.EndTime && appointment.BeginTime <= lastAppointment.BeginTime))
                            return false;
                    }
                }
            }
            return true;
        }
    }

}
