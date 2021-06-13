using Quze.Models.Models.ViewModels;

namespace Quze.API.ViewModels
{
    public class PersonVM : BaseVM
    {
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }
}