using System;

namespace Quze.Models.slots
{
    public class SlotBase : IComparable<SlotBase>,ISlot
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? Duration { get; set; }

        protected SlotBase(DateTime beginTime, DateTime endTime)
        {
            BeginTime = beginTime;
            EndTime = endTime;
            Duration = (endTime - beginTime).Minutes;
        }

        public int CompareTo(SlotBase other)
        {
            return BeginTime.CompareTo(other.BeginTime);
        }
    }
}
