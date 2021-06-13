using Quze.Models.Entities;

namespace Quze.DAL.Stores
{
    public class OrganizationStore : StoreBase<Organization>
    {
        public OrganizationStore(QuzeContext ctx) : base(ctx)
        {

        }

        
    }
}
