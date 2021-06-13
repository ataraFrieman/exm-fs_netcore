using CsvHelper;
using MohServiceType;
using Newtonsoft.Json;
using Quze.Models;
using SendingDataToDB;
using SendingFilesToDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MohProviders;
using System.Linq;
using System.Text;
using System.Net.Http.Headers;

namespace MohSrviceType
{
    class Program
    {
        private static List<MohServiceType.ServiceTypeVM> list;
        private static List<int> organizationIDList = new List<int>();
        const string baseUrl = "http://localhost:30025";
        const string fielPath = "C:\\QUZE\\CSVFile\\ServiceType.csv";
        const string subUrl = @"ServiceType\";
        private static List<MohServiceType.ServiceTypeVM> parentSTList;


        static void Main(string[] args)
        {
            //string subUrl = "serviceTypes/";
            //string path = "C://QUZE//ExcelFile//ServiceType.csv";

            //SendListAsync();

            CreateProvidersList();
        }

        public static async Task SendListAsync()
        {
            await FillOrganizationIDListAndParentST();
            fillListFromFile();

            var s = new SendListToDB<MohServiceType.ServiceTypeVM>(subUrl, list);
        }

        private static int GetParentST(string code)
        {

            return 1;
        }

        private static async Task FillOrganizationIDListAndParentST()
        {
            //var oList = await GetListFromDB<OrganizationVM>.GetListAsync("/organizations");

            var oList = await ProvidersBulkInsert.GetOrganizationsAsync();
            var pList = await GetListFromDB<OrganizationVM>.GetListAsync("/SrviceType");


            foreach (var o in oList)
            {
                organizationIDList.Add(o.Id);
            }
        }

        public static async Task<List<OrganizationVM>> GetOrganizationsAsync()
        {
            Response<OrganizationVM> responseOrganizations;
            using (var client = new HttpClient())
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, baseUrl + "/organizations");
                httpRequest.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMjI3MSIsInVuaXF1ZV9uYW1lIjoiMDUyNzE3NDIxNCIsInR5cCI6IjMiLCJnaXZlbl9uYW1lIjoiIiwiZmFtaWx5X25hbWUiOiIiLCJlbWFpbCI6IiIsIk9yZ2FuaXphdGlvbklkIjoiNSIsIklkZW50aXR5TnVtYmVyIjoiMjI1NDQ4MjMyIiwiZXhwIjoxNTU5OTk3Njk2LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiQVBJX1VzZXIifQ.BtVIaSo56cNp7AdS67fswuLZ1debGPtIBsTSdU6fejk");


                try
                {
                    var organizationsResults = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false);
                    var organizationsResultsString = await organizationsResults.Content.ReadAsStringAsync();
                    responseOrganizations = JsonConvert.DeserializeObject<Response<OrganizationVM>>(organizationsResultsString);

                    Console.WriteLine(responseOrganizations);
                }
                catch (Exception)
                {

                    throw;
                }

                return responseOrganizations.Entities;
            }
        }

        public static async Task<List<MohServiceType.ServiceTypeVM>> GetParentSTAsync()
        {
            Response<MohServiceType.ServiceTypeVM> responseParentST;
            using (var client = new HttpClient())
            {

                var httpRequest = new HttpRequestMessage(HttpMethod.Get, baseUrl + "/SrviceType");
                httpRequest.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMjI3MSIsInVuaXF1ZV9uYW1lIjoiMDUyNzE3NDIxNCIsInR5cCI6IjMiLCJnaXZlbl9uYW1lIjoiIiwiZmFtaWx5X25hbWUiOiIiLCJlbWFpbCI6IiIsIk9yZ2FuaXphdGlvbklkIjoiNSIsIklkZW50aXR5TnVtYmVyIjoiMjI1NDQ4MjMyIiwiZXhwIjoxNTU5OTk3Njk2LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiQVBJX1VzZXIifQ.BtVIaSo56cNp7AdS67fswuLZ1debGPtIBsTSdU6fejk");


                try
                {
                    var organizationsResults = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false);
                    var organizationsResultsString = await organizationsResults.Content.ReadAsStringAsync();
                    responseParentST = JsonConvert.DeserializeObject<Response<MohServiceType.ServiceTypeVM>>(organizationsResultsString);

                    Console.WriteLine(responseParentST);
                }
                catch (Exception)
                {

                    throw;
                }

                return responseParentST.Entities;
            }
        }

        private static int GetRandomOrganozation()
        {
            Random rnd = new Random();

            int num = rnd.Next(0, organizationIDList.Count);

            return organizationIDList[num];

        }

        private static void fillListFromFile()
        {
            var fileList = ReadingCSVFiles<ServiceTypeMatchCSVFile>.GetFromFileToList(fielPath);

            foreach (var st in fileList)
            {
                list.Add(new MohServiceType.ServiceTypeVM()
                {
                    OrganizationId = GetRandomOrganozation(),
                    Code = st.Code,
                    Description = st.Description,
                    Cost = st.Price1,
                    ParentServiceId = GetParentST(st.Code),
                    IsVisibleToApp = false,
                    IsVisibleToOrganization = true,
                });
            }
        }


        public static void CreateProvidersList()
        {
            //TODO : to convert the Arrays to Lists in order to removeAll the unwanted chars
            List<int> exsp = new List<int>();
            var firstNames = File.ReadAllText(@"c:\users\Barukh\Desktop\example.txt", Encoding.Default).Split(" ");
            firstNames = firstNames.Where(l => l.Trim().Length > 0).ToArray();

            var lastNames = File.ReadAllLines(@"c:\users\Barukh\Desktop\example2.txt", Encoding.Default);
            lastNames = lastNames.Where(l => l.Trim().Length > 0).ToArray();

            List<string> Cites = new List<string> { "ירושלים", "תל אביב", "חיפה", "באר שבע", "צפת", "חולון", "רמת גן", " גבעתיים", "בת ים", "נתניה", "חדרה", "נהריה", "אילת" };


            List<ServiceProviderVM> list = new List<ServiceProviderVM>();
            string uri = "ServiceProvider";
            Random random = new Random();
            for (int i = 0; i <= 4; i++)
            {
                var firstNameIndex = new Random().Next(0, firstNames.Length - 1);
                var firstName = firstNames[firstNameIndex];
                var lastNameIndex = new Random().Next(0, lastNames.Length - 1);
                var lastname = lastNames[lastNameIndex];
                int current = new Random().Next(0, 100000000);
                string identityNumber = current.ToString();
                int current1 = random.Next(0, 5);
                int organizationId = current1;
                int current2 = random.Next(0, 1000);
                string licenseNumber = current2.ToString();
                int current3 = random.Next(0, 100);
                int cityId = current3;
                var find = new Random().Next(20, 80);
                var date = DateTime.Now;
                var licenseReceiptDate = date.AddYears(-find);


                var CityNameIndex = new Random().Next(0, Cites.Count - 1);
                string cityName = Cites[CityNameIndex];
                ServiceProviderVM SP = new ServiceProviderVM()
                {
                    IdentityNumber = identityNumber,
                    FirstName = firstName,
                    LastName = lastname,
                    OrganizationId = organizationId,
                    LicenseNumber = licenseNumber,
                    CityId = cityId,
                    LicenseReceiptDate = licenseReceiptDate
                };
                list.Add(SP);
            }

            HttpUtil.Post(uri, list);




        }

    }


    class HttpUtil
    {
        public static async void Post(string url, object obj)
        {
            try
            {
                
                var entitties = new { entities = obj };
                var content = Newtonsoft.Json.JsonConvert.SerializeObject(entitties);

                await PostAsync(content, "http://test.quze.ai:30025/" + url);
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
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false);
                    {
                        var resultString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            var quzeResponse = JsonConvert.DeserializeObject<Response<ServiceProviderVM>>(resultString);
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