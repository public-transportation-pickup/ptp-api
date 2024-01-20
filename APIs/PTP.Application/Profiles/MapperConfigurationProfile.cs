using AutoMapper;
using PTP.Application.ViewModels;
using PTP.Application.ViewModels.Routes;
using PTP.Application.ViewModels.RouteVars;
using PTP.Application.ViewModels.Stores;
using PTP.Application.ViewModels.Timetables;
using PTP.Application.ViewModels.Trips;
using PTP.Application.ViewModels.Users;
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

        #region StoreMapper
        CreateMap<Store, StoreViewModel>().ReverseMap();
        CreateMap<Store, StoreCreateModel>()
            .ForMember(x => x.ClosedTime, opt => opt.Ignore())
            .ForMember(x => x.OpenedTime, opt => opt.Ignore())
            .ForMember(x => x.File, opt => opt.Ignore())
            .ReverseMap();
        CreateMap<Store, StoreUpdateModel>()
            .ForMember(x => x.ClosedTime, opt => opt.Ignore())
            .ForMember(x => x.OpenedTime, opt => opt.Ignore())
            .ForMember(x => x.File, opt => opt.Ignore())
            .ReverseMap();
        #endregion

        #region RouteVariation Mapper
        CreateMap<RouteVar, RouteVarViewModel>().ReverseMap();
        CreateMap<RouteVar, RouteVarCreateModel>().ReverseMap();
        #endregion
        #region TimeTable
        CreateMap<TimetableViewModel, TimeTable>().ReverseMap();
        CreateMap<TimetableCreateModel, TimeTable>().ReverseMap();
        CreateMap<TimetableUpdateModel, TimeTable>().ReverseMap();
        #endregion
        #region Trip
        CreateMap<Trip, TripCreateModel>().ReverseMap();
        CreateMap<Trip, TripViewModel>().ReverseMap();
        CreateMap<Trip, TripUpdateModel>().ReverseMap();

        #endregion
        #region User
        CreateMap<User, UserViewModel>().ReverseMap();
        #endregion
    }
}