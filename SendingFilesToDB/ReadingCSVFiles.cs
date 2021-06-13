using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SendingFilesToDB
{
    public static class ReadingCSVFiles<T> where T :  new()
    {
        public static List<T> GetFromFileToList(string path)
        {

            List<T> list = new List<T>();
            int counter = 0;
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader))
            {
                var records = csv.GetRecords<T>();

                foreach (var record in records)
                {
                    counter++;
                    list.Add(record);
                }


                return list;
            }
        }
    }
}