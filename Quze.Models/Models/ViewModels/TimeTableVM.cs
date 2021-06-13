using System;
using System.Collections.Generic;
using Quze.Models.Entities;

namespace Quze.Models.Models.ViewModels
{
    public class TimeTableVM : BaseVM
    {
    

        public int ServiceProviderId { get; set; }
        public int BranchId { get; set; }

        public DateTime ValidFromDate { get; set; }
        public DateTime ValidUntilDate { get; set; }
        public List<TimeTableLineVM> TimeTableLines { get; set; }
        public Branch Branch { get; set; }
        public ServiceProvider ServiceProvider { get; set; }

        public override string ToString()
        {
            var text = base.ToString();
            text += Environment.NewLine + string.Format("ProviderId: {0}, BranchId: {1}", ServiceProviderId, BranchId);
            foreach (var item in TimeTableLines)
            {
                text += Environment.NewLine + item.ToString();
            }
            return text;
        }
    }
}
