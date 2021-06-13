using System;

namespace MohProviders
{

    public class ServiceProviderVM
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }

        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public DateTime LicenseReceiptDate { get; set; }
        public String LicenseReceiptDateString { get; set; }
        public string OrganizationName { get; set; }

        public DateTime NeareastAppointment { get; set; }

        //public List<ServiceTypeVM> ServiceTypes { get; set; }

        public override string ToString()
        {
            return (FirstName + ","
                 + LastName + ","
                 + LicenseNumber + ","
                 + CityName + ","
                 + LicenseReceiptDateString);
        }

    }
}
