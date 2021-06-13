using System;

namespace Quze.Models.Models.ViewModels
{
    public class UserTaskVM : BaseVM
    {
        public int UserId { get; set; }
        public TaskType TaskType { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? ExecutionDateTime { get; set; }
        public int? AppointmentId { get; set; }


        public virtual UserVM User { get; set; }
        public virtual AppointmentVM Appointment { get; set; }
    }

    public enum TaskType
    {
        Task = 1, Recommendation
    }
}