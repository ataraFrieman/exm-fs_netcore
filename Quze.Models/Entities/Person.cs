using Quze.Infrastruture.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    [NotMapped]
    public class Person : EntityBase
    {
        public string IdentityNumber { get; set; } = null;
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        public string Title { get; set; } = null;
        public string Email { get; set; } = null;
        public string Gender { get; set; } = null;
        public DateTime? BirthDate { get; set; }

        //TODO: Get rid of 2 fullname props, 1 that includes title should be enough
        [NotMapped]
        public string FullName
        {
            get => FirstName + (LastName.IsNull() ? string.Empty : " " + LastName) ;
        }

        [NotMapped]
        public string TitledFullName { get => (Title.IsNull() ? string.Empty : Title + " ") + FirstName + " " + LastName; }
    }
}
