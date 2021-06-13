using Quze.Models.Entities;

namespace Quze.DAL.Stores
{
    public class StreetStore : StoreBase<Street>
    {

        public StreetStore(QuzeContext ctx):base(ctx)
        {
                
        }

    
    }
}
