using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Quze.Models.Entities;

namespace Quze.DAL.Stores
{
    public class BranchStore : StoreBase<Branch>
    {

        public BranchStore(QuzeContext ctx):base(ctx)
        {
                
        }

      

        public IEnumerable<Branch> GetBranchesByOrganizationId(int organizationId)
        {
            //var organizationBranches = Cache.Data.Where(b => b.OrganizationId == organizationId);   
            var organizationBranches = ctx.Branches.Where(b => b.OrganizationId == organizationId)
                .Include(b=>b.Street)
                .ThenInclude(s=>s.City);   
            return organizationBranches;
        }


    }
}
