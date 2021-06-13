using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Models.ViewModels
{
    [Table("ServiceProviders")]
    public class ServiceProviderVM : PersonVM
    {
        /// <summary>
        /// מייצג את סוג הרופא למשל מנתח,מרדים וכו
        /// יתכן שבהמשך זה יהיה שדה חובה nullable כרגע מוגדר כ
        /// </summary>
        public int? ServiceProviderType { get; set; }
        public int OrganizationId { get; set; }
        public int? CityId { get; set; }
        public int? StreetId { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime LicenseReceiptDate { get; set; }
        /// <summary>
        /// מייצג מומחיות אצל רופא או תפקיד וכו'
        /// </summary>
        public string Role { get; set; }
     
        public OrganizationVM Organization { get; set; }
        public List<ServiceProvidersServiceTypeSPChildVM> ServiceProvidersServiceTypes { get; set; }
        public List<TimeTableSPChildVM> TimeTables { get; set; }
        public virtual List<ServiceProvidersBranchesSPChildVM> ServiceProvidersBranches { get; set; }
    }


}
