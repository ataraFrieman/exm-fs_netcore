using Quze.Models;
using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quze.Models.Models;
using Quze.Infrastruture.Extensions;
using Quze.Models.Logic;
using AutoMapper;
using Quze.Models.Models.ViewModels;

namespace Quze.DAL.Stores
{
    public class OperationsResponse
    {
        public List<Appointment> OperationsList;
        public List<Conflict> ConflictList;
        public ServiceQueue ServiceQueue;
    }



    public class OperationQueueStore : StoreBase<ServiceQueue>
    {
        public OperationQueueStore(QuzeContext ctx) : base(ctx)
        {

        }

        public Operation CreateOpration(Operation operation)
        {
            try
            {
                ctx.Operations.Add(operation);
                //ctx.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return operation;
        }

        public int GetStatuseOperation(string operationCode)
        {
            var operation = ctx.Operations.Where(o => o.Code == operationCode).FirstOrDefault();
            if (operation.Status == 0)
                return 0;
            return operation.Status ?? 0;
        }

        public void UpdateOperationStatus(int operationId, int statusOperation)
        {
            var operation = ctx.Operations.Where(o => o.Id == operationId).FirstOrDefault();
            operation.Status = statusOperation;
            ctx.SaveChanges();
        }

        public void UpdateOperationDelay(int operationId, int delay)
        {
            var operation = ctx.Operations.Where(o => o.Id == operationId).FirstOrDefault();
            operation.Delay = delay;
            operation.IsDelay = true;
            ctx.SaveChanges();
        }
        public List<ServiceStation> GetRooms(int organizationId)
        {
            List<ServiceStation> serviceStationList;
            //serviceStationList = ctx.ServiceStation.Where(f => f.OrganizationId == OrganizationId).OrderBy(f => f.TimeUpdated).ToList();
            serviceStationList = ctx.ServiceStation.ToList();
            return serviceStationList;
        }
        public List<Departments> GetDepartments(int organizationId)
        {
            List<Departments> departmentsList = new List<Departments>();
            var departmentsListQuery = ctx.Departments.Where(dep => dep.organizationId == organizationId&& !dep.IsDeleted);
            departmentsList = departmentsListQuery.ToList();
            return departmentsList;
        }
        public List<CancelationReasons> GetReasons(int organizationId)
        {
            //List<CancelationReasons> cancelationReasonsList = new List<CancelationReasons>();
            //cancelationReasonsList = ctx.CancelationReasons.Where(f => f.OrganizationId == OrganizationId).ToList();
            var cancelationReasonsList = ctx.CancelationReasons.ToList();
            return cancelationReasonsList;
        }
        public OperationsResponse GetOperationsByServiceQId(int serviceQId, int? organizationId)
        {
            List<Appointment> appointmentsList;
            List<Conflict> conflictssList;
            //need to select serviceQueueId by 'organizationId' & 'serviceQId' from ServiceQueues
            var serviceQueueQuery = ctx.ServiceQueues
                .Where(sq => sq.OrganizationId == organizationId && sq.Id == serviceQId);

            var serviceQueue = serviceQueueQuery.FirstOrDefault();
            if (serviceQueue != null)
            {
                appointmentsList = FillServiceQueue(serviceQueue);
                foreach (var item in appointmentsList)
                {
                    item.ServiceQueue = null;
                }

                serviceQueue.Appointments = null;
                return new OperationsResponse()
                {
                    ServiceQueue = serviceQueue,
                    OperationsList = appointmentsList,
                };
            }

            return null;
        }
        public Operation CanceleOperation(int operationId, int reasonId)
        {
            try
            {
                var operation = ctx.Operations.FirstOrDefault(op => op.Id == operationId);
                if (operation == null)
                    return null;

                operation.CancelationReasonId = reasonId;
                operation.Status = Operation.Canceled;
                operation.CanceledDate = DateTime.Now;
                operation.StatusCanceled = true;
                ctx.SaveChanges();


                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Appointment> FillServiceQueue(ServiceQueue serviceQ)
        {
            List<Appointment> appointmentsList;

            var serviceQueueId = serviceQ.Id;
            //need to select all operations by 'serviceQueueId' from Operation 
            var appointmentsListQuery = ctx.Appointments
                .Include(app => app.AppointmentDocs).ThenInclude(rd => rd.RequiredDocument)
                .Include(app => app.AppointmentTasks).ThenInclude(rt => rt.RequiredTask)
                .Include(app => app.AppointmentTests).ThenInclude(rs => rs.RequiredTest)
                .Include(app => app.Fellow)
                .Include(app => app.AllocationOfEquipment)
                    .ThenInclude(aE => aE.Equipment)
                .Include(app => app.ServiceType)
                .Include(app => app.Operation)
                    .ThenInclude(o => o.HostingDepartment)
                .Include(app => app.Operation)
                    .ThenInclude(o => o.NursingUnit)
                .Include(app => app.Operation)
                    .ThenInclude(o => o.EquipmentAppointmentRequest)
                .Include(app => app.Operation)
                    .ThenInclude(o => o.SurgicalDepartment)
                .Include(app => app.Operation)
                    .ThenInclude(o => o.Anesthesiologist)
                .Include(app => app.Operation)
                    .ThenInclude(o => o.Surgeon)
                .Include(app => app.Operation)
                    .ThenInclude(o => o.Nurse)
                .Include(app => app.Operation)
                    .ThenInclude(o => o.TeamReady)
                .Where(app => app.ServiceQueueId == serviceQueueId && app.IsDeleted == false);

            appointmentsList = appointmentsListQuery.ToList();

            return appointmentsList;
        }

        public Operation GetOperationById(int operationId)
        {
            var operationQuery = ctx.Operations.Where(opp => opp.Id == operationId);
            Operation operation = operationQuery.FirstOrDefault();
            return operation;
        }
        public Operation updateReadyEquipmentOperation(int operationId,string stateReady)
        {
           var operationQuery= ctx.Operations.Where(opp => opp.Id == operationId);
            Operation operation = operationQuery.FirstOrDefault();
            operation.IsEqpReady = stateReady;
            ctx.SaveChanges();
            return operation;
        }


        public TeamReady AddTeamReady(TeamReady teamReady, Operation operation)
        {
            ctx.Add(teamReady);
            operation.TeamReady = teamReady;
            operation.TeamReadyId = teamReady.Id;
            return teamReady;
        }

        public TeamReady UpdateTeamReady(TeamReady teamReady)
        {
            try
            {
                var teamReadyQuery = ctx.TeamReady.Where(teamR => teamR.Id == teamReady.Id);
                TeamReady team = teamReadyQuery.FirstOrDefault();
                team.Surgeon = teamReady.Surgeon;
                team.Anesthetic = teamReady.Anesthetic;
                team.Nurse = teamReady.Nurse;
                team.Clean = teamReady.Clean;
                return team;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Operation GetOperationByTeamReadtId(int id)
        {
            return ctx.Operations.Where(opp => opp.TeamReadyId == id).FirstOrDefault();
        }

        public Appointment DeleteOperation(int id)
        {
            var appointmentQuery = ctx.Appointments.Where(app => app.Id == id).Include(app => app.Operation);
            var appointment = appointmentQuery.FirstOrDefault();
            appointment.IsDeleted = true;
            appointment.Operation.IsDeleted = true;
            ctx.SaveChanges();
            return appointment;
        }
    }
}

