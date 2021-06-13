using System;
using Quze.Models.Entities;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quze.Organization.Web.ViewModels
{
    public class RegitrationStepBResponse
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        //public Fellow Fellow { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
        public Quze.Models.Entities.Organization Organization { get; set; }
    }
}
