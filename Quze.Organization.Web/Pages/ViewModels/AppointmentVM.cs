using System;

namespace Quze.Organization.Web.ViewModels
{
    public class AppointmentVM : BaseVM
    {
        public int ServiceQueueId { get; set; }
        public int ServiceTypeId { get; set; }
        public int FellowId { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? ActualBeginTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public int? ActualDuration { get; set; }
        public int? Duration { get; set; }
        public bool Served { get; set; }
        public int NoShow { get; set; }
        // Causes cross references if you need it please consult with CM //used in organization controller Get Service Queue()
        //public ServiceQueueVM ServiceQueue { get; set; }  
        public ServiceTypeVM ServiceType { get; set; }
        public ServiceProviderVM ServiceProvider { get; set; }
        public FellowVM Fellow { get; set; }
        public DateTime NextPush { get; set; }
        public int Delay { get; set; }
    }
}
