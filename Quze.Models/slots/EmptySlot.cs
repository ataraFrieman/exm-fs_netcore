using System;

namespace Quze.Models.slots
{
    public class EmptySlot : SlotBase
    {
        public EmptySlot(DateTime beginTime, DateTime endTime): base(beginTime, endTime)
        {
        }
    }
}
