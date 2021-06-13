using Quze.Models.Entities;
using System;
using System.Collections.Generic;

namespace Quze.Models.Models
{

    public class OperationQueue
    {
        public List<Quze.Models.Entities.Operation> OperationList;
        public List<Conflict> ConflictList;
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Department { get; set; }
    }


}
