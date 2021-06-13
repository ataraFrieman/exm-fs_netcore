using System;
using System.Collections.Generic;

namespace Quze.Organization.Web.ViewModels
{
    public class ServiceQueueVM : BaseVM
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ServideProviderId { get; set; }
        public int StationId { get; set; }
        public string Description { get; set; }
        public int TimeTableId { get; set; }
        public int BranchId { get; set; }
        public DateTime? ActualBeginTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public int CurrentAppointementId { get; set; }
        public Boolean Passed { get; set; }
        public ServiceProviderVM ServiceProvider { get; set; }
        public int? ServiceTypeId { get; set; }
        public ServiceTypeOppVM ServiceType { get; set; }
        public BranchVM Branch { get; set; }
        //public AppointmentOppVM CurrentAppointement { get; set; }
        //public List<AppointmentOppVM> Appointments { get; set; }
        public ServiceStationVM ServiceStation { get; set; }
        public int OrganizationId { get; set; }
        



    
    }
}



