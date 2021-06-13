using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace Quze.Infrastruture.Utilities.Email
{
    public class GmailMail : IEmail
    {
        const string ValidationMessage = "Your QUZE validation code is: {0}";


        public async Task<EmailResponse> SendMessageAsync(string to, string subject, string body, string ackId = null)
        {
            var response = new EmailResponse();
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(Configurations.SendingEmailFromAddress, Configurations.EmailPassword),
                EnableSsl = true
            };
            try
            {
                await client.SendMailAsync(Configurations.SendingEmailFromAddress, to, subject, body);
            }
            catch (System.Exception ex)
            {

                response.Description = ex.Message;
            }
            return response;
        }

        public async Task<EmailResponse> SendRegistrationCodeAsync(string to, string code)
        {
            var message = string.Format(ValidationMessage, code);
            return await SendMessageAsync(to, "Quze Registration code!", message);
        }
    }
}
