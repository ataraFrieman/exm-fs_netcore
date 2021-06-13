using Quze.Infrastruture.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Quze.Models.Entities;

namespace Quze.Models.Models.ViewModels
{
    public class ServiceQueueVM : BaseVM
    {
        public int ServiceProviderId { get; set; }
        public int? OrganizationId { get; set; }
        public int BranchId { get; set; }
        public int? StationId { get; set; }

        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }


        public DateTime? ActualBeginTime { get; set; }
        public DateTime? ActualEndTime { get; set; }


        public virtual List<Appointment> Appointments { get; set; }

        public Boolean Passed { get; set; }

        public ServiceProviderVM ServiceProvider { get; set; }

        public int? ServiceTypeId { get; set; }

        public ServiceTypeVM ServiceType { get; set; }

        public Branch Branch { get; set; }

        public virtual ServiceStation ServiceStation { get; set; }

        public int? CurrentAppointementId { get; set; }
        public virtual Appointment CurrentAppointement { get; set; }


        public List<Appointment> SortedApoointments
        {
            get
            {
                return Appointments.IsNullOrEmpty() ? null : Appointments.OrderBy(a => a.BeginTime).ToList();
            }
        }

        public string Description {
            get
            {
                string description = "";
                if (ServiceType != null)
                    description = ServiceType.Description + " " + '\r';
                if(ServiceProvider != null)
                    description = ServiceProvider.FullName + " " + '\r';
                if (Branch != null)
                    description = description + Branch.Name + " " + '\r';
                if (ServiceStation != null)
                    description = description + ServiceStation.Description;
                return description;
                    }

        }





       


        public int Duration { get => (int)(EndTime - BeginTime).TotalSeconds; }


      

       
    }
}
