using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    [Table("Organizations")]
    public class Organization : EntityBase
    {
        /// <summary>
        /// משרד הבריאות
        /// </summary>
        public string MohCode { get; set; }
        public String Name { get; set; }
        public string IconOrganization { get; set; }
        public string Description { get; set; }
        public int? OrganizationTypeCode { get; set; }
        public int? CityCode { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public virtual List<Branch> Branches { get; set; }
        public virtual List<ServiceProvider> ServiceProviders { get; set; }
        public virtual List<ServiceType> ServiceTypes { get; set; }
        public virtual List<Fellow> Fellows { get; set; }
        public virtual List<ServiceQueue> ServiceQueues { get; set; }

        public override string ToString()
        {
            var text = base.ToString();
            text += " " + Name;
            return text;
        }
    }
}
