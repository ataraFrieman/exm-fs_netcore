using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.API.IVR
{
    public class QFlowIVR
    {
        public string PhoneNumber { get; set; }
        public string FellowName { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string ServiceType { get; set; }
        public int AppointmentId { get; set; }
    }
}
