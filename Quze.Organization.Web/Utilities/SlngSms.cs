using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Quze.Organization.Web.Utilites
{
    public class SlngSMS : ISMS
    {
        const string ApiUserName = "efraim@quze.ai";
        const string piPassword = "ff3b92aa-b944-4f8a-a3f9-e54e89e36f50";
        const string FromMobileName = "Quze";
        const string RegistrationValidatinName = "RegistrationValidatinCode";
        const string ValidationMessage = "Your validation code is: {0}";

        const string SlngURL = @"https://slng5.com/Api/SendSmsJsonBody.ashx";

        public void SendMessage(string to, string messageName, string message, string ackId = null)
        {
            message = System.Security.SecurityElement.Escape(message);
            //##שם_פרטי ##AND ##שם_משפחה ##AND NOW INENGLISH: ##FIRST_NAME## AND ##LAST_NAME##");
            var request = new SlngRequest(to, messageName, message)
            {
                UserName = ApiUserName,
                Password = piPassword,
                FromMobile=FromMobileName,
                DeliveryAckUrl = ackId
            };

            string json = JsonConvert.SerializeObject(request, Formatting.Indented);
            string jsonEncoded = System.Web.HttpUtility.UrlEncode(json.ToString(), System.Text.Encoding.UTF8);
            SlngResponse response = new SlngResponse();
            response = PostJsonDataToSLNG(jsonEncoded);
            // if (obj2.Status)
            //     return true;
            // else
            //     return false;

        }

        public void SendRegistrationCode(string to, string validationCode)
        {
            var message = string.Format(ValidationMessage, validationCode);
            SendMessage(to,RegistrationValidatinName, message);
        }

        public SlngResponse PostJsonDataToSLNG(string json)
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
            var response = JsonConvert.DeserializeObject<SlngResponse>(jsonResponse);
            return response;
        }
    }


    internal class SlngMobile
    {
        public string Mobile { get; set; }
        public string AckID { get; set; }
    }

    internal class SlngRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MsgName { get; set; }
        public string MsgBody { get; set; }
        public string FromMobile { get; set; }
        public string DeliveryAckUrl { get; set; }
        public List<SlngMobile> Mobiles = new List<SlngMobile>();

        public SlngRequest(string to, string messageName, string messageBody)
        {
            AddRecipient(to);
            MsgName = messageName;
            MsgBody = messageBody;
        }

        public void AddRecipient(string recipientNumber)
        {
            Mobiles.Add(new SlngMobile() { Mobile = recipientNumber });
        }
    }

    public class SlngResponse
    {
        public string Status { get; set; }
        public string Description { get; set; }
    }
}
