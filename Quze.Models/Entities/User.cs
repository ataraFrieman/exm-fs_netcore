using System.Collections.Generic;

namespace Quze.Models.Entities
{
    public class User : Person
    {
        public const int SystemUser = -999;
        public const int CreateAlertsServiceUser = -1000;
        public const int SendAlertsServiceUser = -2000;

        public const string QuzeApiUserName = "QuzeApiUserName";

        public string UserName { get; set; }
        public int? OrganizationId { get; set; }

        public int UserTypeId { get; set; }
        public virtual UserType UserType { get; set; }

        //[ForeignKey("IdentityNumber")]
        public string DeviceId { get; set; }

        public string Language { get; set; }
        public string Platform { get; set; }
        public string DeviceName { get; set; }
        public string BrowserName { get; set; }
        public string OsName { get; set; }
        public string OsVersion { get; set; }

        public List<Fellow> Fellows { get; set; }

    }
}
