using System;
using System.Collections.Generic;
using Quze.Infrastruture.Extensions;

namespace Quze.Models.Models
{
    public class Slot
    {
        public int? SqId;

        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        private int duration;

        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        //public string BranchName { get; set; }
        //public int BranchId { get; set; }
        //public string ServiceProviderNmae { get; set; }
        //public int ServiceProviderId { get; set; }

        //public virtual ServiceQueue ServiceQueue { get; set; } = null;

        public Slot(int? sqId, DateTime beginTime, DateTime? endTime, int? duration = null)
        {
            if (endTime.IsNull() && duration.IsNull()) throw new ArgumentNullException("Slot constructor must get endTime or duration");
            SqId = sqId;
            BeginTime = beginTime;
            EndTime = endTime.HasValue ? endTime.Value : beginTime.AddSeconds(duration.Value);
            Duration = duration.HasValue ? duration.Value : ((int)(endTime - beginTime).Value.TotalSeconds);
        }

        public Slot(int? sqId, DateTime beginTime, DateTime endTime, int duration)
        {
            SqId = sqId;
            BeginTime = beginTime;
            EndTime = endTime;
            this.duration = duration;
        }

        //public Slot(DateTime beginTime, DateTime? endTime, int? duration, string serviceProviderName, int serviceProviderId, string branchName, int branchId) : this(beginTime, endTime, duration)
        //{
        //    ServiceProviderNmae = serviceProviderName;
        //    ServiceProviderId = serviceProviderId;
        //    BranchName = branchName;
        //    BranchId = branchId;
        //}

        /// <summary>
        /// Devides a slot to slots of size according to the duration parameter
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public IEnumerable<Slot> DevideSlotByDuration(int duration)
        {
            if (Duration < duration) return null;
            
            var slots = new List<Slot>();
            var numberOfSlots = Duration / duration;
            for (var i = 0; i < numberOfSlots; i++)
            {
                var slot = new Slot(SqId, BeginTime.AddSeconds(duration * i), null, duration);
                slots.Add(slot);
            }
            return slots;
        }


        public override string ToString()
        {
            var str = string.Format("between {0} and {1} ({2} minutes)", BeginTime.ToString(@"hh\:mm"), EndTime.ToString(@"hh\:mm"), Duration / 60);
            return str;
        }
    }
}
