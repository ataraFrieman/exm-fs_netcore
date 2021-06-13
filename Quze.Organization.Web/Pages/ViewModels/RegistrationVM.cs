// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quze.Organization.Web.ViewModels
{
    public class RegistrationVM
        {
            public string IdentityNumber { get; set; }
            public string CountryCode { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
            public string OrganizationName { get; set; }
        }
}
