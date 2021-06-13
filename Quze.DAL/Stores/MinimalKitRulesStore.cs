using System;
using System.Linq;
using Quze.Models.Entities;

namespace Quze.DAL.Stores
{
    public class MinimalKitRulesStore : StoreBase<MinimalKitRules>
    {
        public QuzeContext context { get; set; }
        public MinimalKitRulesStore(QuzeContext ctx):base(ctx)
        {
            context = ctx;
        }

        public AppointmentDocument AddDocumentToAppointment(RequiredDocument requiredDocument, int appointmentId)
        {
            context.RequiredDocuments.Add(requiredDocument);
            AppointmentDocument appointmentDocument = new AppointmentDocument { AppointmentId = appointmentId, RequiredDocumentId = requiredDocument.Id };
            context.AppointmentDocuments.Add(appointmentDocument);
            context.SaveChanges();
            return appointmentDocument;
        }

        public AppointmentTask AddTaskToAppointment(RequiredTask requiredTask, int appointmentId)
        {
            context.RequiredTasks.Add(requiredTask);
            AppointmentTask appointmentTask = new AppointmentTask { AppointmentId = appointmentId, RequiredTaskId = requiredTask.Id };
            context.AppointmentTasks.Add(appointmentTask);
            context.SaveChanges();
            return appointmentTask;
        }

        public void UpdateMkStatus(int operationId, string status)
        {
            var operationQuery = ctx.Operations.Where(opp => opp.Id == operationId);
            Operation operation = operationQuery.FirstOrDefault();
            operation.IsMkReady = status;
            ctx.SaveChanges();
        }
    }
}
