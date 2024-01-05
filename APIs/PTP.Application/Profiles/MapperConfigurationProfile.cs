using AutoMapper;
using PTP.Application.ViewModels;
using PTP.Domain.Entities;

namespace PTP.Application.Profiles;
public class MapperConfigurationProfile : Profile 
{
    public MapperConfigurationProfile()
    {
        CreateMap<Role, RoleViewModel>().ReverseMap();
    }
}