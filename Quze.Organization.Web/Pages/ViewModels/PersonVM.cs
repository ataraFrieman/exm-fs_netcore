using System;

namespace Quze.Organization.Web.ViewModels
{
    public class PersonVM : BaseVM
    {
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string TitledFullName { get; set; }
        public string Gender = "";
        public DateTime? BirthDate { get; set; }
    }
}
