using AutoMapper;
using Quze.API.ViewModels;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;
using CityVM = Quze.API.ViewModels.CityVM;
using OrganizationVM = Quze.API.ViewModels.OrganizationVM;
using RequiredDocumentVM = Quze.API.ViewModels.RequiredDocumentVM;
using RequiredTaskVM = Quze.API.ViewModels.RequiredTasksVM;
using ServiceProviderVM = Quze.API.ViewModels.ServiceProviderVM;
using ServiceTypeVM = Quze.API.ViewModels.ServiceTypeVM;

namespace Quze.API.Models
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<ServiceType, ServiceTypeVM>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<ServiceProvidersServiceType, ServiceProvidersServiceTypeVM>(MemberList.Source).IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<Fellow, FellowVM>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<Equipment, EquipmentVM>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<ServiceTypeEqp, ServiceTypeEqpVM>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<ServiceQueue, ServiceQueueVm>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<Organization, OrganizationVM>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<ServiceProvider, ServiceProviderVM>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<Appointment, AppointmentVm>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<RequiredDocument, RequiredDocumentVM>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<RequiredTask, RequiredTaskVM>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<BranchVM, Branch>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<CityVM, City>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<AlertRuleVM, AlertRule>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<ExpertyVM, Experty>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<BranchVM, Branch>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap().
                ForAllOtherMembers(x =>
                {
                    if (x.DestinationMember.Name == "Street")
                        x.Ignore();
                });

        }
    }



}


