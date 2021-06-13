using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quze.BL.Operations
{
    public class StatuseOperationLogic
    {
        private QuzeContext context { get; set; }
        private OperationQueueStore operationStore { get; set; }

        public StatuseOperationLogic()
        {
            
        }

        public StatuseOperationLogic(QuzeContext ctx)
        {
            context = ctx;
            operationStore = new OperationQueueStore(ctx);
        }

        public OperationsResponse UpdateStatuseOperation(OperationsResponse operationQueue, Appointment appointment, char state)
        {
            operationStore.UpdateOperationStatus(appointment.Operation.Id, (int)appointment.Operation.Status);
            Appointment appointmentResult=operationQueue.OperationsList.Where(app => app.Id == appointment.Id).FirstOrDefault();
            appointmentResult.Operation.Status = appointment.Operation.Status;
            return operationQueue;
        }

        public void UpdateDelayOperation(Appointment appointment)
        {
            operationStore.UpdateOperationDelay(appointment.Operation.Id, appointment.Operation.Delay??0);
        }

    }
}
