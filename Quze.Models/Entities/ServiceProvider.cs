using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    [Table("ServiceProviders")]
    public class ServiceProvider : Person
    {
        public object serviceProviders;
        public int? id { get; set; }
        public int OrganizationId { get; set; }
        /// <summary>
        /// מייצג את סוג הרופא למשל מנתח,מרדים וכו
        /// יתכן שבהמשך זה יהיה שדה חובה nullable כרגע מוגדר כ
        /// </summary>
        public int? ServiceProviderType { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string FullName { get => FirstName+" "+LastName; }

        public int? CityId { get; set; }
        public int? StreetId { get; set; }
        public string LicenseNumber { get; set; } = null;
        public DateTime? LicenseReceiptDate { get; set; }
        /// <summary>
        /// מייצג מומחיות אצל רופא או תפקיד וכו'
        /// </summary>
        public string Role { get; set; } = null;
        /// <summary>
        /// Returns the Full name with the leadin title
        /// </summary>
        //public string TitledFullName { get => Person.IsNull() ? string.Empty :Person.Title+ " " + Person.FullName; }
        public Organization Organization { get; set; }
        public List<ServiceProvidersServiceType> ServiceProvidersServiceTypes { get; set; }
        public List<TimeTable> TimeTables { get; set; }
        public virtual List<ServiceProvidersBranches> ServiceProvidersBranches { get; set; }
    }


}
