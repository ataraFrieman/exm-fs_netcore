using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Quze.Infrastruture.Utilities.Email
{
    public class SlngEmail : IEmail
    {
        private const string ApiUserName = "efraim@quze.ai";
        private const string PiPassword = "ff3b92aa-b944-4f8a-a3f9-e54e89e36f50";
        private const string MessageName = "Quze";
        private const string RegistrationValidatinName = "RegistrationValidatinCode";
        private const string ValidationMessage = "Your validation code is: {0}";
        private const string FromName = "Quze Alert System";
        private const string FromEmail = "notifications.no-response@quze.ai";

        const string SlngURL = @"https://slng5.com/Api/SendEmailJson.ashx";

        private EmailResponse SendMessage(string to, string subject, string body, string ackId = null)
        {

            subject = System.Security.SecurityElement.Escape(subject);
            body = System.Security.SecurityElement.Escape(body);

            var request = new SlngRequest(to, subject, body)
            {
                UserName = ApiUserName,
                Password = PiPassword,

                MsgName = MessageName,
                FromName = FromName,
                LanguageType = "1",
                MsgBodyType = "1",
                ReplyToEmail = FromEmail,
                FromEmail = FromEmail,
                DeliveryAckUrl = ackId,
            };

            var json = JsonConvert.SerializeObject(request, Formatting.Indented);
            var jsonEncoded = System.Web.HttpUtility.UrlEncode(json, System.Text.Encoding.UTF8);
            var response = PostJsonDataToSLNG(jsonEncoded);
            return response;

        }
        public async Task<EmailResponse> SendMessageAsync(string to, string subject, string body, string ackId = null)
        {
             return await Task.Run(() => SendMessage(to, subject, body, ackId)); 
        }

        //public void SendMessageAsync1(string to, string subject, string body, string ackId = null)
        //{
        //           SendMessage(to, subject, body, ackId);
        //}

        public async Task<EmailResponse> SendRegistrationCodeAsync(string to, string validationCode)
        {
            var message = string.Format(ValidationMessage, validationCode);
            return await SendMessageAsync(to, RegistrationValidatinName, message);
        }

        private EmailResponse PostJsonDataToSLNG(string json)
        {
            //Setup the web request
            string result = string.Empty;
            WebRequest Request = WebRequest.Create(SlngURL);
            Request.Timeout = 30000;
            Request.Method = "POST";
            Request.ContentType = "application/x-www-form-urlencoded";

            //Set the POST data in a buffer
            byte[] xmlEncoding;
            json = json.Replace(" ", "+");
            //Specify the length of the buffer

            xmlEncoding = Encoding.UTF8.GetBytes(json);
            Request.ContentLength = xmlEncoding.Length;
            //Open up a request stream
            Stream RequestStream = Request.GetRequestStream();
            //Write the POST data
            RequestStream.Write(xmlEncoding, 0, xmlEncoding.Length);
            //Close the stream
            RequestStream.Close();
            //Create the Response object
            WebResponse webResponse;
            webResponse = Request.GetResponse();
            //Create the reader for the response
            StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
            //Read the response
            result = sr.ReadToEnd();
            //Close the reader, and response
            sr.Close();
            webResponse.Close();
            string jsonResponse = System.Web.HttpUtility.UrlDecode(result);
            var response = JsonConvert.DeserializeObject<EmailResponse>(jsonResponse);
            return response;
        }
    }


    internal class SlngEmailAddress
    {
        public string Email { get; set; }
        public string AckID { get; set; }
    }

    internal class SlngRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MsgName { get; set; }
        public string MsgBody { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string ReplyToEmail { get; set; }
        public string MsgBodyType { get; set; }
        public string LanguageType { get; set; }
        public string Subject { get; set; }
        //public string MsgScheduleTime { get; set; }
        public string DeliveryAckUrl { get; set; }
        public List<SlngMobile> Emails = new List<SlngMobile>();

        public SlngRequest(string to, string subject, string messageBody)
        {
            AddRecipient(to);
            Subject = subject;
            MsgBody = messageBody;
        }

        public void AddRecipient(string emailAddress)
        {
            Emails.Add(new SlngMobile() { Mobile = emailAddress });
        }
    }

    public class EmailResponse
    {
        public string Status { get; set; }
        public string Description { get; set; }
        public string GeneralGUID { get; set; }
    }


}
