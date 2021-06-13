using Quze.Infrastruture.Extensions;
using Quze.Infrastruture.Types;
using System;
using System.Collections.Generic;

namespace Quze.Organization.Web.ViewModels
{
    public class TimeTableVM : BaseVM
    {
        public TimeTableVM()
        {

        }
        public int ServiceTypeId { get; set; }
        public int ServiceProviderId { get; set; }
        //public int OrganizationId { get; set; }
        public int BranchId { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime ValidUntilDate { get; set; }

        public List<TimeTableLineVM> TimeTableLines { get; set; }
        public BranchVM Branch { get; set; }
        public ServiceProviderVM ServiceProvider { get; set; }
        public ServiceTypeVM ServiceType { get; set; }

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

      

        public void AddTimeTableLine(TimeTableLineVM line)
        {
            if (TimeTableLines.IsNull()) TimeTableLines = new List<TimeTableLineVM>();
            TimeTableLines.Add(line);
        }

        public void AddTimeTableLine(QuzeDayOfWeek weekDay, TimeSpan beginTime, TimeSpan endTime)
        {
            var line = new TimeTableLineVM(weekDay, beginTime, endTime);
            AddTimeTableLine(line);
        }


    }
}
