using System;
using System.ComponentModel.DataAnnotations;
using Quze.Models.Entities;

namespace Quze.Organization.Web.ViewModels
{
    public class UserTaskVM
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public TaskType TaskType { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime? ExecutionDateTime { get; set; }
        public int? AppointmentId { get; set; }
    }
}

