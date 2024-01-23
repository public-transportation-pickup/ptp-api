using AutoMapper;
using PTP.Application.ViewModels;
using PTP.Application.ViewModels.Categories;
using PTP.Application.ViewModels.Menus;
using PTP.Application.ViewModels.OrderDetails;
using PTP.Application.ViewModels.Orders;
using PTP.Application.ViewModels.ProductMenus;
using PTP.Application.ViewModels.Products;
using PTP.Application.ViewModels.Routes;
using PTP.Application.ViewModels.RouteVars;
using PTP.Application.ViewModels.Stores;
using PTP.Application.ViewModels.Timetables;
using PTP.Application.ViewModels.Trips;
using PTP.Application.ViewModels.Users;
using PTP.Application.ViewModels.Wallets;
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
		CreateMap<Store, StoreViewModel>()
			.ForMember(x => x.Email, opt => opt.MapFrom(x => x.User.Email))
			.ForMember(x => x.Password, opt => opt.MapFrom(x => x.User.Password))
            .ForMember(x => x.WalletAmount, opt => opt.MapFrom(x => x.Wallet!.Amount))
            .ReverseMap();
		CreateMap<Store, StoreCreateModel>()
			.ForMember(x => x.ClosedTime, opt => opt.Ignore())
			.ForMember(x => x.OpenedTime, opt => opt.Ignore())
			.ForMember(x => x.File, opt => opt.Ignore())
			.ReverseMap();
		CreateMap<Store, StoreUpdateModel>()
			.ForMember(x => x.ClosedTime, opt => opt.Ignore())
			.ForMember(x => x.OpenedTime, opt => opt.Ignore())
			.ForMember(x => x.File, opt => opt.Ignore())
			.ReverseMap()
			.ForMember(x => x.ImageName, opt => opt.Ignore());
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
		CreateMap<User, UserViewModel>()
		.ForMember(
		x => x.RoleName,
		opt => opt.MapFrom(x => x.Role.Name)
		)
		.ReverseMap();
		CreateMap<User, UserCreateModel>().ReverseMap();
		CreateMap<User, UserUpdateModel>()
			.ForMember(x => x.RoleName, opt => opt.Ignore())
			.ReverseMap();
		#endregion
		#region RouteVariation Mapper
		CreateMap<RouteVar, RouteVarViewModel>().ReverseMap();
		#endregion

		#region Menu Mapper
		CreateMap<Menu, MenuCreateModel>()
			.ForMember(x => x.StartTime, opt => opt.Ignore())
			.ForMember(x => x.EndTime, opt => opt.Ignore())
			.ReverseMap();
		CreateMap<Menu, MenuUpdateModel>()
			.ForMember(x => x.StartTime, opt => opt.Ignore())
			.ForMember(x => x.EndTime, opt => opt.Ignore())
			.ReverseMap();
		CreateMap<Menu, MenuViewModel>()
			.ForMember(x => x.Store, opt => opt.MapFrom(x => x.Store))
			.ReverseMap();
		#endregion

		#region Category Mapper
		CreateMap<Category, CategoryViewModel>().ReverseMap();
		CreateMap<Category, CategoryCreateModel>().ReverseMap();
		CreateMap<Category, CategoryUpdateModel>().ReverseMap();

		#endregion


		#region Product Mapper
		CreateMap<Product, ProductViewModel>()
		.ForMember(x => x.StoreName, opt => opt.MapFrom(x => x.Store.Name))
		.ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category.Name))
		.ReverseMap();

		CreateMap<Product, ProductCreateModel>()
		.ForMember(x => x.Image, opt => opt.Ignore())
		.ReverseMap();

		CreateMap<Product, ProductUpdateModel>()
		.ForMember(x => x.Image, opt => opt.Ignore())
		.ReverseMap()
		.ForMember(x => x.ImageName, opt => opt.Ignore());

		#endregion

		#region ProductMenu
		CreateMap<ProductInMenu, ProductMenuViewModel>()
		.ForMember(x => x.MenuName, opt => opt.MapFrom(x => x.Menu.Name))
		.ForMember(x => x.MenuDescription, opt => opt.MapFrom(x => x.Menu.Description))
		.ForMember(x => x.ProductName, opt => opt.MapFrom(x => x.Product.Name))
		.ForMember(x => x.ProductDescription, opt => opt.MapFrom(x => x.Product.Description))
		.ForMember(x => x.ImageName, opt => opt.MapFrom(x => x.Product.ImageName))
		.ForMember(x => x.ImageURL, opt => opt.MapFrom(x => x.Product.ImageURL))
		.ForMember(x => x.PreparationTime, opt => opt.MapFrom(x => x.Product.PreparationTime))
		.ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Product.Category.Name))
		.ForMember(x => x.CategoryId, opt => opt.MapFrom(x => x.Product.Category.Id))
		.ForMember(x => x.StoreId, opt => opt.MapFrom(x => x.Product.StoreId))
		.ReverseMap();

		CreateMap<ProductInMenu, ProductMenuCreateModel>()
		.ReverseMap();

		CreateMap<ProductInMenu, ProductMenuUpdateModel>()
		.ReverseMap();
		#endregion

		#region Wallet
		CreateMap<Wallet, WalletViewModel>().ReverseMap();
		#endregion

		#region Order
		CreateMap<Order,OrderViewModel>()
			.ForMember(x=>x.StationAddress,opt=>opt.MapFrom(x=>x.Station.Address))
			.ForMember(x=>x.StationName,opt=>opt.MapFrom(x=>x.Station.Name))
			.ForMember(x=>x.StorePhoneNumber,opt=>opt.MapFrom(x=>x.Store.PhoneNumber))
			.ForMember(x=>x.StoreName,opt=>opt.MapFrom(x=>x.Store.Name))
			.ForMember(x=>x.PaymentType,opt=>opt.MapFrom(x=>x.Payment.PaymentType))
			.ForMember(x=>x.PaymentStatus,opt=>opt.MapFrom(x=>x.Payment.Status))
			.ReverseMap();

		CreateMap<Order, OrderCreateModel>()
			.ForMember(x => x.Payment, opt => opt.Ignore())
			.ForMember(x => x.OrderDetails, opt => opt.Ignore())
			.ReverseMap()
			.ForMember(x => x.Payment, opt => opt.Ignore())
			.ForMember(x => x.OrderDetails, opt => opt.Ignore());
        #endregion

        #region OrderDetail
        CreateMap<OrderDetail,OrderDetailViewModel>()
			.ForMember(x=>x.ProductName,opt=>opt.MapFrom(x=>x.Product.Name))
			.ForMember(x=>x.ProductPrice,opt=>opt.MapFrom(x=>x.Product.Price))
			.ForMember(x=>x.Description,opt=>opt.MapFrom(x=>x.Product.Description))
			.ForMember(x=>x.ImageURL,opt=>opt.MapFrom(x=>x.Product.ImageURL))
			.ReverseMap();

		CreateMap<OrderDetail,OrderDetailCreateModel>().ReverseMap();
		#endregion
	}
}