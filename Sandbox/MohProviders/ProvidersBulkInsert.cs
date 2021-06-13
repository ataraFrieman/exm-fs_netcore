using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quze.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MohProviders
{
    public class ProvidersBulkInsert
    {
        const string spFilePathName = @"C:\QUZE\ExcelFile\sp.csv";
        const string baseUrl = "http://localhost:30025";
        string spUrl = baseUrl + "/serviceProviders/";

        List<CityVM> cities;
        List<OrganizationVM> organizations;

        public async void GetFromFileAndSendAll()
        {
            await GetCitiesAndOrganizations();
            int i = 0;
            var providers = await GetProvidersFromFile();
            foreach (var sp in providers)
            {
                var request = new Request<ServiceProviderVM>() { Entity = sp };
                var requestJson = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                await PostAsync(requestJson, spUrl);
            }
        }

        private async Task GetCitiesAndOrganizations()
        {
            organizations = await GetOrganizationsAsync();

            cities = await GetCitiesAsync();

        }

        public static async Task<List<OrganizationVM>> GetOrganizationsAsync()
        {
            Response<OrganizationVM> responseOrganizations;
            using (var client = new HttpClient())
            {

                var httpRequest = new HttpRequestMessage(HttpMethod.Get, baseUrl + "/organizations/");
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

        private async Task<List<CityVM>> GetCitiesAsync()
        {
            Response<CityVM> responseCities;
            using (var client = new HttpClient())
            {

                var cities = new HttpRequestMessage(HttpMethod.Get, baseUrl + "/cities");
                cities.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMjI3MSIsInVuaXF1ZV9uYW1lIjoiMDUyNzE3NDIxNCIsInR5cCI6IjMiLCJnaXZlbl9uYW1lIjoiIiwiZmFtaWx5X25hbWUiOiIiLCJlbWFpbCI6IiIsIk9yZ2FuaXphdGlvbklkIjoiNSIsIklkZW50aXR5TnVtYmVyIjoiMjI1NDQ4MjMyIiwiZXhwIjoxNTU5OTk3Njk2LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiQVBJX1VzZXIifQ.BtVIaSo56cNp7AdS67fswuLZ1debGPtIBsTSdU6fejk");

                try
                {
                    var citiesResults = await client.SendAsync(cities, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false);
                    var citiesResultsString = await citiesResults.Content.ReadAsStringAsync();
                    responseCities = JsonConvert.DeserializeObject<Response<CityVM>>(citiesResultsString);

                    Console.WriteLine(responseCities);
                }
                catch (Exception)
                {

                    throw;
                }

                return responseCities.Entities;
            }
        }

        private int GetCityIdByName(String cityName)
        {
            var city = cities.Where(o => o.Name == cityName).FirstOrDefault();
            if (city == null) city = cities[0];
            return city.Id;
        }

        private ServiceProviderVM GetProviderFromLine(string line)
        {
            var organizationIndex = new Random().Next(0, organizations.Count - 1);
            var organizationId = organizations[organizationIndex].Id;

            var props = line.Split(',');

            var sp = new ServiceProviderVM()
            {
                FirstName = props[0],
                LastName = props[1],
                //LicenseNumber = props[2],
                CityId = GetCityIdByName(props[3]),
                LicenseReceiptDate = DateTime.Parse(props[4]),
                OrganizationId = organizationId
            };

            return sp;
        }

        private async Task<List<ServiceProviderVM>> GetProvidersFromFile()
        {
            //read file 
            List<ServiceProviderVM> list = new List<ServiceProviderVM>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string fileContent = await File.ReadAllTextAsync(spFilePathName, Encoding.GetEncoding(1255));

            using (var reader = new StringReader(fileContent))
            {
                reader.ReadLine();//כותרת
                while (reader.Peek() > 0)
                {
                    var line = reader.ReadLine();
                    var provider = GetProviderFromLine(line);
                    list.Add(provider);
                }
            }
            return list;
        }

        private static async Task PostAsync(object obj, string url)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            await PostAsync(str, url);
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
