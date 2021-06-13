using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quze.Models.ML
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }

    public class MinimalKit
    {
        /* for every minimal kit id [key]: time of sending to the fellow and time of responsing [value]
         "[123:[2019-01-10 08:00:00,2019-01-10 08:00:00], 456:[2019-01-10 08:00:00,2019-01-10 08:00:00]]" */
        Dictionary<int, List<DateTime?>> MinimalKitReminders;

        public MinimalKit(List<AppointmentDocument> documents, List<AppointmentTask> tasks)
        {
            MinimalKitReminders = new Dictionary<int, List<DateTime?>>();
            foreach (var doc in documents)
            {
                var reminder = new List<DateTime?>();
                reminder.Add(doc.LastReminderSentTime);
                reminder.Add(doc.TimeApproved);
                MinimalKitReminders.Add(doc.RequiredDocumentId, reminder);
            }

            foreach (var task in tasks)
            {
                var reminder = new List<DateTime?>();
                reminder.Add(task.LastReminderSentTime);
                reminder.Add(task.TimeApproved);
                MinimalKitReminders.Add(task.RequiredTaskId, reminder);
            }
        }
    }

    public class BoolConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((bool)value) ? 1 : 0);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value.ToString() == "1";
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
    

    public class RequestML
    {
        public class RequestMLCalculateFields
        {
            public RequestMLCalculateFields(RequestML requestML)
            {
                TimeDifferenceBetweenScheduleToActual = RequestMLConverts.TimeDifferenceBetweenScheduleToActual(requestML.ScheduledDate ?? DateTime.Now, requestML.AppointmentTime ?? DateTime.Now);
                FellowAgeByDays = RequestMLConverts.AgeByDays(requestML.FellowBirthDateAndTime);
                FellowAgeByMonths = RequestMLConverts.AgeByMonths(requestML.FellowBirthDateAndTime);
                FellowAgeByYears = RequestMLConverts.AgeByYears(requestML.FellowBirthDateAndTime);
                ServiceProviderAgeByMonths = 0;
                ServiceProviderAgeByDays = 0;
                ServiceProviderAgeByYears = 0;
                //ServiceProviderAgeByYears = RequestMLConverts.AgeByYears(requestML.ServiceProviderBirthDay);
                //ServiceProviderAgeByMonths = RequestMLConverts.AgeByMonths(requestML.ServiceProviderBirthDay);
                //ServiceProviderAgeByDays = RequestMLConverts.AgeByDays(requestML.ServiceProviderBirthDay);

            }
            public RequestMLCalculateFields()
            {
               
            }
            public int TimeDifferenceBetweenScheduleToActual { get; set; }
            public int FellowAgeByDays { get; set; }
            public int FellowAgeByMonths { get; set; }
            public int FellowAgeByYears { get; set; }
            public int ServiceProviderAgeByYears { get; set; }
            public int ServiceProviderAgeByMonths { get; set; }
            public int ServiceProviderAgeByDays { get; set; }
        }

        public static int IdGenerator { get; set; } = 10000;
        public int RequestId { get; set; }
        public RequestML() { RequestId = IdGenerator++; }
        public RequestMLCalculateFields CalculateFields { get; set; }
        //Fellow details
        [Required(ErrorMessage ="Missing FellowId",ErrorMessageResourceName ="124")]
        public int? FellowId { get; set; }
        public int? FellowIdentity { get; set; }
        public string FellowIdentiyType { get; set; }
        public string FellowGender { get; set; }
        public string FellowMainLang { get; set; }
        public string FellowSecondarylLang { get; set; }
        public string FellowLocation { get; set; } //format: "52.509669, 13.376294"
        public string FellowPhoneNumber { get; set; }
        public string FellowOS { get; set; }
        public string FellowMobileModel { get; set; }
        public string FellowMobility { get; set; }
        public string FellowAccompany { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime FellowBirthDateAndTime { get; set; }
        public string FellowBirthDate { get; set; }
        public int? VisitNumber { get; set; } = 0;

        //Organization details
        public int? OrganizationId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ServiceTypeId { get; set; }

        //Service provider details
        public int? ServiceProviderId { get; set; }
        public string ServiceProviderGender { get; set; }
        public string ServiceProviderMainLang { get; set; }
        public string ServiceProviderSecondarylLang { get; set; }
        public DateTime ServiceProviderBirthDay { get; set; }


        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime? AppointmentTime { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime? ScheduledDate { get; set; }
        public string AppointmentLocation { get; set; } //format: "52.509669, 13.376294"
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime? FirstCase { get; set; }

        public MinimalKit MinimalKit { get; set; }
    }

    public class RequestMLConverts
    {
        public static RequestML ConvetToRequestML(Appointment apointment)
        {
            ServiceProvider serviceProvider = apointment.Operation.Surgeon;
            var result = new RequestML();
            result.ServiceTypeId = apointment.ServiceTypeId;
            result.AppointmentTime = apointment.BeginTime == null ? DateTime.Now : apointment.BeginTime;
            result.ScheduledDate = apointment.TimeCreated == null ? DateTime.Now : apointment.TimeCreated;
            result.FellowBirthDate = apointment.Fellow.BirthDate.ToString();
            result.FellowBirthDateAndTime = apointment.Fellow.BirthDate ?? DateTime.Now;
            result.FellowGender = apointment.Fellow.Gender;
            result.FellowId = apointment.Fellow.Id;
            result.ServiceProviderBirthDay = serviceProvider.BirthDate??DateTime.Now;
            result.ServiceProviderGender = serviceProvider.Gender;
            result.ServiceProviderId = serviceProvider.Id;
            result.CalculateFields = new RequestML.RequestMLCalculateFields(result);
            return result;
        }

        public static int TimeDifferenceBetweenScheduleToActual(DateTime scheduleDate, DateTime actualDate) {
            return (int)(actualDate - scheduleDate).TotalDays;
        }

        public static int AgeByDays(DateTime birthDate= default(DateTime))
        {
            return (int)(DateTime.Today - birthDate).TotalDays;
        }

        public static int AgeByMonths(DateTime birthDate = default(DateTime))
        {
            int months = ((DateTime.Today.Year - birthDate.Year) * 12) + DateTime.Today.Month - birthDate.Month;
            return months;
        }

        public static int AgeByYears(DateTime birthDate = default(DateTime))
        {
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan span = DateTime.Today - birthDate;
            int years = (zeroTime + span).Year - 1;
            return years;
        }
    }



}
