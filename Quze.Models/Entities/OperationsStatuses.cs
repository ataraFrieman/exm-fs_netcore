using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    [Table("OperationsStatuses")]
    public class OperationsStatuses
    {
        public int  Id { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
    }
}
