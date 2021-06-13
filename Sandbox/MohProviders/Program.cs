using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quze.DAL;
using Quze.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MohProviders
{
    class Program
    {
        const string baseUrl = "http://localhost:30025";



        static void Main(string[] args)
        {
            var providersBulkInsert = new ProvidersBulkInsert();
            providersBulkInsert.GetFromFileAndSendAll();

            //var writeSPFromMoh = new WriteSPFromMoh();
            //writeSPFromMoh.button1_Click();



            Console.ReadKey();
        }

    }

}
