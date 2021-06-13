using Quze.Models.Entities;

namespace Quze.DAL.Stores
{
    public class CityStore : StoreBase<City>
    {
        public CityStore(QuzeContext ctx) : base(ctx)
        {

        }

    }
}
