using Quze.Infrastruture.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Models.ViewModels
{
    [NotMapped]
    public class PersonVM : BaseVM
    {
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }

        //TODO: Get rid of 2 fullname props, 1 that includes title should be enough
        public string FullName
        {
            get => FirstName + (LastName.IsNull() ? string.Empty : " " + LastName);
        }

        public string TitledFullName
        {
            get => (Title.IsNull() ? string.Empty : Title + " ") + FirstName + " " + LastName;
        }
    }
}
