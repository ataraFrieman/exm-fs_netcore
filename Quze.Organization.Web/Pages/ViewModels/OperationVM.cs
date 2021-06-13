using Quze.Models.Entities;
using Quze.Organization.Web.Pages.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.ViewModels
{
    public class OperationVM : BaseVM
    {
        public const int Canceled = 7;

        public string Code { get; set; }
        public DateTime AnesthesiaBeginTime { get; set; }
        public DateTime AnesthesiaEndTime { get; set; }

        public DateTime AnesthesiaActualBeginTime { get; set; }
        public DateTime AnesthesiaActualEndTime { get; set; }
        public DateTime CleanBeginTime { get; set; }
        public DateTime CleanEndTime { get; set; }
        public DateTime CleanActualEndTime { get; set; }
        public DateTime CleanActualBeginTime { get; set; }
        public int CleanTeamId { get; set; }
        public DateTime SurgeryBeginTime { get; set; }
        public DateTime SurgeryEndTime { get; set; }
        public DateTime SurgeryActualBeginTime { get; set; }
        public DateTime SurgeryActualEndTime { get; set; }
        public int SurgeonId { get; set; }
        public ServiceProvider Surgeon { get; set; }
        public int RoomId { get; set; }
        public int Priority { get; set; }
        public DateTime SurgeryOrigBeginTime { get; set; }
        public DateTime SurgeryOrigEndTime { get; set; }
        public DateTime CleanOrigEndTime { get; set; }
        public int AnesthesiologistId { get; set; }
        public ServiceProvider Anesthesiologist { get; set; }
        public DateTime CleanOrigBeginTime { get; set; }
        public DateTime AnesthesiaOrigBeginTime { get; set; }
        public DateTime AnesthesiaOrigEndTime { get; set; }
        public int NurseId { get; set; }
        public ServiceProvider Nurse { get; set; }
        public bool? IsHBP { get; set; }
        public int? Status { get; set; }
        public int? SortOrder { get; set; }

        public bool? IsXrayDeviceRequired { get; set; }
        public bool? IsTraining { get; set; }

        public int? OperationDuration { get; set; }
        public int? AnesthesiaDuration { get; set; }
        public int? CleanDuration { get; set; }

        public int HostingDepartmentId { get; set; }

        public Departments HostingDepartment { get; set; }

        public int SurgicalDepartmentId { get; set; }

        public Departments SurgicalDepartment { get; set; }

        public int? NursingUnitId { get; set; }
        public Departments NursingUnit { get; set; }
        public virtual List<EquipmentAppointmentRequestVM> EquipmentAppointmentRequest { get; set; } = null;

        public virtual List<int> NotEnabledEquipmentsAppointmentRequest { get; set; } = null;

        public int? Delay { get; set; }
        public int? Duration { get; set; }
        public int? CancelationReasonId { get; set; }
        // 0 = NOT canceled, 1 = canceled
        public bool? StatusCanceled { get; set; }
        //the time that the operation is canceled
        public DateTime? CanceledDate { get; set; }
        public bool IsDelay
        {
            get { return this.Delay > 0 ? true : false; }
            set { }
        }
        public int? TeamReadyId { get; set; }
        public TeamReady TeamReady { get; set; }
        public string IsTeamReady { get; set; } = null;
        public string IsEqpReady { get; set; }=null;
        public string IsMkReady { get; set; } = null;
    }
}
