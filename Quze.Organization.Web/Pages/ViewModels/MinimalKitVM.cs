using Quze.Models;
using System;
using System.Collections.Generic;

namespace Quze.Organization.Web.ViewModels
{

    public class MinimalKitType
    {
        public const string Appointment = "Appointment";
        public const string Task = "Task";
    }

    public class MinimalKitVM : Response<int> //: BaseVM ??
    {
        //public string Type { get { return (this.GetType() == typeof(AppointmentDocVM)) ? MinimalKitType.Appointment : MinimalKitType.Task; } }
        public string mkStauts { get; set; } = null;
        public int operationId { get; set; }
        public int AppointmentId { get; set; }
        public List<AppointmentDocVM> Docs { get; set; } = null;
        public List<AppointmentTaskVM> Tasks { get; set; } = null;
        public List<AppointmentTestVM> Tests { get; set; } = null;
    }

    //public class AppointmentDocVM : BaseVM
    //{
    //    public int AppointmentId { get; set; }
    //    public int RequiredDocumentId { get; set; }
    //    public RequiredDocument RequiredDocument { get; set; }
    //    public DateTime? LastReminderSentTime { get; set; }
    //    public DateTime? TimeApproved { get; set; }
    //    public string ApprovedFrom { get; set; }
    //    public int? DocumentContentId { get; set; }
    //    public bool? Approved { get; set; }
    //    public string FileContent { get; set; }
    //    //public bool IsRequired { get; set; }
    //}


    //public class AppointmentTaskVM : BaseVM
    //{
    //    public int AppointmentId { get; set; }
    //    public int RequiredTaskId { get; set; }
    //    public RequiredTask RequiredTask { get; set; }
    //    public DateTime? LastReminderSentTime { get; internal set; }
    //    public DateTime? TimeApproved { get; set; }
    //    public string ApprovedFrom { get; set; }
    //    public bool? Approved { get; set; }
    //}


    //public class AppointmentTestVM: BaseVM
    //{
    //    public int AppointmentId { get; set; }
    //    public int RequiredTestId { get; set; }
    //    public RequiredTest RequiredTest { get; set; }
    //    public DateTime? LastReminderSentTime { get; internal set; }
    //    public DateTime? TimeApproved { get; set; }
    //    public string ApprovedFrom { get; set; }
    //    public bool? Approved { get; set; }
    //    //public string ValueOfTest { get; set; } = null;
    //    public int? ValueOfTest { get; set; }
    //}

}
