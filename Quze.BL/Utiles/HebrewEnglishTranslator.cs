using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace Quze.BL.Utiles
{
    public class HebrewEnglishTranslator

    {

        public string Translate(string word, string languagePair = "en|iw")
        {
            var toLanguage = "en";//English
            var fromLanguage = "iw";//Deutsch
            word = HttpUtility.UrlEncode(word);
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q=" + word;
            var webClient = new WebClient
            {
                Encoding = System.Text.Encoding.UTF8
            };
            var result = webClient.DownloadString(url);
            try
            {
                result = result.Substring(4, result.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
                return result;
            }
            catch
            {
                return "Error";
            }

        }
    }
}
