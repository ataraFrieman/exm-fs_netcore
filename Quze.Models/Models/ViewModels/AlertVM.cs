using Quze.Models.Models.Alerts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Models.ViewModels
{
    public class AlertVM : BaseVM
    {
        /// <summary>
        ///     Note that one of the fields below must contain a value
        /// </summary>

        [NotMapped]
        public string AlertRulesId { get; set; }


        public AlertType AlertType { get; set; }
        public string To { get; set; }
        public DateTime TimeToSend { get; set; }
        public string MassageTitle { get; set; }
        public string MassageBody { get; set; }
        public DateTime? ActualSentTime { get; set; }


    }
}
