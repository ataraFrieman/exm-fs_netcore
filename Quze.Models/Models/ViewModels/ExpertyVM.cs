using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Models.ViewModels
{
    public class ExpertyVM:BaseVM
    {
    
        public string Description { get; set; }
        public string MohCode { get; set; }
        
    }
}
