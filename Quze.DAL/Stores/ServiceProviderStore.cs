using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quze.Models.Entities;

namespace Quze.DAL.Stores
{
    public class ServiceProviderStore : StoreBase<ServiceProvider>
    {

        public ServiceProviderStore(QuzeContext ctx) : base(ctx)
        {

        }

       

        public override Task<int> CreateAsync(List<ServiceProvider> entities)
        {
            return base.CreateAsync(entities);
        }

        public async override Task<int> CreateAsync(ServiceProvider entity)
        {
            //var org = ctx.Organizations.FirstOrDefault(o => o.Id == entity.Id);
            entity.Organization = null;
            return await base.CreateAsync(entity);
        }

        public void DeleteSP(ServiceProvider entity)
        {
            entity.Organization = null;
            var Deleted = entity;
            Deleted.IsDeleted = true;
            ctx.Entry(entity).CurrentValues.SetValues(Deleted);
            ctx.Entry(entity).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public ServiceProvider GetServiceProviderById(int ServiceProviderId, int organizationId)
        {
            ServiceProvider ServiceProvider = new ServiceProvider();
            var ServiceProviderQuery = ctx.ServiceProviders.Where(s => s.id == ServiceProviderId&&s.OrganizationId== organizationId);
            ServiceProvider= ServiceProviderQuery.FirstOrDefault();
            return ServiceProvider;
        }

        public Appointment updateServed(int ApointmentId)
        {
            var updateServed = ctx.Appointments.Where(s => s.Id == ApointmentId).FirstOrDefault();
            if (updateServed != null)
            {
                updateServed.Served = true;
                ctx.SaveChanges();
                return updateServed;
            }
            return null;
        }

    }
}
