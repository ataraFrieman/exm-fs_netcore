
using Quze.Models.Models.ViewModels;

namespace Quze.Organization.Web.ViewModels
{
    public class BranchVM : BaseVM
    {
        public string Name { get; set; }
        public int OrganizationId { get; set; }
        public int? StreetId { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public virtual StreetVM Street { get; set; }
    }
}
