using AutoMapper;
using Quze.API.ViewModels;
using Quze.DAL.Stores;
using Quze.Models.Entities;

namespace Quze.API.Controllers
{
    public class RequiredDocumentsController : ControllerBase<RequiredDocument, RequiredDocumentVM,RequiredDocumentStore>
    {
        public RequiredDocumentsController(IMapper mapper, RequiredDocumentStore store) : base(mapper, store)
        {

        }
    }


}