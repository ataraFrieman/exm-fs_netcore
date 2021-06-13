using Quze.Models.Entities;
using System.Collections.Generic;
using Quze.Models.Models;

namespace Quze.Models
{
    public class SP_BranchSlots
    {
        public SP_BranchSlots(TimeTable tt)
        {

            BranchId = tt.Branch.Id;
            BranchName = tt.Branch.Name;
            ServiceProviderId = tt.ServiceProviderId;
            ServiceProviderName = tt.ServiceProvider.FullName;
            ServiceTypeId = tt.ServiceTypeId;
            OrganizationId = tt.ServiceProvider.OrganizationId;
        }

        public SP_BranchSlots(ServiceQueue sq)
        {

            BranchId = sq.BranchId;
            BranchName = sq.Branch.Name;
            BranchHouseNumber = sq.Branch.HouseNumber;
            BranchLatitude = sq.Branch.Lat;
            BranchLongitude = sq.Branch.Lng;
            ServiceProviderId = sq.ServiceProviderId;
            ServiceProviderName = sq.ServiceProvider.FullName;
          //  ServiceTypeId = tt.ServiceTypeId;
            OrganizationId = sq.OrganizationId;
            OrganizationName = sq.Branch.Organization.Name;
            Street = sq.Branch.Street;

        }

        public int ServiceProviderId { get; set; }
        public string ServiceProviderName { get; set; }
        public ServiceType ServiceType { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchHouseNumber { get; }
        public decimal? BranchLatitude { get; set; }
        public decimal? BranchLongitude { get; set; }
        public int? ServiceQueueId { get; set; }
        public int? OrganizationId { get; set; }
        public Street Street { get; set; }
        public string OrganizationName { get; set; }
        public int? ServiceTypeId { get; set; }
        public List<Slot> Slots { get; set; }
    }
}
