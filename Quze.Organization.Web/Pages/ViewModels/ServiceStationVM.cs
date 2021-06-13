namespace Quze.Organization.Web.ViewModels
{
    public class ServiceStationVM : BaseVM
    {
        public string Description { get; set; }
        public int? StationsNumber { get; set; }
        public int BranchId { get; set; }
        public int? OrganizaionId { get; set; }
        public string Location { get; set; }
    }
}
