
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quze.DAL;
using Quze.Models;
using System.Threading.Tasks;
using Quze.DAL.Stores;
using Quze.Models.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Quze.Infrastruture.Types;
using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;

namespace Quze.Organization.Web.Controllers
{
    [Route("api/[controller]")]
    public class RequiredDocumentsController : ControllerBase<RequiredDocument, RequiredDocumentVM, RequiredDocumentStore>
    {
        private Quze.Models.Request<ServiceProviderVM> request;
        private Quze.Models.Response<ServiceProvider> response;
        private readonly RequiredDocumentStore requiredDocuments;


        public RequiredDocumentsController(IMapper mapper, RequiredDocumentStore store) : base(mapper, store)
        {

        }

        [HttpGet("[action]")]
        public Response<RequiredDocumentVM> GetDocumentsByST(int id)
        {
            List<RequiredDocument> list = store.GetDocumentsByST(id);
            var listVM = mapper.Map<List<RequiredDocumentVM>>(list);
            var response = new Response<RequiredDocumentVM> { Entities = listVM };
            return response;
        }

    }
}