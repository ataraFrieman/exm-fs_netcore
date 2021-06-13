using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
        //public string Description { get; set; }
        public DateTime TimeCreated { get; set; } = DateTime.Now;
        public DateTime TimeUpdated { get; set; }
        public int LastUpdateUserId { get; set; }
        public int CreatedUserId { get; set; }
        public bool IsDeleted { get; set; }

        public override string ToString()
        {
            return GetType().Name + '-' + Id;
        }

        [NotMapped]
        protected bool IsNew => Id == 0;


        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            
            var objAsBase = obj as EntityBase;
            return !IsNew ? Id == objAsBase.Id : base.Equals(obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here

            return (GetType().ToString() + Id).GetHashCode();
        }
    }
}
