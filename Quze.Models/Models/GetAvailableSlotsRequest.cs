using System;
using System.Collections.Generic;
using Quze.Models.Entities;

namespace Quze.Models
{
    public class GetAvailableSlotsRequest
    {
        public int? OrganizationId { get; set; }
        public List<Branch> OrganiztionBranches { get; set; }
        public int? ServiceTypeId { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? BranchId { get; set; }
        public int? FellowId { get; set; }
        public Fellow Fellow { get; set; }
        public int? Duration { get; set; }
        private int? arrivalTime;
        public int ArrivalTime { get => arrivalTime != null ? arrivalTime.Value : 0; set => arrivalTime = value; }

        public DateTime BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        
        public int? CityId { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        public ServiceQueue ServiceQueue { get; set; }

    }
}
