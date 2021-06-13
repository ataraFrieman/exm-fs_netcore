// Json.NET library

using System.Threading.Tasks;

namespace Quze.Infrastruture.Utilities.Email
{
    public interface IEmail
    {
     //  void SendMessageAsync1(string to, string subject, string body, string ackId = null);
        Task<EmailResponse> SendMessageAsync(string to, string subject, string body, string ackId = null);
        Task<EmailResponse> SendRegistrationCodeAsync( string to,string code);
    }


}
