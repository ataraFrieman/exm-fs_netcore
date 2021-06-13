using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Models.Entities
{
    public class Location : EntityBase
    {
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Continent { get; set; }
        public string ContinentCode { get; set; }
        public decimal Langtitude { get; set; }
        public decimal  Longtitude { get; set; }
        public int CallingCode { get; set; }
    }
}
