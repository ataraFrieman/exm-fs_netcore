using System;

namespace Quze.Models.Models.Alerts
{
    public enum AlertType
    {
        SMS = 1,
        Email,
        APP,
        IVR
    }


    public interface IAlert
    {
        AlertType AlertType { get; set; }
        string To { get; set; }
        DateTime TimeToSend { get; set; }
        string MessageTitle { get; set; }
        string MessageBody { get; set; }
        DateTime? ActualSentTime { get; set; }

        string Validate();
    }
}
