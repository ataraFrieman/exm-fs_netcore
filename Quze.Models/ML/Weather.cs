using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Models.ML
{
    public class Weather
    {
        DateTime timeInDay;
        public float MinTemp { get; set; }
        public float MaxTemp { get; set; }
        public float Rainfall { get; set; }
        public float BarometricPressure { get; set; }
    }
}
