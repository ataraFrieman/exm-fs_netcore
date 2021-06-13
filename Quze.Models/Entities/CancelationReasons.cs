using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Models.Entities
{
    public class CancelationReasons : EntityBase
    {
        public string Description { get; set; }

        public string Code { get; set; }

     }
}
