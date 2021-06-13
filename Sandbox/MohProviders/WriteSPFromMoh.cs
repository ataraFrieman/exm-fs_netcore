using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text;
namespace MohProviders
{
    public class WriteSPFromMoh
    {
        public WriteSPFromMoh()
        {
            // InitializeComponent();
        }

        static List<ServiceProviderVM> providers = new List<ServiceProviderVM>();
        string providersPathName = @"C:\QUZE\CSVWrite\sp.csv";
        string expertiesPathName = @"C:\QUZE\CSVWrite\‏‏Experties.csv";
        string providersUrl = @"https://www.old.health.gov.il/oskimbbriut/rufim/DoctorSearchNew.asp?p=";
        string expertiesUrl = @"https://www.old.health.gov.il/oskimbbriut/rufim/AlBsisitNew.asp?AcNam=%F4%F8%E8%E9%ED%20%F2%EC%20%EE%E5%EE%E7%E9%E5%FA%20%20%E3%27%F8&AcotCode=";
        string spNewUrl1 = "https://www.old.health.gov.il/oskimbbriut/rufim/DoctorSearchNew.asp?p=";
        string spNewUrl2 = "&Yshuv=&FirstName=&LastName=&RegNum=&Mumhiut=";


        StreamWriter providersSW, expertiesSW;

        //pagesCount 2102
        int pagesCount = 2102, pageIndex = 1;

        public async void button1_Click()
        {
            File.Delete(providersPathName);
            File.Delete(expertiesPathName);

            providersSW = new StreamWriter(providersPathName, false, Encoding.UTF8);
            expertiesSW = new StreamWriter(expertiesPathName, false, Encoding.UTF8);
            System.Text.Encoding utf_8 = System.Text.Encoding.UTF8;

            for (; pageIndex <= pagesCount; pageIndex++)
            {
                Console.WriteLine("page " + pageIndex.ToString());
                var table = await GetProvidersTableFromPage(providersUrl + pageIndex.ToString());
                await WriteProvidersFromTableToCsvFile(table);
            }


            providersSW.Close();
            expertiesSW.Close();

            Console.WriteLine("Done!!!");
        }

        private async Task<HtmlNode> GetProvidersTableFromPage(string pageUrl)
        {
            HtmlAgilityPack.HtmlDocument doc = await GetHtmlDocument(pageUrl);
            var tables = doc.DocumentNode.Descendants("table").ToList();

            var table = tables[4];
            return table;
        }

        private async Task<HtmlAgilityPack.HtmlDocument> GetHtmlDocument(string url)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding.GetEncoding("windows-1255");
                System.Text.Encoding utf_8 = System.Text.Encoding.UTF8;

                Encoding latinEncoding = Encoding.GetEncoding("Windows-1255");
                Encoding hebrewEncoding = Encoding.GetEncoding("Windows-1255");


                var http = new HttpClient();
                var response = await http.GetByteArrayAsync(url);
                String source = hebrewEncoding.GetString(response);

                source = WebUtility.HtmlDecode(source);
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(source);
                return doc;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.Read();
                return null;
            }
        }

        private async Task<bool> WriteProvidersFromTableToCsvFile(HtmlNode table)
        {
            var rows = table.SelectNodes("tr");
            for (int i = 1; i < 21; i++)
            {
                if (pageIndex == pagesCount && rows.Count == i) break;

                var row = rows[i];

                //Console.WriteLine(row.InnerHtml);

                await HandleProviderRow(row);
                
            }
            return true;
        }

        private async Task<bool> HandleProviderRow(HtmlNode row)
        {
            HtmlNodeCollection cells = row.SelectNodes("td");
            if (cells.Count < 6)
            {
                return false;
            }
            if (cells == null)
            {
                return false;
            }
            var person = new ServiceProviderVM()
            {
                FirstName = cells[6].InnerHtml.Trim(),
                LastName = cells[7].InnerHtml.Trim(),
               // LicenseNumber = cells[5].InnerHtml.Trim(),
                CityName = cells[4].InnerHtml.Trim(),
                LicenseReceiptDateString = cells[3].InnerHtml.Trim()
            };

            var img = cells[0].Descendants("Img").FirstOrDefault();
            if (img != null)
            {
                var value = img.Attributes["onclick"].Value;
                var n = (value.Substring(39));
                var endnum = n.IndexOf("\'");
                var num = n.Substring(0, endnum);

                //var url = "https://www.old.health.gov.il/oskimbbriut/rufim/AlBsisitNew.asp?AcNam=";
                //url +=;
                //url += "&AcotCode=" + num;
               // await HandleExperties(num, person.LicenseNumber);
            }

            //File.AppendAllLines(providersPathName, new List<string>() { person.ToString() }, Encoding.UTF8);
           // await providersSW.WriteLineAsync(person.ToString());
            return true;
        }

        private async Task<bool> HandleExperties(string num, string licenseNumber)
        {
            var expertiesUrl = this.expertiesUrl + num;
            try
            {
                Console.WriteLine(expertiesUrl);
                HtmlAgilityPack.HtmlDocument doc = await GetHtmlDocument(expertiesUrl);
                var table = doc.DocumentNode.Descendants("table").FirstOrDefault();
                var rows = table.SelectNodes("tr");

                for (int i = 2; i <= rows.Count - 3; i++)
                {
                    var row = rows[i];
                    Console.WriteLine(row.InnerHtml);
                    await WriteExpertyRowToCSV(row, licenseNumber);

                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return true;
        }

        private async Task<bool> WriteExpertyRowToCSV(HtmlNode row, string licenseNumber)
        {
            HtmlNodeCollection cells = row.SelectNodes("td");
            if (cells.Count < 4)
            {
                return false;
            }
            if (cells == null)
            {
                return false;
            }

            var csvRow = string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", licenseNumber, cells[0].InnerText.Trim(), cells[1].InnerText.Trim(), cells[2].InnerText.Trim(), cells[3].InnerText.Trim());

            // File.AppendAllLines(expertiesPathName, new List<string>() { csvRow }, Encoding.UTF8);
            try
            {
           await expertiesSW.WriteLineAsync(csvRow);

            }
            catch (Exception e)
            {

                throw;
            } 

            return true;
        }
    }
}

