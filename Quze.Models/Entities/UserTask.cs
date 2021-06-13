using System;

namespace Quze.Models.Entities
{
    public class UserTask : EntityBase
    {
        public int UserId { get; set; }
        public TaskType TaskType { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? ExecutionDateTime { get; set; }
        public int? AppointmentId { get; set; }


        public virtual User User { get; set; }
        public virtual Appointment Appointment { get; set; }
    }

    public enum TaskType
    {
        Task = 1, Recommendation
    }
}