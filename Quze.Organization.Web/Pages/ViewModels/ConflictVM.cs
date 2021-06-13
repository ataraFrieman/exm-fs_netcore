using Quze.Organization.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.Pages.ViewModels
{
    public class ConflictVM
    {
        public int AppointmentAId { get; set; }
        public int AppointmentBId { get; set; }
        public AppointmentOppVM AppointmentA { get; set; }
        public AppointmentOppVM AppointmentB { get; set; }
        public string Type { get; set; }
        public DateTime ConflictBeginTime { get; set; }
        public DateTime ConflictEndTime { get; set; }
        public int ServiceQueueId { get; set; }
    }
}
