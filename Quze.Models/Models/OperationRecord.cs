using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Models.Models
{
    public class OperationRecord
    {
        public OperationRecord()
        {

        }
        public string Code { get; set; }
        public int LocationCode { get; set; }
        public string OperationTypeCode { get; set; }
        public int SurgeonCode { get; set; }
        public int AnesthesiologistCode { get; set; }
        public int CleanTeamCode { get; set; }
        public int? SurgicalNursCode { get; set; }
        public string SurgeonId { get; set; } = null;
        public string AnesthesiologistId { get; set; } = null;
        public string SurgicalNursId { get; set; } = null;
        public DateTime BeginTime { get; set; } = DateTime.UtcNow;
        public int Priority { get; set; }
        public string FellowCode { get; set; }
        public string FellowName { get; set; }
        public int FellowAge { get; set; }
        public string FellowGender { get; set; }
        public int? FellowWeight { get; set; }
        public int? FellowHeight { get; set; }
        public bool IsFellowDiabetic { get; set; }

        //minutes
        public int OperationDuration { get; set; }
        public int? Delay { get; set; }
        public bool? AsDelay { get; set; }
        public bool IsHBP { get; set; }
        public int? SortOrder { get; set; }
        public bool? IsXrayDeviceRequired { get; set; }
        public bool? IsTraining { get; set; }
        public int HostingDepartmentCode { get; set; }
        public int SurgicalDepartmentCode { get; set; }
        public string HostingDepartmentId { get; set; } = null;
        public string SurgicalDepartmentId { get; set; } = null;
        public int? NursingUnitDepartmentCode { get; set; }
        public string NursingUnitDepartmentId { get; set; } = null;
    }
}
