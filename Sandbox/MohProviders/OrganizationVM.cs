using Quze.Models.Models.ViewModels;

namespace MohProviders
{
    public class OrganizationVM : BaseVM
    {
        /// <summary>
        /// משרד הבריאות
        /// </summary>
        public int? MohCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OrganizationTypeCode { get; set; }
        public int? CityCode { get; set; }
       
        public string ZipCode { get; set; }
        public string Address { get; set; }
    }
}