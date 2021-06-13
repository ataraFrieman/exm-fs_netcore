using Quze.Models.Entities;
using System;
using System.Linq;

namespace Quze.DAL.Stores
{
    public class ServiceQueueStore : StoreBase<ServiceQueue>
    {

        public ServiceQueueStore(QuzeContext ctx):base(ctx)
        {
                
        }
        
        public int AddServiceQ(ServiceQueue serviceQ)
        {
            ctx.ServiceQueues.Add(serviceQ);
            ctx.SaveChanges();
            return serviceQ.Id;
        }

        public ServiceQueue GetServiceQ(int serviceQId)
        {
            return ctx.ServiceQueues.Where(o => o.Id == serviceQId).FirstOrDefault();
        }
        public  ServiceQueue GetServiceQ(DateTime beginTime,int organizationId)
        {
            return ctx.ServiceQueues.Where(o => o.BeginTime.Date == beginTime.Date&&o.OrganizationId==organizationId).FirstOrDefault();
        }
        public  ServiceQueue CreateOpeartionQueue( DateTime beginTime, int organizationId, DateTime? endTime=null)  {
            if (!endTime.HasValue)
                endTime = beginTime.Date.AddHours(16);
            ServiceQueue operationServiceQueue = new ServiceQueue()
            {
                BeginTime = beginTime,
                EndTime = endTime.Value,
                OrganizationId = organizationId,
                ServiceProviderId = 1,//לתקן
                BranchId = 551,//לתקן
                TimeTableId = 520//?
            };
            ctx.ServiceQueues.AddAsync(operationServiceQueue);
            ctx.SaveChanges();
            return operationServiceQueue;
        }



    }
}
