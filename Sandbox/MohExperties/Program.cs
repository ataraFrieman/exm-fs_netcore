using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quze.Models;
using Quze.Models.Models.ViewModels;
using SendingDataToDB;
using SendingFilesToDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace MohExperties
{
    public class Program
    {
        const string expertiesFilePathName = @"C:\‏‏Experties.csv";
        const string baseUrl = "http://localhost:30025";
        static string expertyUrl = "experties/";

        static void Main(string[] args)
        {
            // SendExperties();

            sendList();


            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        public static void sendList()
        {
            List<ExpertyVM> list = new List<ExpertyVM>();
            var s = ReadingCSVFiles<ExpertyMatchExcel>.GetFromFileToList(expertiesFilePathName);
            list = ClearList(s);
            var send = new SendListToDB<ExpertyVM>(expertyUrl, list);

        }

        private static List<ExpertyVM> ClearList(List<ExpertyMatchExcel> csvList)
        {
            List<ExpertyVM> list = new List<ExpertyVM>();

            foreach (var record in csvList)
            {
                bool flag = true;
                string code = record.MohCode.Substring(0, record.MohCode.IndexOf('-'));

                int beginning=code.Length;
                int end=record.MohCode.Length - code.Length;

                var checkClean =string.Concat(record.MohCode[0],0);//.Substring(beginning);
                int temp;

                flag = int.TryParse(checkClean, out temp);
                if (flag)
                {

                    foreach (var li in list)
                    {
                        if (li.MohCode.Equals(code) || li.Description.Equals(record.Description))
                        {
                            flag = false;
                            break;
                        }

                    }
                }
                if (flag == true)
                    list.Add(new ExpertyVM
                    {
                        MohCode = code,
                        Description = record.Description
                    });

            }
            return list;
        }

        private static async void SendExperties()
        {
            //read file
            string fileContent = await File.ReadAllTextAsync(expertiesFilePathName, Encoding.GetEncoding(12));

            using (var reader = new StringReader(fileContent))
            {

                reader.ReadLine();//כותרת
                while (reader.Peek() > 0)
                {
                    var line = reader.ReadLine();
                    string ExpertiesJsonString = GetExpertiesJsonFromLine(line);
                    //try
                    //{
                    //    await PostBasicAsync(ExpertiesJsonString, url);
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}
                }
            }

        }

        public static string GetExpertiesJsonFromLine(string line)
        {
            var props = line.Split(',');
            //var props1 = line.Split('|');
            string json = "{Entity:{MohDoctor:\"" + props[0] +
                "\",LicenseNumber:\"" + props[1] +
                "\",TypeExperties:\"" + props[2] +
                "\",MohCodeExperties:\"" + props[3] +
                "\",CountExperties:\"" + props[4] +
                "\",OrganizationId:\"3\"}}";

            return json;
        }

        private static async Task PostBasicAsync(string json, string url)
        {

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMjI3MSIsInVuaXF1ZV9uYW1lIjoiMDUyNzE3NDIxNCIsInR5cCI6IjMiLCJnaXZlbl9uYW1lIjoiIiwiZmFtaWx5X25hbWUiOiIiLCJlbWFpbCI6IiIsIk9yZ2FuaXphdGlvbklkIjoiNSIsIklkZW50aXR5TnVtYmVyIjoiMjI1NDQ4MjMyIiwiZXhwIjoxNTU5OTk3Njk2LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiQVBJX1VzZXIifQ.BtVIaSo56cNp7AdS67fswuLZ1debGPtIBsTSdU6fejk");

                var cities = new HttpRequestMessage(HttpMethod.Get, "http://localhost:30025/cities");
                cities.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMjI3MSIsInVuaXF1ZV9uYW1lIjoiMDUyNzE3NDIxNCIsInR5cCI6IjMiLCJnaXZlbl9uYW1lIjoiIiwiZmFtaWx5X25hbWUiOiIiLCJlbWFpbCI6IiIsIk9yZ2FuaXphdGlvbklkIjoiNSIsIklkZW50aXR5TnVtYmVyIjoiMjI1NDQ4MjMyIiwiZXhwIjoxNTU5OTk3Njk2LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiQVBJX1VzZXIifQ.BtVIaSo56cNp7AdS67fswuLZ1debGPtIBsTSdU6fejk");

                var citiesResults = await client.SendAsync(cities, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false);
                var citiesResultsString = await citiesResults.Content.ReadAsStringAsync();
                var resCities = JsonConvert.DeserializeObject<Response<CityVM>>(citiesResultsString);

                JObject jsonObj = JObject.Parse(json);
                JObject Entity = (JObject)jsonObj["Entity"];

                var cityVM = resCities.Entities.Find(city => city.Name == Entity["streetId"].ToString());

                Entity["streetId"] = cityVM.Id;
                json = jsonObj.ToString();

                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {

                    // stringContent not contains the json data
                    request.Content = stringContent;
                    var response = await client
                                            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false);
                    {
                        var resultString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            var quzeResponse = JsonConvert.DeserializeObject<Response<ExpertyVM>>(resultString);
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


