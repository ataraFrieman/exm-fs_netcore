using Quze.Models.Entities;
using System;
using Quze.Models.Models.ViewModels;

namespace MohExperties
{
    public class ExpertyVM:BaseVM
    {
        public string Description { get; set; }
        public string MohCode { get; set; }
    }
}
