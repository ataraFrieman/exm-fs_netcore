using System;

namespace Quze.Models.Entities
{
    public class TimeTableVacation: EntityBase
    {
        public int ServiceProviderId { get; set; }
        
        //public DateTime Date { get; set; }

        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
