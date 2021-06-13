using Quze.Models.Models.ViewModels;

namespace Quze.API.ViewModels
{
    public class RequiredTasksVM : BaseVM
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public int ServiceTypeID { get; set; }
    }
}