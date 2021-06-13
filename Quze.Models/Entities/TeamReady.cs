using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quze.Models.Entities
{
    [Table("TeamReady")]
    public class TeamReady
    {
        public int Id { get; set; }
        public bool? Surgeon { get; set; }
        public bool? Anesthetic { get; set; }
        public bool? Nurse { get; set; }
        public bool? Clean { get; set; }
    }
}
