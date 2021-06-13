using Quze.Models.Entities;

namespace Quze.DAL.Stores
{
    public class AppointmentTaskStore : StoreBase<AppointmentTask>
    {

        public AppointmentTaskStore(QuzeContext ctx):base(ctx)
        {
                
        }

    
    }
}
