using Microsoft.EntityFrameworkCore;
using Quze.Models.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Quze.DAL.Stores
{
    public class ServiceTypeStore : StoreBase<ServiceType>
    {

        public ServiceTypeStore(QuzeContext ctx) : base(ctx)
        {

        }



        public List<ServiceType> GetSTChildren(int organizationId, int serviceTypeId)
        {
            if (organizationId == 0 || serviceTypeId == 0)
                return null;
            var serviceTypes = ctx.ServiceTypes
                .Where(st => st.OrganizationId == organizationId && st.IsVisibleToOrganization != false && st.ParentServiceId == serviceTypeId)
                .OrderBy(st => st.ParentServiceId)
                .Select(st => new ServiceType
                {
                    Description = st.Description,
                    Id = st.Id,
                    ParentServiceId = st.ParentServiceId,
                }).ToList();
            return serviceTypes;
        }

        public List<ServiceType> GetSTByOrganization(int organizationId)
        {
            if (organizationId == 0)
                return null;
            var serviceTypes = ctx.ServiceTypes
                .Where(st => st.OrganizationId == organizationId&& st.IsVisibleToOrganization != false&& !st.IsDeleted)
                    .OrderBy(st => st.Id)
                .Select(st => new ServiceType
                {
                    Code=st.Code,
                    Description = st.Description,
                    Id = st.Id,
                    ParentServiceId = st.ParentServiceId,
                })
        .ToList();
            return serviceTypes;

        }

        public List<ServiceType> GetSTByServiceProvider(int serviceProviderId)
        {
            if (serviceProviderId == 0)
            {

                return null;
            }
            var serviceTypesQuery = ctx.ServiceProvidersServiceTypes
                .Where(spst => spst.ServiceProviderId == serviceProviderId)
                .Select(spst => spst.ServiceTypeId);
            var serviceTypes = ctx.ServiceTypes
                .Where(st => serviceTypesQuery.Contains(st.Id))
                .OrderBy(st => st.Id)
                .Select(st => new ServiceType
                {
                    Description = st.Description,
                    Id = st.Id,
                    ParentServiceId = st.ParentServiceId,

                }).ToList();
            return serviceTypes;

        }

        public object GetSQST(int sqId)
        {
            throw new NotImplementedException();
        }

        //public List<ServiceType> GetDescendants(int STId)
        //{
        //    var serviceTypes = ctx.ServiceTypes.ToList();
        //    var filteredServiceTypes = new List<ServiceType>();
        //    var parentsServiceTypes = new List<int>();

        //    filteredServiceTypes.AddRange(serviceTypes.Where(st => st.Id == STId).ToList());
        //    parentsServiceTypes = filteredServiceTypes.Select(fst => fst.Id).ToList();
        //    for (var i = 0; i < parentsServiceTypes.Count; i++)
        //    {
        //        filteredServiceTypes.AddRange(serviceTypes.Where(st => st.ParentServiceId != null && st.ParentServiceId == parentsServiceTypes[i]).ToList());
        //        parentsServiceTypes = filteredServiceTypes.Select(fst => fst.Id).ToList();
        //    }
        //    return filteredServiceTypes;

        //}

        // TODO: fix it
        public IEnumerable<ServiceType> GetDescendants(int serviceTypeCategoryID)
        {

            var result = ctx.ServiceTypes.Where(x => x.ParentServiceId == serviceTypeCategoryID || x.Id == serviceTypeCategoryID);
            return result;
        }
        public ServiceType GetServiceTypeOperation(string operationCode)
        {
            try
            {
                var serviceTypeQuery = ctx.ServiceTypes
                    .Where(s => s.Code == operationCode);
                var serviceType= serviceTypeQuery.FirstOrDefault();

                return serviceType;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}
