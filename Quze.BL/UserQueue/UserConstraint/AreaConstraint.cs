using Quze.BL.Utiles;

namespace Quze.BL.UserQueue.UserConstraint
{
  public  class AreaConstraint 
    {
        public AreaConstraint(Address address, int maxDistanceInKm)
        {
            Address = address;
            MaxDistanceInKm = maxDistanceInKm;
        }

        public Address Address { get; }
        public int MaxDistanceInKm { get; }

    }
}
