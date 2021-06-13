//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using Quze.BL.UserQueue.UserConstraint;
//using Quze.DAL;
//using Quze.DAL.Stores;
//using Quze.Models.Entities;

//namespace Quze.BL.ServiceProviderSecretary
//{
//    internal class ServiceQueueManager
//    {
//        private TimeTable timeTable;
//        public ServiceQueueManager(TimeTable timeTable, ServiceQueue serviceQueue)
//        {
//            this.timeTable = timeTable;
//        }

//        //private ServiceQueue GetServiceQueueOfDate(DateTime date)
//        //{
//        //    var sQ = serviceQueues.ServiceQueuesOfSp(serviceProvider).TryGetServiceQueueOfDate(date);
//        //    return sQ ?? CreateNewServiceQueue();
//        //}

//        private IEnumerable<TimeTableLine>  GetTimeTables(DateTime date)
//        {
//            throw new NotImplementedException();
//          //  var result = ttl.FilterByDate(date).FilterByServiceProvider(timeTables, serviceProvider);
//         //  return result;
//        }

      

//        private void getRelevantTimeTables()
//        {
           

//        }


//        private  ServiceQueue CreateNewServiceQueue()
//        {
//            throw new  NotImplementedException();
//        }
       
//    }
//}
