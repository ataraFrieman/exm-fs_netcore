using Quze.Models.Entities;
using Quze.Organization.Web.Pages.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.ViewModels
{
    public class AppointmentOppVM: BaseVM
    {
        public int ServiceQueueId { get; set; }
        public int FellowId { get; set; }
        public Fellow Fellow { get; set; }
        public int? ServiceTypeId { get; set; }

        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? Duration { get; set; }

        public DateTime? ActualBeginTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public int? ActualDuration { get; set; }

        public Boolean Served { get; set; } = false;
        public int? NoShow { get; set; }

        public virtual ServiceQueueVM ServiceQueue { get; set; } = null;

        public virtual ServiceTypeOppVM ServiceType { get; set; } = null;
        //Minimal Kit
        //public virtual List<AppointmentTask> AppointmentTasks { get; set; }
        //public virtual List<AppointmentDocument> AppointmentDocs { get; set; }
        //public virtual List<AppointmentTest> AppointmentTests { get; set; }

        public virtual List<AllocationOfEquipmentVM> AllocationOfEquipment { get; set; } = null;

        public DateTime? reminderSent { get; set; }
        public DateTime? reminderReceived { get; set; }


        public bool? ArrivalConfirmed { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public int? OperationId { get; set; }
        public OperationVM Operation { get; set; }

        public int? PositionInQueue { get; set; }
        public int AdvanceQueue { get; set; }
        public int Delay { get; set; }
        public DateTime ETTS { get; set; }
    }
}
