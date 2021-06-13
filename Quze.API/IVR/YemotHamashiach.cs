using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Quze.API.IVR
{
    public class YemotHamashiach
    {
        string yemotURL = "https://www.call2all.co.il/ym/api";
        string templateId = "751708";
        string token = String.Empty;
        public async Task<Campaign> sendIVRMessageAsync( Message message)
        {
            token = await GetYemotTokenAsync();
            var tts = new GoogleTextToSpeech();
            var fileName = tts.FileFromText(message.TextToSpeech);
            var file = File.ReadAllBytes(fileName);
            var a = await UploadFile(file, fileName);
            var campaignResp = await RunCampaign(message.Phone);
            var campaign = JsonConvert.DeserializeObject<Campaign>(campaignResp);
            File.Delete(fileName);
            return campaign;
        }

        public async Task<string> UploadFile(byte[] file, string fileName)
        {
            var fileContent = new ByteArrayContent(file);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "upload",
                FileName = fileName
            };
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mp3");
            var builder = new UriBuilder(yemotURL + @"/UploadFile");
            string url = builder.ToString();
            using (var client = new HttpClient())
            {
                var requestContent = new MultipartFormDataContent();
                requestContent.Add(new StringContent(token), "token");
                requestContent.Add(new StringContent(templateId + ".wav"), "path");
                requestContent.Add(new StringContent("1"), "convertAudio");
                requestContent.Add(new StringContent("Submit"), "submit");
                requestContent.Add(fileContent, templateId + ".wav");
                return await client.PostAsync(url, requestContent).Result.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> GetYemotTokenAsync()
        {
            var builder = new UriBuilder(yemotURL + @"/Login");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["username"] = "0795466969";
            query["password"] = "777719";
            builder.Query = query.ToString();
            string url = builder.ToString();
            using (var client = new HttpClient())
            {
                var rsp = await client.GetAsync(url).Result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<Token>(rsp);
                if (response.responseStatus == "OK")
                {
                    return response.token;
                }
                else
                {
                    return "error";
                }
            }
        }

        public async Task<string> RunCampaign(string phone)
        {
            var builder = new UriBuilder(yemotURL + @"/RunCampaign");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["token"] = token;
            query["templateId"] = templateId;
            query["phones"] = phone;
            builder.Query = query.ToString();
            string url = builder.ToString();
            using (var client = new HttpClient())
            {
                var rsp = await client.GetAsync(url).Result.Content.ReadAsStringAsync();
                return rsp;
            }
        }

        
        public class Token
        {
            public string token { get; set; }
            public string responseStatus { get; set; }
        }

        public class Campaign
        {
            public string campaignId { get; set; }
            public string responseStatus { get; set; }
        }
    }
}
