using System.Collections.Generic;
using Quze.Models.Entities;

namespace Quze.BL.UserQueue.UserConstraint
{
   public class UserConstraints
    {
        public UserConstraints()
        { }

        public UserConstraints(Organization organization , ServiceType serviceType,
            List<ServiceProvider> serviceProviders, List<Branch> branches, DatesPossible dateConstraint,
            AreaConstraint areaConstraint = null)
        {
            AreaConstraint = areaConstraint;
            DateConstraint = dateConstraint;
            Organization = organization;
            ServiceType = serviceType;
            ServiceProviders = serviceProviders;
            Branches = branches;
        }

        public AreaConstraint AreaConstraint { get; set; }
        public DatesPossible  DateConstraint { get; set; }

        public Organization Organization { get; set; }
        public List<Branch> Branches { get; set; }

        public List<ServiceProvider> ServiceProviders { get; set; }
        public ServiceType ServiceType { get; set; }




    }
}
