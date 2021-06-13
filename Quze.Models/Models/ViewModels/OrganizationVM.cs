using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Models.ViewModels
{
    [Table("Organizations")]
    public class OrganizationVM : BaseVM
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
      

        public override string ToString()
        {
            var text = base.ToString();
            text += " " + Name;
            return text;
        }
    }
}
