namespace MohOrganizations
{
    public class OrganizationVM
    {

        public int? Id { get; set; }
        /// <summary>
        /// משרד הבריאות
        /// </summary>
        public string MohCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OrganizationTypeCode { get; set; }
        public int? CityCode { get; set; }

        public string ZipCode { get; set; }
        public string Address { get; set; }
    }
}