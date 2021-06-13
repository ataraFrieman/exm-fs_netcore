using Newtonsoft.Json;
using Quze.Models.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quze.Models.ML
{
    public class OperationRequestML
    {
        public OperationRequestML()
        {

        }
        //Fellow
        public int? FellowAge { get; set; }
        public int? FellowWeight { get; set; }
        public int? FellowHeight { get; set; }
        public string FellowGender { get; set; }

        //Appointment
        public int AppointmentMonth { get; set; }
        public int AppointmentWeekDay { get; set; }
        //public float AppointmentHour { get; set; }
        public float AppointmentHour { get; set; }
        public bool IsRoomFirstCase { get; set; }
        public bool IsRoomLastCase { get; set; }
        public bool IsSerProvFirstCase { get; set; }
        public bool IsSerProvLastCase { get; set; }
        public int RoomId { get; set; }
        //opration service type code 1
        public string ServiceMainPricedure { get; set; }
        //opration service type code 2
        public string ServiceSecondaryPricedure { get; set; } = null;
        public int ServiceProviderId { get; set; }
        //Anesthesiologist Id
        public int? ServiceProviderAssistantId { get; set; }

        //Department
        public int DepDepartmentId { get; set; }
        public int DepTreatmentDepartmentId { get; set; }
        public int? DepNursingDepartmentId { get; set; }

        //נתוני ניתוח קודם

        //מנתח קודם
        public int PreviousMenateach { get; set; }
        //מרדים קוד
        public int? PreviousMardim { get; set; }
        //יחידה רםואית קוד
        public int PreviousYechidaRefuitID{ get; set; }
        //יחידה סיעודית קוד
        public int PreviousYechidaSiuditID { get; set; }
        //מחלקה מנתחת
        public int PreviousMachlakaMenatachatID { get; set; }
        //אי סי די ניין
        public string PreviousICD9 { get; set; }
        //גיל מטופל קוד
        public int PreviousGilMetupal { get; set; }
        //מין המטופל הקודם
        public string PreviousGender { get; set; }
        //גובה
        public int PreviousGova { get; set; }
        //משקל
        public int PreviousMishkal { get; set; }
        //סכרת
        public bool PreviousDiabetis { get; set; }
        //לחץ דם
        public bool PreviousHypertension { get; set; }


     //"PreviousrnChederYomi",
     //"PreviousHadrachatMardim",
     
        

        public string ConvertOperationRequestMLToArray(OperationRequestML req)
        {
            return JsonConvert.SerializeObject(req, Formatting.Indented);
        }
    }




}
