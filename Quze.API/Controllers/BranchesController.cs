using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Quze.DAL.Stores;
using Quze.Models;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class BranchesController : ControllerBase<Branch, BranchVM, BranchStore>
    {
        public BranchesController(IMapper mapper, BranchStore store) : base(mapper, store)
        {
        }

        [HttpGet("[action]/{OrganizationId}")]
        public Response<BranchVM> GetBranchesByOrganization(int organizationId)
        {
            var response = new Response<BranchVM>();

            var branches = store.GetBranchesByOrganizationId(organizationId);
            var branchesVM = mapper.Map<List<BranchVM>>(branches);
            response.Entities = branchesVM;

            return response;
        }

    }
}


