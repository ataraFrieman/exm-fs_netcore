using Quze.Models.Entities;

namespace Quze.DAL.Stores
{
    public class AppointmentDocumentStore : StoreBase<AppointmentDocument>
    {

        public AppointmentDocumentStore(QuzeContext ctx):base(ctx)
        {
                
        }

    
    }
}
