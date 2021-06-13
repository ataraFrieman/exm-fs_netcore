
using Newtonsoft.Json;
using Quze.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Quze.Models.Entities;
using System.Net.Http.Headers;

namespace BLTests
{
    


    class HttpUtil
    {
        public static async void Post(string url, object obj)
        {
            try
            {

                var entitties = new { entities = obj };
                var content = Newtonsoft.Json.JsonConvert.SerializeObject(entitties);

                await PostAsync(content, "http://localhost:30025/" + url);
            }
            catch (Exception ex)
            {

                throw;
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
                    try
                    {
                        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false);
                        {
                            var resultString = await response.Content.ReadAsStringAsync();

                            if (response.IsSuccessStatusCode)
                            {

                                if (response.IsSuccessStatusCode)
                                {

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("{0}  CREATED!!!!", json);
                                    Console.WriteLine(response.ToString());
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("{0} Not CREATED!!!!", json);
                                    //Console.WriteLine(quzeResponse.Errors[0].Description);
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("{0} NOT CREATED!!!!", json);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                  

                }

            }

        }




        //public async Task<Response<TEntityVM>> GetAsync()
        //{
        //    var entities = await store.ToListAsync();
        //    var entitiesVM = mapper.Map<List<TEntityVM>>(entities);
        //    var response = new Response<TEntityVM> { Entities = entitiesVM };
        //    return response;
        //}
    }

}