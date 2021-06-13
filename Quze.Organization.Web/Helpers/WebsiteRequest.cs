
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Quze.Models.Entities;
using Quze.Organization.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;


namespace Quze.Organization.Web.Helpers
{
    public class WebsiteRequest
    {

        private string WebsitUrl;
        private readonly IConfiguration configuration;

        public WebsiteRequest(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.WebsitUrl = configuration.GetConnectionString("WebsiteUrl");
        }

        public static void SendRequestToWebsiteServer(List<AppointmentPushVM> request, string url,ILogger logger)
        {
            if (request != null && request.Count > 0)
            {
                var dataString = JsonConvert.SerializeObject(request);
                SendRequest(dataString, url,logger);
            }
        }

        public static void SendRequestToWebsiteServer(Appointment appointment, string url, ILogger logger)
        {
            if (appointment != null)
            {
                //appointment.ServiceType.RequiredTasks.ForEach(e => e.ServiceType = null);
                //appointment.ServiceType.RequiredDocuments.ForEach(e => e.ServiceType = null);
                appointment.ServiceQueue.ServiceProvider.Organization = null;
                appointment.ServiceQueue.Branch.Organization = null;
                try {
                    var dataString = JsonConvert.SerializeObject(appointment, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                    SendRequest(dataString, url, logger);
                }
                catch (Exception ex) {
                    logger.LogInformation("SendRequest : {Message}", ex.Message+" "+(ex.InnerException!=null?ex.InnerException.Message:""));
                }
                }
        }

        private static void SendRequest(string dataString, string url, ILogger logger)
        {
            try
            {
                using (var client = new WebClient())
                {
                    logger.LogInformation("SendRequest : {Message}", url);

                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    var response = client.UploadString(url, "POST", dataString);
                    var responseString = response;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
