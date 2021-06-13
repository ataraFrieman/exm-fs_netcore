using Quze.Models.Entities;

namespace Quze.Models.Models.ViewModels
{
    public class UserVM : PersonVM
    {
        public const int SystemUser = -999;
        public const int CreateAlertsServiceUser = -1000;
        public const int SendAlertsServiceUser = -2000;

        public const string QuzeApiUserName = "QuzeApiUserName";

        public string UserName { get; set; }
        public int? OrganizationId { get; set; }
        public string DeviceId { get; set; }

        public int UserTypeId { get; set; }
        public virtual UserType UserType { get; set; }
    }
}
