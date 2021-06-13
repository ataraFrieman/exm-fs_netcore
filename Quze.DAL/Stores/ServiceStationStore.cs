using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.DAL.Stores
{
   public class ServiceStationStore :StoreBase<ServiceStation>
    {
        public ServiceStationStore(QuzeContext ctx):base(ctx)
        {

        }

        public ServiceStation GetServiceStationById(int id)
        {
            ServiceStation serviceStation = ctx.ServiceStation.Find(id);
            return serviceStation;
        }
    }
}
