using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Models.Entities
{
    public class Departments : EntityBase
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int ? DepartmentsTypesId { get; set; }
        public int? organizationId { get; set; }
    }
}
