using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SendingDataToDB
{
    public class SendListToDB<TVM>
    {
        private List<TVM> list = new List<TVM>();
        private string baseUrl = "http://localhost:30025/";
        private string subUrl;
        private string token = "Bearer EyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMCIsInVuaXF1ZV9uYW1lIjoiMDUyNzE3NDIxMCIsImdpdmVuX25hbWUiOiLXoteV15bXmTEyMzEyMzEyMzgiLCJmYW1pbHlfbmFtZSI6IteR16jXmdeZ16g1NjQ1NDE0NSIsImVtYWlsIjoiIiwiZXhwIjoxNTU0ODg3NDg2LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiQVBJX1VzZXIifQ.Ew747fE9cSiPnFh4jnW1L1UCI74sAt_krvjNiCwLzBU";
        private static int counter = 0;

        public string BaseUrl
        {
            get { return baseUrl; }
            set { baseUrl = value; }
        }

        public SendListToDB(string subUrl, List<TVM> list)
        {
            this.subUrl = baseUrl + subUrl;
            this.list = list;
            SendListToDBAsync();
        }

        private async void SendListToDBAsync()
        {

            foreach (var li in list)
            {
                counter++;
                var request = new Request<TVM>() { Entity = li };
                var requestJson = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                await PostAsync(requestJson, subUrl);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0} send succesfuly", counter);
            }
        }

        private static async Task PostAsync(string json, string url)
        {

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMjI3MSIsInVuaXF1ZV9uYW1lIjoiMDUyNzE3NDIxNCIsInR5cCI6IjMiLCJnaXZlbl9uYW1lIjoiIiwiZmFtaWx5X25hbWUiOiIiLCJlbWFpbCI6IiIsIk9yZ2FuaXphdGlvbklkIjoiNSIsIklkZW50aXR5TnVtYmVyIjoiMjI1NDQ4MjMyIiwiZXhwIjoxNTU5OTk3Njk2LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiQVBJX1VzZXIifQ.BtVIaSo56cNp7AdS67fswuLZ1debGPtIBsTSdU6fejk");

                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {

                    //stringContent not contains the json data
                    request.Content = stringContent;
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false);
                    {
                        var resultString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            var quzeResponse = JsonConvert.DeserializeObject<Quze.Models.Response<TVM>>(resultString);
                            if (quzeResponse.ResultCode == 0)
                            {

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("{0}  CREATED!!!!", json);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("{0} Not CREATED!!!!", json);
                                Console.WriteLine(quzeResponse.Errors[0].Description);
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("{0} NOT CREATED!!!!", json);
                        }
                    }

                }

            }

        }

    }
}
