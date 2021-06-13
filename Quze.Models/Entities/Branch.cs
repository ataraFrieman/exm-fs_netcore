using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    public class Branch : EntityBase
    {
        public string Name { get; set; }
        public int OrganizationId { get; set; }
        public int? StreetId { get; set; }
        public virtual Street Street { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        public List<ServiceProvidersBranches> ServiceProvidersBranches { get; set; }
        public string PhonNumber { get; set; }
        public string EmailAddress { get; set; }
        public Organization Organization { get; set; }
        [NotMapped]
        public string Address
        {
            get
            {
                //return Street != null ? Street.Name + " " + HouseNumber + " " + Street.City.Name != null ? Street.City.Name: "" : "";
                return Street != null ? Street.Name + " " + HouseNumber : "";
            }
        }
    }
}
