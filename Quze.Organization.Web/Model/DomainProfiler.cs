using AutoMapper;
using Quze.Organization.Web.ViewModels;
using Quze.Models.Entities;
using Quze.DAL.Stores;
using Quze.BL.Operations;
using Quze.Organization.Web.Pages.ViewModels;
//using Quze.Models;

namespace Quze.Organization.Web.Models
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<ServiceType, ServiceTypeVM>();
            CreateMap<OperationsResponse, OperationsResponseVM>();
            CreateMap<ServiceType, ServiceTypeOppVM>();
            CreateMap<ServiceTypeEqp, ServiceTypeEqpVM>();
            CreateMap<Appointment, AppointmentOppVM>();
            CreateMap<Equipment, EquipmentVM>();
            CreateMap<ResponseReadyEqpOperation, ResponseReadyEqpOperationVM>();
            CreateMap<EquipmentAppointmentRequest, EquipmentAppointmentRequestVM>();
            CreateMap<Conflict, ConflictVM>();

            

            CreateMap<Appointment, AppointmentVM>()
                .ForMember(a=>a.ServiceProvider,d=>d.MapFrom(w=>w.ServiceQueue.ServiceProvider));
            CreateMap<Quze.Models.Entities.Organization, OrganizationVM>();
            CreateMap<ServiceProvider, ServiceProviderVM>();
            CreateMap<Branch, Quze.Organization.Web.ViewModels.BranchVM>();
           
            CreateMap<ServiceProvider, ServiceProviderVM>();
            CreateMap<ServiceStation, ServiceStationVM>();
            CreateMap<Fellow, FellowVM>();
            CreateMap<ServiceQueue, ServiceQueueVM>().ReverseMap();
            CreateMap<AlertRule, Quze.Models.Models.ViewModels.AlertRuleVM>().ReverseMap();
            //CreateMap<MinimalKit, MinimalKitVM>();
            CreateMap<UserTask, UserTaskVM>().ReverseMap();
            CreateMap<TimeTable, TimeTableVM>().ReverseMap();
            CreateMap<User, Quze.Models.Models.ViewModels.UserVM>().ReverseMap();
            CreateMap<MinimalKit, MinimalKitVM>();
                      //.ForMember(dest => dest.Docs, doc => doc.MapFrom(src => src.Docs))
                      //.ForMember(dest => dest.Tasks, task => task.MapFrom(src => src.Tasks))
                      //.ForMember(dest => dest.Tests, test => test.MapFrom(src => src.Tests));
            CreateMap<AppointmentDocument, AppointmentDocVM>().ReverseMap();
            CreateMap<AppointmentTask, AppointmentTaskVM>()
                .ForMember(dest => dest.RequiredTask, tar => tar.MapFrom(src => src.RequiredTask));
            CreateMap<AppointmentTest, AppointmentTestVM>();
            CreateMap<RequiredTask, Quze.Models.Models.ViewModels.RequiredTaskVM>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<RequiredDocument, Quze.Models.Models.ViewModels.RequiredDocumentVM>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            //need to add using Quze.Models.Models.ViewModels
            CreateMap<RequiredTest, Quze.Models.Models.ViewModels.RequiredTestVM>();
            CreateMap<Equipment, Quze.Models.Models.ViewModels.EquipmentVM>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<ServiceTypeEqp, Quze.Models.Models.ViewModels.ServiceTypeEqpVM>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            
        }
    }
}
