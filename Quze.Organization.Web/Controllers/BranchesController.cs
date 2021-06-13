using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Quze.Models.Entities;
//using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Quze.DAL;
using Quze.Organization.Web.ViewModels;
using Quze.DAL.Stores;
using Quze.Models;

namespace Quze.Organization.Web.Controllers
{
    [Route("api/[controller]")]
    public class BranchesController : BaseController
    {

        protected readonly BranchStore branchStore;


        private Quze.Models.Response<BranchVM> response;

        public BranchesController(QuzeContext ctx,IMapper mapper, BranchStore branchStore) :base(ctx,mapper)
        {
            this.branchStore = branchStore;
        }

        [HttpGet("[action]")]
        public Response<BranchVM> GetBranches()
        {

            response = new Response<BranchVM>();
            var OrganizationId = JwtUser.OrganizationId;
            var Branches = branchStore.GetBranchesByOrganizationId(OrganizationId ?? 0).ToList();
            var BranchVMList = mapper.Map<List<BranchVM>>(Branches);

            response.Entities = BranchVMList;
            return response;
        }

        [HttpPut]
        public Branch UpdateBranch([FromBody]Branch branch)
        {
            var oldBranch = context.Branches
                .Where(b => b.Id == branch.Id)
                .FirstOrDefault();

            if (oldBranch != null)
            {
                context.Entry(oldBranch).CurrentValues.SetValues(branch);
                context.SaveChanges();
                // TO DO 
            }
            //var BranchesVM = mapper.Map<List<BranchVM>>(organization.Branches);
            return oldBranch;

        }

        [HttpPost]
        public int Create([FromBody]Branch branch)
        {
            context.Branches.AddRange(branch);
            context.Entry(branch.Street.City).State = EntityState.Unchanged;
            context.Entry(branch.Street).State = EntityState.Unchanged;
            context.SaveChanges();

            return branch.Id;

        }

        [HttpDelete]
        public Branch Delete([FromBody]int branchId)
        {
            var origionalBranch = context.Branches.FirstOrDefault(x => x.Id == branchId);
            if (origionalBranch == null)
            {
                //TO DO ERROR
                return origionalBranch;
            }

            else
            {
                var Deleted = origionalBranch; 
                Deleted.IsDeleted = true;
                context.Entry(origionalBranch).CurrentValues.SetValues(Deleted);
                context.SaveChanges();
                return Deleted;
            }
            
        }

        [HttpGet("[action]")]
        public IActionResult GetServiceType(int branchId)
        {
                var BranchesServiceTypes = context.Branches
                    .Include(q => q.ServiceProvidersBranches)
                    .ThenInclude(q => q.ServiceProvider).
                    ThenInclude(q => q.ServiceProvidersServiceTypes).
                    ThenInclude(q => q.ServiceType).
                     ThenInclude(q => q.MinimalKitRules)
                    .Where(q => q.Id == 1)
                   ;


            HashSet<ServiceType> ServiceTypes = new HashSet<ServiceType>();
            foreach (var item in BranchesServiceTypes.ToList())
            {
                foreach (var spb in item.ServiceProvidersBranches.ToList())
                {
                    foreach (var spst in spb.ServiceProvider.ServiceProvidersServiceTypes.ToList())
                    {
                        spst.ServiceType.ServiceProvidersServiceTypes = null;
                        ServiceTypes.Add(spst.ServiceType);
                    }
                }

            }


            return Ok(ServiceTypes);

        }


    }
}
