using System.Collections.Generic;
using Quze.BL.UserQueue.UserConstraint;
using Quze.Models.Entities;

namespace Quze.BL.Utiles
{
   public class ServiceQAndDateCreteria
    {
        public ServiceQueue ServiceQueue { get; set; }
        public IEnumerable<PossibleTime> PossibleTimes { get; set; }

        public ServiceQAndDateCreteria(IEnumerable<PossibleTime> possibleTimes, ServiceQueue serviceQueue)
        {
            ServiceQueue = serviceQueue;
            PossibleTimes = possibleTimes;
        }

        public override string ToString()
        {
            return $"the serviceQueue is: {ServiceQueue} , possible times are: {PossibleTimes}   ";
        }
    }
}
