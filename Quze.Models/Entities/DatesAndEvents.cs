using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Models.Entities
{

    public class DatesAndEvents
    {
        public DatesAndEvents()
        {

        }
        public DatesAndEvents(object[] dataArray)
        {
            if (dataArray != null && dataArray.Length >= 8)
            {
                gDate = (DateTime)dataArray[0];
                he_IsDayOfRest = (int)dataArray[1];
                he_IsEveOfHoliday = (int)dataArray[2];
                he_isWorkingDay = (int)dataArray[3];
                he_isFast = (int)dataArray[4];
                m_IsDayOfRest = (int)dataArray[5];
                m_IsEveOfHoliday = (int)dataArray[6];
                m_isFast = (int)dataArray[7];
            }

        }
        public DateTime gDate { get; set; }
        public int he_IsDayOfRest { get; set; }
        public int he_IsEveOfHoliday { get; set; }
        public int he_isWorkingDay { get; set; }
        public int he_isFast { get; set; }
        public int m_IsDayOfRest { get; set; }
        public int m_IsEveOfHoliday { get; set; }
        public int m_isFast { get; set; }
    }

}
