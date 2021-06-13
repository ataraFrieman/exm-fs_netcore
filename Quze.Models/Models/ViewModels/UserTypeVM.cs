namespace Quze.Models.Models.ViewModels
{
    public class UserTypeVM : BaseVM
    {
        public string Description { get; set; }

        public const int ApplicationUser = 1;
        public const int OrganizationUser = 2;
        public const int SuperUser = 3;
    }
}
