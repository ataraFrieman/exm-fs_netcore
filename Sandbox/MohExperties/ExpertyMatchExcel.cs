using SendingFilesToDB;
using System;
using System.Collections.Generic;
using System.Text;
using Quze.Models.Models.ViewModels;

namespace MohExperties
{
    public class ExpertyMatchExcel: SendingFilesToDB.IConvertToVM<ExpertyVM>
    {
        public string Description { get; set; }
        public string MohCode { get; set; }

        public ExpertyVM Convert()
        {
            var experty = new ExpertyVM()
            {
                Description=Description,
                MohCode=MohCode
               
            };

            return experty;
        }

     
    }
}
