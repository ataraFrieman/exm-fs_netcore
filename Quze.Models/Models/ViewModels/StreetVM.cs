namespace Quze.Models.Models.ViewModels
{
   public class StreetVM : BaseVM
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public int StreetSymbol { get; set; }
        public virtual CityVM City { get; set; }
    }
}
