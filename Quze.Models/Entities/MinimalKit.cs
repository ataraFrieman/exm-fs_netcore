using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Models.Entities
{
    public class MinimalKit : Response<int>
    {
        public string mkStauts { get; set; } = null;
        public int operationId { get; set; }
        public int AppointmentId { get; set; }
        public List<AppointmentDocument> Docs { get; set; } = null;
        public List<AppointmentTask> Tasks { get; set; } = null;
        public List<AppointmentTest> Tests { get; set; } = null;
    }
}
