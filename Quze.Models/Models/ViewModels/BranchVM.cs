namespace Quze.Models.Models.ViewModels
{
    public class BranchVM : BaseVM
    {
        public string Name { get; set; }
        public int OrganizationId { get; set; }
        public int? StreetId { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        public string phonNumber { get; set; }
        public string EmailAddress { get; set; }

    }

    
}
