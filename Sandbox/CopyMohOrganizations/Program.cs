using MohOrganizations;
using Newtonsoft.Json;
using Quze.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CopyMohOrganizations
{
    class Program
    {

        const string url = "http://localhost:30025/organizations/";


        static void Main(string[] args)
        {
            SendOrganizations();

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        private static async void SendOrganizations()
        {
            //Read file

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string fileContent = File.ReadAllText(@"c:\Temp\Institute.csv", Encoding.GetEncoding(1255));


            using (var reader = new StringReader(fileContent))
            {
                reader.ReadLine();//כותרת
                while (reader.Peek() > 0)
                {
                    var line = reader.ReadLine();
                    string organizationJsonString = GetOrganizationJsonFromLine(line);
                    try
                    {
                        await PostBasicAsync(organizationJsonString, url);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }

        }


        public static string GetOrganizationJsonFromLine(string line)
        {

            var props = line.Split('|');
            //var props = line.Split(',');
            for (int i = 0; i < props.Length; i++)
            {
                props[i] = props[i].Replace(@"""", @"\""");
            }
            
            string json = "{Entity:{MohCode:\"" + props[0] + "\",Name:\"" + props[1] + "\",Description:\"" + props[2] + "\",OrganizationTypeCode:\"" + props[3] + "\",CityCode:\"" + props[4] + "\",ZipCode:\"" + props[5] + "\",Address:\"" + props[6] + "\"}}";
            return json;
        }

        private static async Task PostBasicAsync(string json, string url)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMjI3MSIsInVuaXF1ZV9uYW1lIjoiMDUyNzE3NDIxNCIsInR5cCI6IjMiLCJnaXZlbl9uYW1lIjoiIiwiZmFtaWx5X25hbWUiOiIiLCJlbWFpbCI6IiIsIk9yZ2FuaXphdGlvbklkIjoiNSIsIklkZW50aXR5TnVtYmVyIjoiMjI1NDQ4MjMyIiwiZXhwIjoxNTU5OTk3Njk2LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiQVBJX1VzZXIifQ.BtVIaSo56cNp7AdS67fswuLZ1debGPtIBsTSdU6fejk");
                //var json = JsonConvert.SerializeObject(content);
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None)
                        .ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var resultString = await response.Content.ReadAsStringAsync();
                            var quzeResponse = JsonConvert.DeserializeObject<Response<OrganizationVM>>(resultString);
                            if (quzeResponse.ResultCode == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("{0}  CREATED!!!!", json);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("{0} recived by server - BUT Not CREATED!!!!", json);
                                Console.WriteLine(quzeResponse.Errors[0].Description);
                            }
                        }
                        else
                        {
                            var resultString = await response.Content.ReadAsStringAsync();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("{0} NOT CREATED!!!!", json);
                            Console.WriteLine(resultString);

                        }


                    }
                }
            }
        }
    }
}
