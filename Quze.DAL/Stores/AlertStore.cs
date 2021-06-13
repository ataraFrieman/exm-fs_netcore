using Quze.Models.Entities;

namespace Quze.DAL.Stores
{
    public class AlertStore : StoreBase<Alert>
    {

        public AlertStore(QuzeContext ctx):base(ctx)
        {
                
        }

    
    }
}
