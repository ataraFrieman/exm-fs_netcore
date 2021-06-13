using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    [Table("ServiceTypes")]
    public class ServiceType : EntityBase, IComparable { 
        public string Code { get; set; }

        public string Description { get; set; }
        public int? ParentServiceId { get; set; }
        public int? OrganizationId { get; set; }
        public int? Cost { get; set; }
        public List<RequiredTask> RequiredTasks { get; set; }
        public List<RequiredDocument> RequiredDocuments { get; set; }
        public List<RequiredTest> RequiredTests { get; set; }
        public List<ServiceProvidersServiceType> ServiceProvidersServiceTypes { get; set; }

        //[NotMapped]
        //public virtual List<ServiceTypeEqp> Equipments { get; set; }
        public virtual List<MinimalKitRules> MinimalKitRules { get; set; }
        public bool? IsVisibleToApp { get; set; }
        public bool? IsVisibleToOrganization { get; set; }

        public int CompareTo(object obj)
        {
            var serviceTypeToCompare = obj as ServiceType;
            if (serviceTypeToCompare == null)
                return 0;
            return this.Description.CompareTo(serviceTypeToCompare.Description);

        }

        
        protected bool Equals(ServiceType other)
        {
            return !IsNew ? Id == other.Id :
            string.Equals(Description, other.Description);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ServiceType) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                if (!IsNew)
                    return (hashCode * 397) ^ Id;

                return (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
            }
        }
        

     
    }
}
