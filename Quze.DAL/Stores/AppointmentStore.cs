using Microsoft.EntityFrameworkCore;
using Quze.Models.Entities;
using Quze.Models.Models;
using System.Collections.Generic;
using System.Linq;

namespace Quze.DAL.Stores
{
    public class AppointmentStore : StoreBase<Appointment>
    {

        public AppointmentStore(QuzeContext ctx) : base(ctx)
        {

        }
        public Appointment CreateAppointment(OperationRecord operationRecord, int serviceQueueId, int fellowId, Operation operation, int serviceTypeId)
        {
            Appointment operationApp = new Appointment()
            {
                BeginTime = operationRecord.BeginTime,
                EndTime = operation.SurgeryOrigEndTime,
                FellowId = fellowId,
                ServiceTypeId = serviceTypeId,
                OperationId = operation.Id,
                ServiceQueueId = serviceQueueId,
            };
            return operationApp;
        }
        public int AddAppointment(Appointment Appointment)
        {
            ctx.Appointments.Add(Appointment);
            //ctx.SaveChanges();
            return Appointment.Id;
        }
        public Appointment GetAppointmentById(int appointmentId)
        {
            var appointmentQuery = ctx.Appointments.Include(app => app.AppointmentDocs).ThenInclude(rd => rd.RequiredDocument)
                   .Include(app => app.AppointmentTasks).ThenInclude(rt => rt.RequiredTask)
                   .Include(app => app.AppointmentTests).ThenInclude(rs => rs.RequiredTest)
                   .Include(app => app.Fellow)
                   .Include(app => app.AllocationOfEquipment)
                       .ThenInclude(aE => aE.Equipment)
                   .Include(app => app.ServiceType)
                   .Include(app => app.Operation)
                       .ThenInclude(o => o.HostingDepartment)
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
                   .Where(app => app.Id == appointmentId);
            Appointment appointment = appointmentQuery.FirstOrDefault();
            return appointment;
        }

        public Appointment RemoveAppointment(int appointmentId)
        {
            Appointment appointment = GetAppointmentById(appointmentId);
            ctx.Appointments.Remove(appointment);
            return appointment;
        }

        public void UpdateAppointmentsOrigTimes(List<Appointment> appointments)
        {
            foreach (Appointment app in appointments)
            {
               var newAppointment= ctx.Appointments.Include(appDb=> appDb.Operation).Where(appDb=>appDb.Id== app.Id).FirstOrDefault();
                newAppointment.BeginTime = app.BeginTime;
                newAppointment.EndTime = app.EndTime;
                newAppointment.Operation.AnesthesiaOrigBeginTime = app.Operation.AnesthesiaOrigBeginTime;
                newAppointment.Operation.AnesthesiaOrigEndTime = app.Operation.AnesthesiaOrigEndTime;
                newAppointment.Operation.SurgeryOrigBeginTime = app.Operation.SurgeryOrigBeginTime;
                newAppointment.Operation.SurgeryOrigEndTime = app.Operation.SurgeryOrigEndTime;
                //newAppointment.Operation.CleanOrigBeginTime = app.Operation.CleanOrigBeginTime;
                //newAppointment.Operation.CleanOrigEndTime = app.Operation.CleanOrigEndTime;
            }
            //ctx.Appointments.UpdateRange(appointments);

        }
    }
}
