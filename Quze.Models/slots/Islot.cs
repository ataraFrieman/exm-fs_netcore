using System;

namespace Quze.Models.slots
{
   public interface ISlot
   {
        DateTime BeginTime { get; set; }
        DateTime EndTime { get; set; }
        int? Duration { get; set; }
    }
}
