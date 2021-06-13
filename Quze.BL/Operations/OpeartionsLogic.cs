using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Models;
using Quze.Models.Models;
using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using Quze.Models.ML;
using Newtonsoft.Json;
using Quze.Infrastruture.Extensions;
using Microsoft.Extensions.Configuration;
using System.IO;
using Quze.Models.Logic;
using AutoMapper;

namespace Quze.BL.Operations
{
    public class AddOperationsRequest : Request<OperationRecord>
    {
        public int ServiceQueueId;
        public bool SortOrderByBeginTime;
        public DateTime BeginTime;
        public OperationsResponse OperationQueue;
        public int? OrganizationId;
        public char State;
    }

    public class AddOperationsResponse
    {
        public List<Appointment> OperationsList;
        public List<Conflict> ConflictList;
        public ServiceQueue ServiceQueue;

    }

    public class IsTeamReady
    {
        public TeamReady TeamReady;
        public string State;
        public int OperationId;
    }

    public class OpeartionsLogic
    {
        private string URL = "";

        private readonly ServiceProviderStore serviceProviderStore;
        public AddOperationsRequest request { get; set; }
        public OperationsResponse response { get; set; }
        private QuzeContext context;
        private OperationQueueStore OperationsST;
        private ServiceTypeStore ServiceTypeST;
        private FellowStore fellowStore;
        private MinimalKitLogic minimalKitLogic;
        private AppointmentStore appointmentStore;
        private ServiceQueueStore serviceQueueStore;
        private List<Conflict> conflictsList;
        private List<Appointment> operationList;
        private OperationConflictLogic operationConflictLogic;
        private ServiceStationStore serviceStationStore;
        private DepartmentsStore departmentsStore;
        private EditOperationLogic editOperationLogic;


        int AnesthesiaDuration = 1000;
        int CleanDuration = 500;
        int SurgeyDuration = 3000;

        public OpeartionsLogic(QuzeContext context, IConfiguration configuration)
        {
            this.context = context;
            fellowStore = new FellowStore(context);
            minimalKitLogic = new MinimalKitLogic(context, configuration);
            OperationsST = new OperationQueueStore(context);
            ServiceTypeST = new ServiceTypeStore(context);
            appointmentStore = new AppointmentStore(context);
            serviceQueueStore = new ServiceQueueStore(context);
            serviceProviderStore = new ServiceProviderStore(context);
            operationConflictLogic = new OperationConflictLogic(context);
            serviceStationStore = new ServiceStationStore(context);
            departmentsStore = new DepartmentsStore(context);
            editOperationLogic = new EditOperationLogic(context);
            conflictsList = new List<Conflict>();
            operationList = new List<Appointment>();
            URL = configuration.GetSection("AppSettings").GetValue<string>("ML_URL");
        }
        public OpeartionsLogic(QuzeContext context)
        {
            this.context = context;
            fellowStore = new FellowStore(context);
            OperationsST = new OperationQueueStore(context);
            ServiceTypeST = new ServiceTypeStore(context);
            appointmentStore = new AppointmentStore(context);
            serviceQueueStore = new ServiceQueueStore(context);
            serviceProviderStore = new ServiceProviderStore(context);
            operationConflictLogic = new OperationConflictLogic(context);
            serviceStationStore = new ServiceStationStore(context);
            departmentsStore = new DepartmentsStore(context);
            conflictsList = new List<Conflict>();
            operationList = new List<Appointment>();
        }
        public async Task<OperationsResponse> AddOperation(int serviceQueueId, DateTime beginTime, int organizationId, OperationRecord operationRecord, char state)
        {
            bool isExistsServiceQueue = false;
            //1.create SQ
            ServiceQueue serviceQueue;
            OperationsResponse operationsResponse = new OperationsResponse();
            try
            {
                //getExist serviceQ or create
                if (serviceQueueId > 0)
                {
                    serviceQueue = serviceQueueStore.GetServiceQ(serviceQueueId);
                    if (request.OperationQueue != null)
                        GetconflictsAndOperationFromOperationQueue(request.OperationQueue.ConflictList, request.OperationQueue.OperationsList);
                }
                else
                    serviceQueue = serviceQueueStore.GetServiceQ(beginTime, organizationId);
                if (serviceQueue == null)
                    serviceQueue = serviceQueueStore.CreateOpeartionQueue(beginTime, organizationId);
                else if (operationList.IsNull() || operationList.Count == 0)
                {
                    isExistsServiceQueue = true;
                    operationsResponse = OperationsST.GetOperationsByServiceQId(serviceQueue.Id, organizationId);
                    GetconflictsAndOperationFromOperationQueue(operationsResponse.ConflictList, operationsResponse.OperationsList);
                }

                //2.  Create Opeartion
                Appointment appointment = await CreateOpeartionByOprationRecordAsync(operationRecord, serviceQueue.Id, serviceQueue.BeginTime, organizationId, state, null);
                context.SaveChanges();
                FillOperationServiseProviders(appointment, organizationId);
                FiilOperationDepartments(appointment);

                if (operationsResponse.IsNotNull() && operationList.Count > 0)
                {
                    var OperationLiStToCreateConflicts = operationList.Where(op => op.Id != appointment.Id).ToList();
                    var conflicts = CreateConflictsByOpration(OperationLiStToCreateConflicts, appointment);
                    conflictsList.AddRange(conflicts);
                }
                operationsResponse.ConflictList = conflictsList;
                operationsResponse.ServiceQueue = serviceQueue;
                operationsResponse.OperationsList = operationList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return operationsResponse;

        }

        public void FiilOperationDepartments(Appointment appointment)
        {
            appointment.Operation.SurgicalDepartment = departmentsStore.GetDepartmentById(appointment.Operation.SurgicalDepartmentId);
            appointment.Operation.HostingDepartment = departmentsStore.GetDepartmentById(appointment.Operation.HostingDepartmentId);
            appointment.Operation.NursingUnit = appointment.Operation.NursingUnitId.IsNotNull() ? departmentsStore.GetDepartmentById(appointment.Operation.NursingUnitId.Value) : null;

        }

        public void GetconflictsAndOperationFromOperationQueue(List<Conflict> conflictList, List<Appointment> appointment)
        {
            if (conflictList.IsNotNull() && conflictList.Count > 0)
                conflictsList = conflictList;
            operationList = appointment;
        }

        public void FillOperationServiseProviders(Appointment appointment, int organizationId)
        {
            appointment.Operation.Surgeon = serviceProviderStore.GetServiceProviderById(appointment.Operation.SurgeonId, organizationId);
            appointment.Operation.Anesthesiologist = serviceProviderStore.GetServiceProviderById(appointment.Operation.AnesthesiologistId, organizationId);
            appointment.Operation.Nurse = appointment.Operation.NurseId.IsNotNull()? serviceProviderStore.GetServiceProviderById(appointment.Operation.NurseId.Value, organizationId):null;
        }

        public TeamReady UpdateTeamReady(IsTeamReady isTeamReady)
        {
            TeamReady team;
            Operation operation = OperationsST.GetOperationById(isTeamReady.OperationId);//get operation by teamReady I
            if (operation.TeamReadyId != null)//For this operation, a ready-made team update
            {
                team = OperationsST.UpdateTeamReady(isTeamReady.TeamReady);//Updates each team whether it is ready or not
            }
            else//firstv time team ready update
                team = OperationsST.AddTeamReady(isTeamReady.TeamReady, operation);//add new team ready
            operation.IsTeamReady = isTeamReady.State;//update new state
            context.SaveChanges();
            return team;
        }

        // create operatiion from EXCEL
        public async Task<OperationsResponse> CraeteOperationsQueue(string sqlDMConnection, char state)//AddOperationsRequest request
        {
            OperationsResponse operationsResponse = new OperationsResponse();
            ServiceProviderStore ServiceProviderDetails = new ServiceProviderStore(context);
            List<Operation> operationsBySortOrder = new List<Operation>();
            bool isExistsServiceQueue = false;

            //1. create ServiceQueue
            ServiceQueue OperationServiceQueue;
            if (request.ServiceQueueId == -1)
            {
                OperationServiceQueue = serviceQueueStore.GetServiceQ(request.BeginTime, request.OrganizationId.Value);
                if (OperationServiceQueue == null)//create new ServiceQueue
                    OperationServiceQueue = serviceQueueStore.CreateOpeartionQueue(request.BeginTime, request.OrganizationId.Value);
                else if (operationList.IsNull() || operationList.Count == 0)//  צריך לשלוף אופרישן של התאריך הנוכחי 
                {
                    OperationServiceQueue = serviceQueueStore.GetServiceQ(request.BeginTime, request.OrganizationId.Value);
                    isExistsServiceQueue = true;
                }
            }
            else// Adds Service Queue to the same SQ at display in calendar
            {
                OperationServiceQueue = request.OperationQueue.ServiceQueue;
                GetconflictsAndOperationFromOperationQueue(request.OperationQueue.ConflictList, request.OperationQueue.OperationsList);
            }
            int serviceQueuId = OperationServiceQueue.Id;


            if (!request.SortOrderByBeginTime)
            {
                operationsBySortOrder = this.ConvertRecordToOperation(request);
                RescheduleStore RS = new RescheduleStore(context);
                operationsBySortOrder = RS.InitTimeAfterRescheduling(operationsBySortOrder, request.BeginTime, 'O');
            }

            if (isExistsServiceQueue)
            {
                operationsResponse = OperationsST.GetOperationsByServiceQId(serviceQueuId, request.OrganizationId.Value);
                GetconflictsAndOperationFromOperationQueue(operationsResponse.ConflictList, operationsResponse.OperationsList);
            }

            //2. create operation
            List<Appointment> CreatedOperationsList = new List<Appointment>();
            foreach (var operationRecord in request.Entities)
            {
                Appointment appointment = await CreateOpeartionByOprationRecordAsync(operationRecord, serviceQueuId, OperationServiceQueue.BeginTime, request.OrganizationId.Value, state, operationsBySortOrder);//operationsBySortOrder
                CreatedOperationsList.Add(appointment);
            }
            context.SaveChanges();
            foreach (var ap in CreatedOperationsList)
                FillOperationServiseProviders(ap, request.OrganizationId.Value);
            //4. find conflict
            if (operationList.Count > 0)
            {
                var OperationLiStToCreateConflicts = operationList.Where(op => op.Id != CreatedOperationsList[0].Id).ToList();
                var conflicts = CreateConflictsByOpration(OperationLiStToCreateConflicts, CreatedOperationsList[0]);
                conflictsList.AddRange(conflicts);
            }

            for (int i = 1; i < CreatedOperationsList.Count; i++)
            {
                Appointment appointment = CreatedOperationsList[i];
                var OperationLiStToCreateConflicts = operationList.Where(op => op.Id != appointment.Id).ToList();
                var conflicts = CreateConflictsByOpration(OperationLiStToCreateConflicts, appointment);
                conflictsList.AddRange(conflicts);
            }

            OperationServiceQueue.Appointments = null;
            operationsResponse.ServiceQueue = new ServiceQueue();
            operationsResponse.ServiceQueue = OperationServiceQueue;
            operationsResponse.OperationsList = operationList;
            operationsResponse.ConflictList = conflictsList;
            return operationsResponse;
        }


        public async Task<Appointment> CreateOpeartionByOprationRecordAsync(OperationRecord item, int serviceQueuId, DateTime serviceQueuBeginTime, int organizationId, char state, List<Operation> operationsBySortOrder = null)
        {
            //1.calculate durations
            if (item.OperationDuration != 0)
                SurgeyDuration = item.OperationDuration * 60;
            AnesthesiaDuration = GetAnesthesiaDuration(SurgeyDuration);
            CleanDuration = GetCleaningDuration(SurgeyDuration);
            //3.create opration if not exsist in  operationsBySortOrder
            Operation operation = null;
            if (operationsBySortOrder != null && operationsBySortOrder.Count > 0)
            {
                operation = operationsBySortOrder.First(o => o.Code == item.Code);
                operation.Delay = item.Delay ?? 0;
                operation.IsDelay = item.AsDelay ?? false;
                operation.Duration = item.OperationDuration;
                operation = OperationsST.CreateOpration(operation);
            }
            else
                operation = CreateOpration(item);
            operation.IsDelay = operation.IsDelay;
            //4. create fellow                      
            Fellow fellow = fellowStore.AddFellow(item, organizationId, true);
            // 5.add appointment to SQL & operations list
            ServiceType ServiceType = ServiceTypeST.GetServiceTypeOperation(item.OperationTypeCode);
            int serviceTypeId = ServiceType.Id;

            Appointment operationApp = appointmentStore.CreateAppointment(item, serviceQueuId, fellow.Id, operation, serviceTypeId);
            //3. get and update ML data
            operationApp = await GetMLDataAPIRequestAsync(operationApp, operationList, fellow, operation, ServiceType, serviceQueuBeginTime, item, state);//7 שניות
            operationApp.BeginTime = operation.AnesthesiaOrigBeginTime;
            operationApp.EndTime = operation.SurgeryOrigEndTime;
            appointmentStore.AddAppointment(operationApp);
            operationApp.Operation = operation;
            operationApp.ServiceType = ServiceType;
            minimalKitLogic.GetMinimalKit(operationApp);
            return operationApp;
        }



        public List<Conflict> CreateConflictsByOpration(List<Appointment> CreatedOperationsList, Appointment operationApp)
        {
            // 6.find Confilcts
            List<Conflict> NewOperationConflicts = new List<Conflict>();
            //create conflict just if appointment not Canceled or Done
            if (CreatedOperationsList.Count > 0 && operationApp.Operation.Status != 7 && operationApp.Operation.Status != 8)
                NewOperationConflicts = operationConflictLogic.FindIntersects(operationApp, CreatedOperationsList);
            //6.1 return conflicts list
            return NewOperationConflicts;
        }

        private List<Operation> ConvertRecordToOperation(AddOperationsRequest request)
        {
            List<Operation> operations = new List<Operation>();
            foreach (var item in request.Entities)
            {
                ServiceProvider surgeon = serviceProviderStore.GetServiceProviderById(item.SurgeonCode, request.OrganizationId.Value);
                Operation operation = new Operation()
                {
                    Code = item.Code,
                    AnesthesiaOrigBeginTime = item.BeginTime,
                    AnesthesiaOrigEndTime = item.BeginTime.AddSeconds(SurgeyDuration + AnesthesiaDuration),
                    AnesthesiologistId = item.AnesthesiologistCode,
                    Anesthesiologist = serviceProviderStore.GetServiceProviderById(item.AnesthesiologistCode, request.OrganizationId.Value),
                    CleanOrigBeginTime = item.BeginTime.AddSeconds(AnesthesiaDuration + SurgeyDuration),
                    CleanOrigEndTime = item.BeginTime.AddSeconds(AnesthesiaDuration + SurgeyDuration + CleanDuration),
                    CleanTeamId = item.CleanTeamCode,
                    SurgeryOrigBeginTime = item.BeginTime.AddSeconds(AnesthesiaDuration),
                    SurgeryOrigEndTime = item.BeginTime.AddSeconds(AnesthesiaDuration + SurgeyDuration),
                    SurgeonId = item.SurgeonCode,
                    Surgeon = surgeon,
                    RoomId = item.LocationCode,
                    Priority = item.Priority,
                    NurseId = item.SurgicalNursCode.Value,
                    Nurse = item.SurgicalNursCode == 0 ? null : serviceProviderStore.GetServiceProviderById(item.SurgicalNursCode.Value, request.OrganizationId.Value),
                    IsHBP = item.IsHBP,
                    IsTraining = item.IsTraining,
                    IsXrayDeviceRequired = item.IsXrayDeviceRequired,
                    SortOrder = item.SortOrder,
                    SurgicalDepartmentId = item.SurgicalDepartmentCode,
                    HostingDepartmentId = item.HostingDepartmentCode,
                    NursingUnitId = item.NursingUnitDepartmentCode,
                    OperationDuration = item.OperationDuration * 60,
                    AnesthesiaDuration = GetAnesthesiaDuration(item.OperationDuration * 60),
                    CleanDuration = GetCleaningDuration(item.OperationDuration * 60)
                };
                operations.Add(operation);
            }
            return operations;
        }

        public int GetAnesthesiaDuration(int surgeryDuration)
        {
            if (surgeryDuration <= 60 * 60)
                return 3 * 60;
            else if (surgeryDuration <= 120 * 60)
                return 5 * 60;
            else
                return 5 * 60;

        }

        public int GetCleaningDuration(int surgeryDuration)
        {
            return 15 * 60;
        }


        public async Task<Appointment> GetMLDataAPIRequestAsync(Appointment oprationAppointment, List<Appointment> _operationList, Fellow fellow, Operation operationML, ServiceType serviceType, DateTime serviceQueueBeginTime, OperationRecord item, char state, bool isSaveDB = true)
        {
            List<Appointment> myRoomsList;
            OperationsMLLogic operationsMLLogic;
            operationsMLLogic = new OperationsMLLogic(oprationAppointment, _operationList, fellow, operationML, serviceType, item, context, state);
            var req = operationsMLLogic.InitOperationRequest();
            string json = JsonConvert.SerializeObject(req);
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, URL))
            {
                //request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMjM3ODEiLCJ1bmlxdWVfbmFtZSI6IjA1MjcxNzQyMTAiLCJ0eXAiOiIzIiwiZ2l2ZW5fbmFtZSI6Itee15nXm9ecIiwiZmFtaWx5X25hbWUiOiLXkNeR16jXnteV15HXmdelIiwiZW1haWwiOiJhQGEuY29tIiwiT3JnYW5pemF0aW9uSWQiOiI5OTYxIiwiSWRlbnRpdHlOdW1iZXIiOiIxMDAwMDAwMDkiLCJleHAiOjE1ODc4MjA5NjUsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3QiLCJhdWQiOiJBUElfVXNlciJ9.YWxAMpQq_8og--rNlfW65xIegS0AjWft2p1Ewf7vnqM"); //90 days
                request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMjM3ODEiLCJ1bmlxdWVfbmFtZSI6IjA1MjcxNzQyMTAiLCJ0eXAiOiIzIiwiZ2l2ZW5fbmFtZSI6Itee15nXm9ecIiwiZmFtaWx5X25hbWUiOiLXkNeR16jXnteV15HXmdelIiwiZW1haWwiOiJhQGEuY29tIiwiT3JnYW5pemF0aW9uSWQiOiI5OTYxIiwiSWRlbnRpdHlOdW1iZXIiOiIxMDAwMDAwMDkiLCJleHAiOjE5MjU4MTk2ODgsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3QiLCJhdWQiOiJBUElfVXNlciJ9.oOpIAvjKlsCHG5FwbZNeEmTPCZdkpbpxtxY6_5p55kI"); //10 years
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;
                    try
                    {//5 sec
                        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false);
                        {
                            var resultString = response.Content.ReadAsStringAsync().Result;
                            if (response.IsSuccessStatusCode && response.IsSuccessStatusCode)
                            {
                                var mlResponse = JsonConvert.DeserializeObject<ResponseML>(resultString);
                                var operation = context.Entry<Operation>(oprationAppointment.Operation);
                                oprationAppointment.Operation.Duration = mlResponse.responseDuration;
                                oprationAppointment.Operation.IsDelay = mlResponse.AsDelay.Value >= 1;
                                oprationAppointment.Operation.Delay = mlResponse.responseDelayDuration;
                                if (state == 'O')
                                    myRoomsList = _operationList.OrderBy(x => x.Operation.RoomId).ThenBy(x => x.BeginTime).Where(x => x.Operation.RoomId == oprationAppointment.Operation.RoomId).ToList();
                                else
                                    myRoomsList = _operationList.OrderBy(x => x.Operation.RoomId).ThenBy(x => x.Operation.Priority).Where(x => x.Operation.RoomId == oprationAppointment.Operation.RoomId).ToList();
                                System.Diagnostics.Debug.WriteLine(myRoomsList);
                                //int index = myRoomsList.Count == 1 ? 1 : 2;
                                var index = myRoomsList.FindIndex(x => x.BeginTime == oprationAppointment.BeginTime);
                                if (myRoomsList.Count > 1 && index > 0)//not first operation, 2+ operations
                                    index -= 1;
                                if (state == 'O')//orig
                                    oprationAppointment.Operation = EnterTimeByDuration(oprationAppointment, mlResponse.responseDuration, serviceQueueBeginTime, req.IsRoomFirstCase, myRoomsList[index], index, myRoomsList);
                                else//schedule
                                    oprationAppointment.Operation = EnterTimeByDurationSchedule(oprationAppointment, mlResponse.responseDuration, serviceQueueBeginTime, req.IsRoomFirstCase, myRoomsList[index], index, myRoomsList);
                            }
                            else
                                Console.Write("API fails!");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return oprationAppointment;
        }





        public int CalcDifferenceBetweenPrevOperation(DateTime prev, DateTime current)
        {
            TimeSpan minutesDiff;
            int differenceBetweenOperations = 0, hours = 0;
            if (prev <= current)  //prev = 9:10 , current = 9:15 || prev = 8:20 , current = 9:35 פה אנחנו רוצים להתייחס רק לסוף
            {
                minutesDiff = current.Subtract(prev);// 5 min' Commencement of specific surgery less termination were given earlier
                if (minutesDiff.Hours > 0)
                    hours = minutesDiff.Hours * 60;
                differenceBetweenOperations = minutesDiff.Minutes + hours;
                if (differenceBetweenOperations < 15)
                    differenceBetweenOperations = 15 - differenceBetweenOperations;
            }
            return differenceBetweenOperations;

        }


        private int CalcDelay(int delay, int? operationDuration, DateTime prevOperation, DateTime currOperationTime, DateTime serviceQueueBeginTime, Appointment currOperation, int indexOperation, bool isRoomFirstCase, Appointment nextAppointment = null, List<Appointment> myRoomsList = null)
        {
            TimeSpan minutesDiff;
            int differenceBetweenOperations = 0, hours = 0, timeTurnOver = 0;
            if (currOperation.Operation.Delay < 15)
                currOperation.Operation.Delay = 15;
            if (isRoomFirstCase)//first operation in the room
            {
                if (delay < 0)
                    timeTurnOver = 0;
                else
                {
                    minutesDiff = currOperationTime.Subtract(serviceQueueBeginTime);// 5 min' Commencement of specific surgery less termination were given earlier
                    if (minutesDiff.Hours > 0)
                        hours = minutesDiff.Hours * 60;
                    differenceBetweenOperations = minutesDiff.Minutes + hours;//5
                    timeTurnOver = CalcTimeTurnOver(differenceBetweenOperations, delay);
                }
            }
            else if (prevOperation <= currOperationTime)  //prev = 9:10 , current = 9:15 || prev = 8:20 , current = 9:35 פה אנחנו רוצים להתייחס רק לסוף
            {

                minutesDiff = currOperationTime.Subtract(prevOperation);// 5 min' Commencement of specific surgery less termination were given earlier
                if (minutesDiff.Hours > 0)
                    hours = minutesDiff.Hours * 60;
                differenceBetweenOperations = minutesDiff.Minutes + hours;//5
                timeTurnOver = CalcTimeTurnOver(differenceBetweenOperations, delay);
            }
            if (nextAppointment.IsNotNull())//have operation after
            {
                var currentAppointmentEndTime = currOperation.BeginTime.AddMinutes(timeTurnOver).AddMinutes(operationDuration.Value);
                currOperation.EndTime = currentAppointmentEndTime;
                if (nextAppointment.BeginTime >= currentAppointmentEndTime)
                    if (nextAppointment.BeginTime == currentAppointmentEndTime)//n=9:15 c=9:15
                    {
                        if (nextAppointment.Operation.Delay < 15)
                            editOperationLogic.MoveOperationsInRoomId(myRoomsList, currOperation, indexOperation, 15); //הוזזה ב15
                        else
                            editOperationLogic.MoveOperationsInRoomId(myRoomsList, currOperation, indexOperation, nextAppointment.Operation.Delay.Value); //הוזה בדילי
                    }
                    else//n=9:15 c=9:10
                    {
                        minutesDiff = nextAppointment.BeginTime.Subtract(currentAppointmentEndTime);//5
                        if (minutesDiff.Hours > 0)
                            hours = minutesDiff.Hours * 60;
                        differenceBetweenOperations = minutesDiff.Minutes + hours;
                        if (differenceBetweenOperations > nextAppointment.Operation.Delay)//dif=13 delay=12
                        {
                            if (differenceBetweenOperations <= 15)
                                editOperationLogic.MoveOperationsInRoomId(myRoomsList, currOperation, indexOperation, 15); //15מסוף הניתוח הספציפי
                        }
                        else//delay>diff   
                        {
                            if (nextAppointment.Operation.Delay < 15)
                                editOperationLogic.MoveOperationsInRoomId(myRoomsList, currOperation, indexOperation, 15); //  הוזזה ב15
                            else
                                editOperationLogic.MoveOperationsInRoomId(myRoomsList, currOperation, indexOperation, nextAppointment.Operation.Delay.Value); //מהוזזה בדילי
                        }
                    }
            }

            return timeTurnOver;
        }

        private int CalcTimeTurnOver(int differenceBetweenOperations, int delay)
        {
            if (differenceBetweenOperations > delay)//60 30 curr:10 prev 9 delay 30
            {
                if (differenceBetweenOperations < 15) //14 12
                    return 15 - differenceBetweenOperations;
                return 0;
            }
            if (delay <= 15) //differenceBetweenOperations 5 11:55 12:00 begin 12:10
                return 15 - differenceBetweenOperations;
            else
                return delay - differenceBetweenOperations;
        }

        public Operation EnterTimeByDuration(Appointment appointment, int? OperationDuration, DateTime serviceQueueBeginTime, bool isRoomFirstCase, Appointment prevOperation, int indexOperation, List<Appointment> myRoomsList)
        {
            Appointment nextAppointment = null;
            if (isRoomFirstCase)
                nextAppointment = myRoomsList.ElementAtOrDefault(indexOperation + 1);
            else
            {
                nextAppointment = myRoomsList.ElementAtOrDefault(indexOperation + 2);
                indexOperation += 1;
            }
            int timeTurnOver = CalcDelay(appointment.Operation.Delay.Value, OperationDuration, prevOperation.EndTime, appointment.BeginTime, serviceQueueBeginTime, appointment, indexOperation, isRoomFirstCase, nextAppointment, myRoomsList);
            DateTime time = appointment.Operation.AnesthesiaOrigBeginTime.AddMinutes((int)timeTurnOver); //-> need to improve
            appointment.Operation.AnesthesiaOrigBeginTime = appointment.Operation.SurgeryOrigBeginTime = time;
            appointment.Operation.AnesthesiaOrigEndTime = appointment.Operation.SurgeryOrigEndTime = time.AddMinutes((int)OperationDuration);
            appointment.BeginTime = appointment.Operation.SurgeryOrigBeginTime;
            appointment.EndTime = appointment.Operation.SurgeryOrigEndTime;
            return appointment.Operation;
        }

        //אני עשיתי אותה אולי נמחק אותה אחרי זה


        public Operation EnterTimeByDurationSchedule(Appointment appointment, int? OperationDuration, DateTime serviceQueueBeginTime, bool isRoomFirstCase, Appointment prevOperation, int indexOperation, List<Appointment> myRoomsList)
        {
            DateTime time;
            int timeTurnOver;
            if (!isRoomFirstCase)
            {
                indexOperation += 1;
                timeTurnOver = CalcDelay(appointment.Operation.Delay.Value, OperationDuration, prevOperation.EndTime, prevOperation.EndTime, serviceQueueBeginTime, appointment, indexOperation, isRoomFirstCase);
            }
            else
                timeTurnOver = CalcDelay(appointment.Operation.Delay.Value, OperationDuration, prevOperation.EndTime, appointment.BeginTime, serviceQueueBeginTime, appointment, indexOperation, isRoomFirstCase);


            if (isRoomFirstCase)
                time = appointment.Operation.AnesthesiaBeginTime.AddMinutes((int)timeTurnOver); //first
            else
                time = prevOperation.EndTime.AddMinutes((int)timeTurnOver); //one after else 
            appointment.Operation.AnesthesiaBeginTime = appointment.Operation.SurgeryBeginTime = time;
            appointment.Operation.AnesthesiaEndTime = appointment.Operation.SurgeryEndTime = time.AddMinutes((int)OperationDuration);
            appointment.BeginTime = appointment.Operation.AnesthesiaBeginTime;
            appointment.EndTime = appointment.Operation.SurgeryEndTime;
            return appointment.Operation;
        }


        public Operation CreateOpration(OperationRecord operationRecord)
        {
            int surgeyDuration = operationRecord.OperationDuration * 60;
            int anesthesiaDuration = GetAnesthesiaDuration(surgeyDuration);
            int cleanDuration = GetCleaningDuration(surgeyDuration);
            Operation operation;
            try
            {
                operation = new Operation()
                {
                    Code = operationRecord.Code,
                    AnesthesiaOrigBeginTime = operationRecord.BeginTime,
                    AnesthesiaOrigEndTime = operationRecord.BeginTime.AddSeconds(surgeyDuration + anesthesiaDuration),
                    AnesthesiologistId = operationRecord.AnesthesiologistCode,
                    CleanOrigBeginTime = operationRecord.BeginTime.AddSeconds(-cleanDuration),
                    CleanOrigEndTime = operationRecord.BeginTime,
                    CleanTeamId = operationRecord.CleanTeamCode,
                    SurgeryOrigBeginTime = operationRecord.BeginTime,
                    SurgeryOrigEndTime = operationRecord.BeginTime.AddSeconds(anesthesiaDuration + surgeyDuration),
                    SurgeonId = operationRecord.SurgeonCode,
                    RoomId = operationRecord.LocationCode,
                    Priority = operationRecord.Priority,
                    NurseId = operationRecord.SurgicalNursCode,
                    IsHBP = operationRecord.IsHBP,
                    IsTraining = operationRecord.IsTraining,
                    IsXrayDeviceRequired = operationRecord.IsXrayDeviceRequired,
                    SortOrder = operationRecord.SortOrder,
                    SurgicalDepartmentId = operationRecord.SurgicalDepartmentCode,
                    HostingDepartmentId = operationRecord.HostingDepartmentCode,
                    NursingUnitId = operationRecord.NursingUnitDepartmentCode,
                    Delay = operationRecord.Delay ?? 0,
                    IsDelay = operationRecord.AsDelay ?? false,
                    Duration = operationRecord.OperationDuration,
                    IsEqpReady = "Enabled"
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return OperationsST.CreateOpration(operation);
        }

        public OperationsResponse GetServiceQueuById(int id, int? organizationId)
        {

            List<Appointment> operationList = new List<Appointment>();
            List<Conflict> conflictsList = new List<Conflict>();
            var operationsResponse = OperationsST.GetOperationsByServiceQId(id, organizationId);

            //create conflict
            if (operationsResponse != null)
                operationsResponse.ConflictList = CreateConflictsByOprationList(operationsResponse.OperationsList);

            return operationsResponse;
        }

        public List<Conflict> CreateConflictsByOprationList(List<Appointment> operationsList)
        {
            List<Appointment> operationList = new List<Appointment>();
            List<Conflict> conflictsList = new List<Conflict>();
            if (operationsList.Count > 0)
            {
                operationList.Add(operationsList[0]);
                for (int j = 1; j < operationsList.Count; j++)
                {
                    Appointment appointment = operationsList[j];
                    var conflicts = CreateConflictsByOpration(operationList, appointment);
                    conflictsList.AddRange(conflicts);
                    operationList.Add(appointment);
                }
            }
            return conflictsList;
        }

        public List<Appointment> GetAllOperationsByRoom(List<Appointment> appontments, int roomId)
        {
            List<Appointment> appointmentsByRoom = appontments.Where(app => app.Operation.RoomId == roomId).OrderBy(app => app.BeginTime).ToList();
            return appointmentsByRoom;
        }
    }
}

