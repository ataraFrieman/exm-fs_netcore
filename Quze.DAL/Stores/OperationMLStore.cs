using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quze.DAL.Stores
{
    public class OperationMLStore : StoreBase<Operation>
    {
        private static QuzeContext ctx;
        public enum OpeartionStateInQueue
        {
            Firts = 0,
            Last = 1,
            FirstAndLast = 2,
            Middle = -1

        }
        public OperationMLStore(QuzeContext context) : base(context)
        {
            ctx = context;
        }
        public static OpeartionStateInQueue IsOpeartionFirstInRoom(Appointment appointment, List<Appointment> operationList, Operation operation, char state = 'O')
        {
            OpeartionStateInQueue opeartionStateInQueue = OpeartionStateInQueue.Middle;
            appointment.Operation = operation;
            //if (state != 'S')
            //    operationList.Add(appointment);
            int indexApp = operationList.FindIndex(app => app.BeginTime == appointment.BeginTime && app.Operation.RoomId == appointment.Operation.RoomId);
            if (indexApp == -1)
                operationList.Add(appointment);
            List<Appointment> appointmentsList = operationList
                .Where(app => app.Operation.RoomId == operation.RoomId).OrderBy(app => app.BeginTime).ToList();
            if (appointmentsList.Count == 1)
                return OpeartionStateInQueue.FirstAndLast;
            if (appointmentsList[0].BeginTime == appointment.BeginTime)
                opeartionStateInQueue = OpeartionStateInQueue.Firts;
            else if (appointmentsList[appointmentsList.Count - 1].BeginTime == appointment.BeginTime)
                opeartionStateInQueue = OpeartionStateInQueue.Last;

            return opeartionStateInQueue;
        }
        public static OpeartionStateInQueue IsOpeartionFirstToServiceProvider(Appointment appointment, List<Appointment> operationList)
        {
            OpeartionStateInQueue opeartionStateInQueue = OpeartionStateInQueue.Middle;
            int indexApp = operationList.FindIndex(app => app.BeginTime == appointment.BeginTime && app.Operation.RoomId == appointment.Operation.RoomId);
            if (indexApp == -1)
                operationList.Add(appointment);
            List <Appointment> appointmentsList = operationList
                .Where(app => app.Operation.SurgeonId == appointment.Operation.SurgeonId ).OrderBy(app => app.BeginTime).ToList();
            if (appointmentsList.Count == 1)
                opeartionStateInQueue = OpeartionStateInQueue.FirstAndLast;
            else if (appointmentsList.Count > 0)
            {
                if (appointmentsList[0].BeginTime== appointment.BeginTime)
                    opeartionStateInQueue = OpeartionStateInQueue.Firts;
                else
                if (appointmentsList[appointmentsList.Count - 1].BeginTime == appointment.BeginTime)
                    opeartionStateInQueue = OpeartionStateInQueue.Last;
            }
            return opeartionStateInQueue;
        }
        public static DataMiningTable getDMObjectFromDB(string connectionString, string condition)
        {
            DataMiningTable dataMining = new DataMiningTable();

            string queryString = "select ID,serviceProviderAverageStartingTimeOfWork,serviceProviderAverageEndTimeOfWork"
                + ",depDepartmentPercNoService,serviceProviderPercNoService,serviceProviderAssistantPercNoService,RoomPercNoService,"
                + "serviceProviderId,serviceProviderAssistantId,depDepartmentId,roomId,"
                + "roomPercDur_Result,servicePercDur_Result,servicMProcPercDur_Result,servicSProcPercDur_Result,serviceProviderPercDur_Result,serviceProviderAssistantPercDur_Result,serviceProviderPercNoServiceFC,serviceProviderAssistantPercNoServiceFC from [DataMining].[dbo].[DataMiningTable] where "
                + condition;

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        object[] objs = new object[11];
                        int quant = reader.GetValues(objs);
                        dataMining = new DataMiningTable(objs);
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            return dataMining;

        }
        public static DatesAndEvents getDateAndEventsFromDB(string connectionString, DateTime beginTime)
        {
            DatesAndEvents datesAndEvents = new DatesAndEvents();
            string dateString = beginTime.Year + "-" + beginTime.Month + "-" + beginTime.Day;
            string queryString = "select gDate,he_IsDayOfRest,he_IsEveOfHoliday,he_isWorkingDay,he_isFast,m_IsDayOfRest,m_IsEveOfHoliday,m_isFast "
                + "from [DataMining].[dbo].[DatesAndEvents] where gDate='" + dateString + "'"; ;
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        object[] objs = new object[11];
                        int quant = reader.GetValues(objs);
                        datesAndEvents = new DatesAndEvents(objs);
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            return datesAndEvents;
        }
        public static void SaveDurationAndAsDelayDB(QuzeContext context, int operationId, int duration, int delay)
        {
            Operation operation = context.Operations.FirstOrDefaultAsync(o => o.Id == operationId).Result;

            if (operation != null)
            {
                operation.Duration = duration;
                operation.Delay = delay;
                DateTime time = operation.AnesthesiaOrigBeginTime;
                time = time.AddSeconds(duration * 60);
                //int cleanDuration = (int)(operation.CleanOrigEndTime - operation.CleanOrigBeginTime).TotalSeconds;
                operation.AnesthesiaOrigEndTime = time;
                operation.SurgeryOrigEndTime = time;
                //operation.CleanOrigBeginTime = time;
                //time = time.AddSeconds(cleanDuration);
                //operation.CleanOrigEndTime = time;
                context.SaveChanges();
            }
        }

    }
}
