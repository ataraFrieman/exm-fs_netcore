using System;
using System.Collections.Generic;
using Quze.Models.Models.ViewModels;

namespace Quze.API.ViewModels
{
    public class ServiceProviderVM : BaseVM
    {
        public int OrganizationId { get; set; }

        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public int? CityId { get; set; }
        public int? StreetId { get; set; }
        public DateTime LicenseReceiptDate { get; set; }
        public string OrganizationName { get; set; }

        public DateTime NeareastAppointment { get; set; }

        public List<ServiceTypeVM> ServiceTypes { get; set; }


    }
}
