//using System;
//using System.ComponentModel.DataAnnotations;

//namespace Quze.Models.Models.Alerts
//{
//    public class AlertToSend : IAlert
//    {
//        public AlertType AlertType { get; set; }
//        public string To { get; set; }
//        public DateTime TimeToSend { get; set; }
//        public string MessageTitle { get; set; }
//        public string MessageBody { get; set; }
//        public DateTime? ActualSentTime { get; set; }


//        public string Validate()
//        {

//            switch (AlertType)
//            {
//                case AlertType.SMS:
//                    var phoneNumber = new PhoneAttribute();
//                    if (!phoneNumber.IsValid(To))
//                    {
//                        return "Wrong phone number";
//                    }
//                    break;
//                case AlertType.Email:
//                    if (!new EmailAddressAttribute().IsValid(To))
//                    {
//                        return "To is not a valis email";
//                    };
//                    break;
//                case AlertType.APP:
//                    break;
//                case AlertType.IVR:
//                    break;
//                default:
//                    break;
//            }
//            return string.Empty;
//        }

//        private void PhoneNumber(object countryCode, object p, object phoneNumber)
//        {
//            throw new NotImplementedException();
//        }
//    }

//}
