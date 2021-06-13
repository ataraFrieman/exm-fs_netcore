using AutoMapper;
using Quze.API.ViewModels;
using Quze.DAL.Stores;
using Quze.Models.Entities;

namespace Quze.API.Controllers
{
    public class OrganizationsController : ControllerBase<Organization, OrganizationVM,OrganizationStore>
    {
        public OrganizationsController(IMapper mapper, OrganizationStore store) : base(mapper, store)
        {

        }
    }


}