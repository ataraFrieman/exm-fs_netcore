using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    [Table("Experties")]
    public class Experty : EntityBase
    {
        public string Description { get; set; }
        public string MohCode { get; set; }

    }
}
