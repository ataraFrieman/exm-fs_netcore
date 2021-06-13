using Quze.Infrastruture.Extensions;
using Quze.Models.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Quze.Models.Models;

namespace Quze.Models.Entities
{
    public sealed class ServiceQueue : EntityBase , IComparable<ServiceQueue>
    {
        public int TimeTableId { get; set; }  
        public int ServiceProviderId { get; set; }
        public int? OrganizationId { get; set; }
        public int BranchId { get; set; }
        public int? StationId { get; set; }
       


        public DateTime EndTime { get; set; }
        public DateTime BeginTime { get; set; }


        public DateTime? ActualBeginTime { get; set; }
        public DateTime? ActualEndTime { get; set; }


        public List<Appointment> Appointments { get; set; }

        public bool Passed { get; set; }

        public ServiceProvider ServiceProvider { get; set; }

        public int? ServiceTypeId { get; set; }

        public ServiceType ServiceType { get; set; }

        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }

        [ForeignKey("StationId")]
        public ServiceStation ServiceStation { get; set; }

        public int? CurrentAppointementId { get; set; }
        public Appointment CurrentAppointement { get; set; }

        [NotMapped]
        public List<Appointment> SortedApoointments
        {
            get
            {
                if (Appointments.IsNullOrEmpty()) return null;
                return Appointments.OrderBy(a => a.BeginTime).ToList();
            }
        }

        [NotMapped]
        public string Description
        {
            get
            {
                string description = "";
                if (ServiceType != null)
                    description = ServiceType.Description + " " + '\r';
                if (ServiceProvider != null)
                    description = ServiceProvider.FullName + " " + '\r';
                if (Branch != null)
                    description = description + Branch.Name + " " + '\r';
                if (ServiceStation != null)
                    description = description + ServiceStation.Description;
                return description;
            }

        }

        public ServiceQueue()
        {

        }

        /// <summary>
        /// gets ttException and creates a new service queue
        /// </summary>
        /// <param name="ttException"></param>
        public ServiceQueue(TimeTableException ttException)
        {
            TimeTableId = ttException.TimeTableId;
            // the date with time of 00:00:00
            var date = ttException.DateTime.Date;

            BeginTime = new DateTime(date.Ticks).Add(new TimeSpan(BeginTime.Hour, BeginTime.Minute, BeginTime.Second));

            EndTime = new DateTime(date.Ticks).Add(new TimeSpan(EndTime.Hour, EndTime.Minute, EndTime.Second));

            BranchId = ttException.TimeTable.BranchId;
            ServiceProviderId = ttException.TimeTable.ServiceProviderId;
            OrganizationId = ttException.TimeTable.Branch.OrganizationId;
        }

        public ServiceQueue(TimeTableLine timeTableLine, DateTime date)
        {
            TimeTableId = timeTableLine.TimeTableId;
            BeginTime = date.ChangeTime(timeTableLine.BeginTime);
            EndTime = date.ChangeTime(timeTableLine.EndTime);
            BranchId = timeTableLine.TimeTable.BranchId;
            ServiceProviderId = timeTableLine.TimeTable.ServiceProviderId;
            var a = timeTableLine.TimeTable.BranchId;
            OrganizationId = timeTableLine.TimeTable.Branch.OrganizationId;
           ServiceTypeId = timeTableLine.TimeTable.ServiceTypeId;
        }

        //public ServiceQueue(DateTime beginTime, DateTime endTime)
        //{
        //    BeginTime = beginTime;
        //    EndTime = endTime;
        //}

        public ServiceQueue(ServiceQueue sq, DateTime beginTime, DateTime endTime)
        {
            BeginTime = beginTime;
            EndTime = endTime;
            ServiceProviderId = sq.ServiceProviderId;
            OrganizationId = sq.OrganizationId;
            BranchId = sq.BranchId;
            StationId = sq.StationId;
            Appointments = sq.Appointments?.Where(a => a.BeginTime.IsBetween(beginTime, endTime)).ToList();
            Passed = sq.Passed;
            ServiceProvider = sq.ServiceProvider;
            ServiceTypeId = sq.ServiceTypeId;
            ServiceType = sq.ServiceType;
            Branch = sq.Branch;
            ServiceStation = sq.ServiceStation;
            CurrentAppointementId = sq.CurrentAppointementId;
            CurrentAppointement = sq.CurrentAppointement;
        }

        public ServiceQueue(DateTime beginTime, DateTime? endTime, int? duration = null)
        {
            if (endTime.IsNull() && duration.IsNull())
                throw new ArgumentNullException("Slot constructor must get endTime or duration");
            BeginTime = beginTime;
            if (duration != null)
                EndTime = endTime ?? beginTime.AddSeconds(duration.Value);
            //Duration = duration.HasValue ? duration.Value : ((int)(endTime - beginTime).Value.TotalSeconds);
        }

        private bool Equals(ServiceQueue other)
        {
            return !IsNew ? Id == other.Id:
                    ServiceProviderId == other.ServiceProviderId 
                   && BranchId == other.BranchId
                   && EndTime.Equals(other.EndTime) 
                   && BeginTime.Equals(other.BeginTime);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ServiceQueue other && Equals(other);
        }
        

        public override int GetHashCode()
        {
                var hashCode = base.GetHashCode();
                if (!IsNew)
                     return (hashCode * 397) ^ Id;

                hashCode = (hashCode * 397) ^ ServiceProviderId;
                hashCode = (hashCode * 397) ^ BranchId;
                hashCode = (hashCode * 397) ^ EndTime.GetHashCode();
                hashCode = (hashCode * 397) ^ BeginTime.GetHashCode();
                return hashCode;
        }
        
        public int Duration => (int)(EndTime - BeginTime).TotalSeconds;


        public int GetQueDurationBetweenBeginTimeToFirstAppointment()
        {
            var sortedApoointments = SortedApoointments;
            if (sortedApoointments.IsNullOrEmpty()) return 0;
            if (sortedApoointments.IsNotNullOrEmpty())
                return (int)(sortedApoointments[0].BeginTime - BeginTime).TotalSeconds;
            return 0;
        }

        /// <summary>
        /// Gets duration between appointment to the next or to the end of the queue
        /// </summary>
        /// <returns></returns>
        public int GetDurationBetweenAppointmentToTheNextOne(int index)
        {
            var sortedApoointments = SortedApoointments;
            if (sortedApoointments.IsNullOrEmpty()) return 0;
            if (sortedApoointments.Count < index) throw new ArgumentOutOfRangeException();
            if (sortedApoointments.Count == index + 1)
                return (int)(EndTime - sortedApoointments[index].EndTime).TotalSeconds;
            return (int)(sortedApoointments[index + 1].BeginTime - sortedApoointments[index].EndTime).TotalSeconds;
        }


        /// <summary>
        /// Gets slot between appointment to the next or to the end of the queue
        /// </summary>
        /// <returns></returns>
        public Slot GetSlotBetweenAppointmentToTheNextOne(int index)
        {
            var sortedApoointments = SortedApoointments;
            var endTime = sortedApoointments[index].EndTime > Time.GetNow() ? sortedApoointments[index].EndTime : Time.GetNow();
            if (sortedApoointments.IsNullOrEmpty()) return null;
            if (sortedApoointments.Count <= index) return null;
            if (sortedApoointments.Count == index + 1 && this.EndTime > Time.GetNow())
                return new Slot(Id, endTime, this.EndTime);
            if (sortedApoointments[index + 1].BeginTime > Time.GetNow())
                return new Slot(Id, endTime, sortedApoointments[index + 1].BeginTime);
            return null;
        }

        public int CompareTo(ServiceQueue that)
        {
            if (ServiceProviderId < that.ServiceProviderId)
                return -1;
            if (ServiceProviderId > that.ServiceProviderId)
                return 1;
            if (BranchId < that.BranchId)
                return -1;
            if (BranchId > that.BranchId)
                return 1;
            if (BeginTime < that.BeginTime)
                return -1;
            if (BeginTime > that.BeginTime)
                return 1;

            return 0;
            //  return this.Balance.CompareTo(that.Balance);
        }






    }
}
