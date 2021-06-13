using Newtonsoft.Json;
using Quze.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SendingDataToDB
{
    public class GetListFromDB<TVM>
    {
        public static async Task<List<TVM>> GetListAsync(string subUrl)
        {
            const string baseUrl = "http://localhost:30025";
            Response<TVM> response;
            var list = new List<TVM>();
            using (var client = new HttpClient())
            {

                var httpRequest = new HttpRequestMessage(HttpMethod.Get, baseUrl + subUrl);
                httpRequest.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMjI3MSIsInVuaXF1ZV9uYW1lIjoiMDUyNzE3NDIxNCIsInR5cCI6IjMiLCJnaXZlbl9uYW1lIjoiIiwiZmFtaWx5X25hbWUiOiIiLCJlbWFpbCI6IiIsIk9yZ2FuaXphdGlvbklkIjoiNSIsIklkZW50aXR5TnVtYmVyIjoiMjI1NDQ4MjMyIiwiZXhwIjoxNTU5OTk3Njk2LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiQVBJX1VzZXIifQ.BtVIaSo56cNp7AdS67fswuLZ1debGPtIBsTSdU6fejk");


                try
                {
                    var Results = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false);
                    var ResultsString = await Results.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<Response<TVM>>(ResultsString);

                    Console.WriteLine(response);
                }
                catch (Exception)
                {

                    throw;
                }

                return response.Entities;
            }
        }


    }
}
