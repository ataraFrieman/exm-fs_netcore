using System;

namespace Quze.Organization.Web.ViewModels
{
    public class AppointmentPushVM : BaseVM
    {
        public DateTime NextPush { get; set; }
        public int? PositionInQueue { get; set; }
        public DateTime CurrentTime { get; set; }
        public int FellowId { get; set; }
        public FellowVM Fellow { get; set; }
        public int Delay { get; set; }
        public DateTime ETTS { get; set; }
        public DateTime? ActualBeginTime { get; set; }

    }
}
