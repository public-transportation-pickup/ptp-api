using AutoMapper;
using PTP.Application.ViewModels;
using PTP.Application.ViewModels.Routes;
using PTP.Domain.Entities;

namespace PTP.Application.Profiles;
public class MapperConfigurationProfile : Profile 
{
    public MapperConfigurationProfile()
    {
        CreateMap<Role, RoleViewModel>().ReverseMap();

        #region RouteMapper
        CreateMap<Route, RouteViewModel>().ReverseMap();
        #endregion
    }
}