using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class AppointmentDocumentsController : ControllerBase<AppointmentDocument, AppointmentDocumentVM,AppointmentDocumentStore>
    {
        public AppointmentDocumentsController(IMapper mapper, AppointmentDocumentStore store) : base(mapper, store)
        {

        }
    }


}