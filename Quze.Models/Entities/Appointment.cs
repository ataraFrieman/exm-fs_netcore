using Quze.Infrastruture.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Quze.Models.slots;

namespace Quze.Models.Entities
{
    [Table("Appointments")]
    public class Appointment : EntityBase , ISlot
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
        
        [ForeignKey(nameof(ServiceQueueId))]
        public virtual ServiceQueue ServiceQueue { get; set; } = null;
       
        public virtual ServiceType ServiceType { get; set; } = null;
        //Minimal Kit
        public virtual List<AppointmentTask> AppointmentTasks { get; set; }
        public virtual List<AppointmentDocument> AppointmentDocs { get; set; }
        public virtual List<AppointmentTest> AppointmentTests { get; set; }
        
        public virtual List<AllocationOfEquipment> AllocationOfEquipment { get; set; } = null;

        public DateTime? reminderSent { get; set; }
        public DateTime? reminderReceived { get; set; }


        public bool? ArrivalConfirmed { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public int? OperationId { get; set; }
        public Operation Operation { get; set; }

        [NotMapped]
        public int? PositionInQueue { get; set; }
        [NotMapped]
        public int AdvanceQueue { get; set; }
        [NotMapped]
        public int Delay { get; set; }/// <summary>
                                      /// before using delay you need to calculate it in serviceQueueLogice.CalculateDelay
                                      /// </summary>
        [NotMapped]
        public DateTime NextPush { get; set; }/// <summary>
                                              /// before using NextPush you need to calculate it in serviceQueueLogice.CalculateDelay
                                              /// </summary>
        [NotMapped]
        public DateTime ETTS { get; set; }

        public Appointment()
        {

        }

        public Appointment(DateTime beginTime, DateTime? endTime, int? duration = null)
        {
            if (endTime.IsNull() && duration.IsNull()) throw new ArgumentNullException("Slot constructor must get endTime or duration");
            BeginTime = beginTime;
            EndTime = endTime.HasValue ? endTime.Value : beginTime.AddSeconds(duration.Value);
            //Duration = duration.HasValue ? duration.Value : ((int)(endTime - beginTime).Value.TotalSeconds);
        }

       
    }
}
