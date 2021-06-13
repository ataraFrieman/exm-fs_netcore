using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Models.Entities
{
    public class DataMiningTable
    {
        public DataMiningTable()
        {

        }
        public DataMiningTable(object[] arrayData)
        {
            if (arrayData != null && arrayData.Length >= 11)
            {
                ID = !Convert.IsDBNull(arrayData[0]) ? (int)arrayData[0]:0;
                serviceProviderAverageStartingTimeOfWork = !Convert.IsDBNull(arrayData[1]) ? (int)arrayData[1] : 0;
                serviceProviderAverageEndTimeOfWork = !Convert.IsDBNull(arrayData[2]) ? (int)arrayData[2] : 0;
                depDepartmentPercNoService = !Convert.IsDBNull(arrayData[3]) ? (int)arrayData[3] : 0;
                serviceProviderPercNoService = !Convert.IsDBNull(arrayData[4]) ? (int)arrayData[4] : 0;
                serviceProviderAssistantPercNoService = !Convert.IsDBNull(arrayData[5]) ? (int)arrayData[5] : 0;
                RoomPercNoService = !Convert.IsDBNull(arrayData[6]) ? (int)arrayData[6] : 0;
                serviceProviderId = !Convert.IsDBNull(arrayData[7]) ? (int)arrayData[7] : 0;
                serviceProviderAssistantId = !Convert.IsDBNull(arrayData[8]) ? (int)arrayData[8] : 0;
                depDepartmentId = !Convert.IsDBNull(arrayData[9]) ? (int)arrayData[9] : 0;
                roomId = !Convert.IsDBNull(arrayData[10]) ? (int)arrayData[10] : 0;
            }

        }
        public int ID { get; set; }
        public int serviceProviderAverageStartingTimeOfWork { get; set; }
        public int serviceProviderAverageEndTimeOfWork { get; set; }
        public int depDepartmentPercNoService { get; set; }
        public int serviceProviderPercNoService { get; set; }
        public int serviceProviderAssistantPercNoService { get; set; }
        public int RoomPercNoService { get; set; }
        public int serviceProviderId { get; set; }
        public int serviceProviderAssistantId { get; set; }
        public int depDepartmentId { get; set; }
        public int roomId { get; set; }
        public int roomPercDur_Result { get; set; }
        public int servicePercDur_Result { get; set; }
        public int servicMProcPercDur_Result { get; set; }
        public int servicSProcPercDur_Result { get; set; }
        public int serviceProviderPercDur_Result { get; set; }
        public int serviceProviderAssistantPercDur_Result { get; set; }
        public int serviceProviderPercNoServiceFC { get; set; }
        public int serviceProviderAssistantPercNoServiceFC { get; set; }
    }


}
