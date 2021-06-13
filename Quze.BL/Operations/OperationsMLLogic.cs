using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using Quze.Models.ML;
using Quze.Models.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static Quze.DAL.Stores.OperationMLStore;

namespace Quze.BL.Operations
{
    public class OperationsMLLogic
    {
        private OperationRequestML operationRequestML;
        private Appointment appointment;
        private OperationMLStore operationMLStore;
        private RequestMLConverts requestMLConverts;
        private List<Appointment> operationList;
        private Fellow fellow;
        private Operation operation;
        private ServiceType serviceType;
        private OperationRecord item;
        private Appointment previousAppointment;
        private char state;


        public OperationsMLLogic(Appointment _appointment, List<Appointment> _operationList, Fellow _fellow, Operation _operation, ServiceType _serviceType, OperationRecord _item, QuzeContext ctx, char _state='O')
        {
            operationRequestML = new OperationRequestML();
            appointment = _appointment;
            requestMLConverts = new RequestMLConverts();
            operationMLStore = new OperationMLStore(ctx);
            operationList = _operationList;
            fellow = _fellow;
            operation = _operation;
            serviceType = _serviceType;
            item = _item;
            state =_state;
            previousAppointment = GetPreviousAppointment(_operationList, _appointment);
        }

        private Appointment GetPreviousAppointment(List<Appointment> operationList, Appointment appointment)
        {
            var operationsByRoom = operationList.Where(app => app.Operation.RoomId == item.LocationCode).OrderBy(app => app.BeginTime).ToList();
            if (operationsByRoom.Count > 0)
            {
                var CurrAppointment = operationsByRoom.FindLast(app => app.BeginTime < item.BeginTime);
                if (CurrAppointment.IsNotNull())
                    return CurrAppointment;
            }
            return null;
        }

        //convert Operation to requestML
        public OperationRequestML InitOperationRequest()
        {

            InitOperationRequestFellow();
            InitOperationRequestAppointment();
            InitOperationRequestFirstAndLastRoomAndServiceProvider();
            operationRequestML.RoomId = appointment.Operation.RoomId > 10 ? appointment.Operation.RoomId - 10 : appointment.Operation.RoomId;
            InitOperationRequestServiceProviders();
            InitOperationRequestDepartment();
            ServicePricedure();
            InitPreviousAppointment();
            return operationRequestML;
        }

        private void InitPreviousAppointment()
        {
            if (previousAppointment.IsNotNull())
            {
                operationRequestML.PreviousMenateach = int.Parse(previousAppointment.Operation.Surgeon.IdentityNumber);
                operationRequestML.PreviousMardim = int.Parse(previousAppointment.Operation.Anesthesiologist.IdentityNumber);
                operationRequestML.PreviousMachlakaMenatachatID = int.Parse(previousAppointment.Operation.SurgicalDepartment.Code);
                operationRequestML.PreviousYechidaRefuitID = int.Parse(previousAppointment.Operation.HostingDepartment.Code);
                operationRequestML.PreviousYechidaSiuditID = previousAppointment.Operation.NursingUnit.IsNotNull() ? int.Parse(previousAppointment.Operation.NursingUnit.Code) : 0;
                operationRequestML.PreviousICD9 = previousAppointment.ServiceType.Code.Split('.')[0];
                operationRequestML.PreviousGilMetupal = previousAppointment.Fellow.Age.Value; //previousAppointment.Fellow.Age.Value.IsNotNull() ? previousAppointment.Fellow.Age.Value : 0;
                operationRequestML.PreviousGender = previousAppointment.Fellow.Gender;
                operationRequestML.PreviousGova = previousAppointment.Fellow.Height.IsNotNull() ? previousAppointment.Fellow.Height.Value : 0;
                operationRequestML.PreviousMishkal = previousAppointment.Fellow.Weight.IsNotNull() ? previousAppointment.Fellow.Weight.Value : 0;
                operationRequestML.PreviousDiabetis = previousAppointment.Fellow.diabetis.Value;
                operationRequestML.PreviousHypertension = previousAppointment.Fellow.hypertension.Value;
            }

        }

        //Init Fellow
        public void InitOperationRequestFellow()
        {
            operationRequestML.FellowAge = fellow.Age.Value;
            operationRequestML.FellowGender = fellow.Gender;
            operationRequestML.FellowWeight = fellow.Weight.IsNotNull() ? fellow.Weight.Value : 0;
            operationRequestML.FellowHeight = fellow.Height.IsNotNull() ? fellow.Height.Value : 0;
        }

        //Init Appointment
        public void InitOperationRequestAppointment()
        {
            string AMOrPM= appointment.BeginTime.ToString("tt", CultureInfo.InvariantCulture);
            string t = appointment.BeginTime.ToString("H:mm");
            t = t.Replace(':', '.');
            float beginTime = float.Parse(t);
            if (AMOrPM=="PM"&&t[0]==12)
            {
                string b="0." + (t[1] / 6) * 60;
                beginTime = float.Parse(b);

            }
                operationRequestML.AppointmentHour = beginTime;
                operationRequestML.AppointmentMonth = appointment.BeginTime.Month;
                operationRequestML.AppointmentWeekDay = (int)appointment.BeginTime.DayOfWeek + 1;

        }

        //Init First And Last Room And Serviceprovider
        public void InitOperationRequestFirstAndLastRoomAndServiceProvider()
        {
            OpeartionStateInQueue opeartionStateInQueue = IsOpeartionFirstInRoom(appointment, operationList, operation,state);
            operationRequestML.IsRoomFirstCase = opeartionStateInQueue == OpeartionStateInQueue.Firts || opeartionStateInQueue == OpeartionStateInQueue.FirstAndLast;
            operationRequestML.IsRoomLastCase = opeartionStateInQueue == OpeartionStateInQueue.Last || opeartionStateInQueue == OpeartionStateInQueue.FirstAndLast;
            opeartionStateInQueue = IsOpeartionFirstToServiceProvider(appointment, operationList);
            operationRequestML.IsSerProvFirstCase = opeartionStateInQueue == OpeartionStateInQueue.Firts || opeartionStateInQueue == OpeartionStateInQueue.FirstAndLast;
            operationRequestML.IsSerProvLastCase = opeartionStateInQueue == OpeartionStateInQueue.Last || opeartionStateInQueue == OpeartionStateInQueue.FirstAndLast;
        }


        //Init ServiceProvider
        public void InitOperationRequestServiceProviders()
        {
            operationRequestML.ServiceProviderId = int.Parse(item.SurgeonId);
            operationRequestML.ServiceProviderAssistantId = int.Parse(item.AnesthesiologistId);
        }

        //Init Department
        public void InitOperationRequestDepartment()
        {
            operationRequestML.DepDepartmentId = int.Parse(item.HostingDepartmentId);
            operationRequestML.DepTreatmentDepartmentId = int.Parse(item.SurgicalDepartmentId);
            operationRequestML.DepNursingDepartmentId = item.NursingUnitDepartmentId.IsNotNull() ? int.Parse(item.NursingUnitDepartmentId) : 0;
        }

        //Init ServicePricedure
        public void ServicePricedure()
        {
            string[] strList;
            if (serviceType != null)
                if (serviceType.Code != null && serviceType.Code.IndexOf('.') >= 0)
                {

                    strList = serviceType.Code.Split('.');
                    operationRequestML.ServiceMainPricedure = strList[0];
                    operationRequestML.ServiceSecondaryPricedure = strList[1];
                }
                else if (serviceType.Code != null)
                {
                    operationRequestML.ServiceMainPricedure = serviceType.Code;
                    operationRequestML.ServiceSecondaryPricedure = "0";

                }
                else
                    operationRequestML.ServiceSecondaryPricedure = operationRequestML.ServiceSecondaryPricedure = "-1";

        }


    }
}

