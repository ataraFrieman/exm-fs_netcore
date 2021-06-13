using Quze.Organization.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.Pages.ViewModels
{
    public class ResponseReadyEqpOperationVM
    {
        public AppointmentOppVM Appointment { get; set; } = null;
        public List<int> UnableEquipments { get; set; }
    }
}
