// Json.NET library

using System;

namespace Quze.Infrastruture.Utilities
{
    public interface ISMS
    {
        void SendMessage(string to, string messageName, string message, string ackId);
        void SendRegistrationCode( string to,string message);
        void SendAppointmentDetails(string to, string organization, string serviceProvider, DateTime date, string branch, string URL);
    }

   
}
