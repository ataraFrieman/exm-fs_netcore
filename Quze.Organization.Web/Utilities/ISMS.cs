// Json.NET library

namespace Quze.Organization.Web.Utilites
{
    public interface ISMS
    {
        void SendMessage(string to, string messageName, string message, string ackId);
        void SendRegistrationCode( string to,string message);
    }

   
}
