using Quze.Models.Models.Alerts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    public class Alert : EntityBase, IAlert
    {
        /// <summary>
        ///     Note that one of the fields below must contain a value
        /// </summary>

        [NotMapped]
        public string AlertRulesId { get; set; }


        public AlertType AlertType { get; set; }
        public string To { get; set; }
        public DateTime TimeToSend { get; set; }
        public string MessageTitle { get; set; }
        public string MessageBody { get; set; }
        public DateTime? ActualSentTime { get; set; }


        public string Validate()
        {

            switch (AlertType)
            {
                case AlertType.SMS:
                    var phoneNumber = new PhoneAttribute();
                    if (!phoneNumber.IsValid(To))
                    {
                        return "Wrong phone number";
                    }
                    break;
                case AlertType.Email:
                    if (!new EmailAddressAttribute().IsValid(To))
                    {
                        return "To is not a valid email";
                    }
                    break;
                case AlertType.APP:
                    break;
                case AlertType.IVR:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return string.Empty;
        }
    }
}
