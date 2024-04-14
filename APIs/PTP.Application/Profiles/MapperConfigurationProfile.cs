using AutoMapper;
using MongoDB.Bson;
using PTP.Application.Features.Routes.Commands;
using PTP.Application.ViewModels;
using PTP.Application.ViewModels.Categories;
using PTP.Application.ViewModels.Menus;
using PTP.Application.ViewModels.MongoDbs.Carts;
using PTP.Application.ViewModels.MongoDbs.Notifications;
using PTP.Application.ViewModels.OrderDetails;
using PTP.Application.ViewModels.Orders;
using PTP.Application.ViewModels.ProductMenus;
using PTP.Application.ViewModels.Products;
using PTP.Application.ViewModels.Routes;
using PTP.Application.ViewModels.RouteVars;
using PTP.Application.ViewModels.Stations;
using PTP.Application.ViewModels.Stores;
using PTP.Application.ViewModels.Timetables;
using PTP.Application.ViewModels.Transactions;
using PTP.Application.ViewModels.Trips;
using PTP.Application.ViewModels.Users;
using PTP.Application.ViewModels.WalletLogs;
using PTP.Application.ViewModels.Wallets;
using PTP.Domain.Entities;
using PTP.Domain.Entities.MongoDbs;
using ZstdSharp.Unsafe;

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
			.ForMember(x => x.StationIds, opt => opt.MapFrom(x => x.Stations.Select(x => x.Id)))
			.ForMember(x => x.StationName, opt => opt.MapFrom(x => x.Stations.Select(x => x.Name)))
			// .ForMember(x => x.WalletAmount, opt => opt.MapFrom(x => x.Wallet!.Amount))
			.ReverseMap();
		CreateMap<Store, StoreCreateModel>()
			.ForMember(x => x.File, opt => opt.Ignore())
			.ReverseMap();
		CreateMap<Store, StoreUpdateModel>()
			.ForMember(x => x.File, opt => opt.Ignore())
			.ReverseMap()
			.ForMember(x => x.ImageName, opt => opt.Ignore());
		#endregion

		#region RouteVariation Mapper
		CreateMap<RouteVar, RouteVarViewModel>().ReverseMap();
		CreateMap<RouteVar, RouteVarCreateModel>().ReverseMap()
			.ForMember(x => x.RouteStations, cfg => cfg.Ignore());
		CreateMap<RouteVar, RouteVar>();
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
			.ForMember(x => x.ProductInMenus, opt => opt.Ignore())
			.ReverseMap()
			.ForMember(x => x.ProductInMenus, opt => opt.Ignore());
		#endregion

		#region Category Mapper
		CreateMap<Category, CategoryViewModel>().ReverseMap();
		CreateMap<Category, CategoryCreateModel>()
		.ForMember(x => x.Image, opt => opt.Ignore())
		.ReverseMap();
		CreateMap<Category, CategoryUpdateModel>()
		.ForMember(x => x.Image, opt => opt.Ignore())
		.ReverseMap()
		.ForMember(x => x.ImageName, opt => opt.Ignore());


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
		CreateMap<Wallet, WalletViewModel>()
			.ForMember(x => x.WalletLogs, opt => opt.MapFrom(x => x.WalletLogs))
			.ForMember(x => x.Transactions, opt => opt.Ignore())
			.ReverseMap();
		#endregion

		#region Order
		CreateMap<Order, OrderViewModel>()
			.ForMember(x => x.StationAddress, opt => opt.MapFrom(x => x.Station.Address))
			.ForMember(x => x.CreationDate, opt => opt.MapFrom(x => x.CreationDate))
			.ForMember(x => x.ModificationDate, opt => opt.MapFrom(x => x.ModificationDate))
			.ForMember(x => x.StationName, opt => opt.MapFrom(x => x.Station.Name))
			.ForMember(x => x.StorePhoneNumber, opt => opt.MapFrom(x => x.Store.PhoneNumber))
			.ForMember(x => x.StoreName, opt => opt.MapFrom(x => x.Store.Name))
			.ForMember(x => x.PaymentType, opt => opt.MapFrom(x => x.Payment.PaymentType))
			.ForMember(x => x.PaymentStatus, opt => opt.MapFrom(x => x.Payment.Status))
			.ForMember(x => x.StoreAddress, opt => opt.MapFrom(x => $"{x.Store.AddressNo}, {x.Store.Street}, {x.Store.Ward}, {x.Store.Zone}"))
			.ReverseMap();
		CreateMap<Order, OrderUpdateModel>().ReverseMap();

		CreateMap<Order, OrderCreateModel>()
			.ForMember(x => x.Payment, opt => opt.Ignore())
			.ForMember(x => x.OrderDetails, opt => opt.Ignore())
			.ReverseMap()
			.ForMember(x => x.Payment, opt => opt.Ignore())
			.ForMember(x => x.OrderDetails, opt => opt.Ignore());
		#endregion

		#region OrderDetail
		CreateMap<OrderDetail, OrderDetailViewModel>()
			.ForMember(x => x.MenuId, opt => opt.MapFrom(x => x.ProductInMenu.MenuId))
			.ForMember(x => x.ProductId, opt => opt.MapFrom(x => x.ProductInMenu.ProductId))
			.ForMember(x => x.ProductName, opt => opt.MapFrom(x => x.ProductInMenu.Product.Name))
			.ForMember(x => x.ProductPrice, opt => opt.MapFrom(x => x.ProductInMenu.Product.Price))
			.ForMember(x => x.Description, opt => opt.MapFrom(x => x.ProductInMenu.Product.Description))
			.ForMember(x => x.ImageURL, opt => opt.MapFrom(x => x.ProductInMenu.Product.ImageURL))
			.ReverseMap();

		CreateMap<OrderDetail, OrderDetailCreateModel>().ReverseMap();
		#endregion

		#region Transactions
		CreateMap<Transaction, TransactionViewModel>()
		.ForMember(x => x.Name, opt => opt.MapFrom(x => x.Payment!.Order.Name))
		.ForMember(x => x.OrderId, opt => opt.MapFrom(x => x.Payment!.OrderId))
		.ForMember(x => x.PhoneNumber, opt => opt.MapFrom(x => x.Payment!.Order.PhoneNumber))
		.ReverseMap();
		#endregion

		#region WalletLogs
		CreateMap<WalletLog, WalletLogViewModel>().ReverseMap();
		#endregion

		#region Station
		CreateMap<Station, StationViewModel>().ReverseMap();
		CreateMap<Station, StationUpdateModel>().ReverseMap();
		CreateMap<Route, RouteCreateModel>().ReverseMap();
		#endregion

		#region Carts
		CreateMap<CartEntity, CartCreateModel>().ReverseMap();
		CreateMap<CartEntity, CartViewModel>().ReverseMap()
			.ForMember(x => x.Id, cfg => cfg.MapFrom(x => ObjectId.Parse(x.Id)))
			.ForMember(x => x.Items, cfg => cfg.MapFrom(x => x.Items));
		CreateMap<CartEntity, CartUpdateModel>().ReverseMap()
			.ForMember(x => x.Id, cfg => cfg.MapFrom(x => ObjectId.Parse(x.Id)))
			.ForMember(x => x.Items, cfg => cfg.MapFrom(x => x.Items));
		CreateMap<CartItemCreateModel, CartItemEntity>().ReverseMap();
		CreateMap<CartItemEntity, CartItemViewModel>().ReverseMap();
		CreateMap<CartItemEntity, CartItemUpdateModel>().ReverseMap();
		#endregion
		#region Notifications
		CreateMap<NotificationEntity, NotificationViewModel>()
			.ReverseMap()
			.ForMember(x => x.Id, cfg => cfg.MapFrom(x => ObjectId.Parse(x.Id)));
		CreateMap<NotificationEntity, NotificationCreateModel>()
			.ReverseMap()
			.ForMember(x => x.Source, cfg => cfg.MapFrom(x => (NotificationSourceEnum)x.Source));
		CreateMap<NotificationEntity, NotificationUpdateModel>()
			.ReverseMap()
			.ForMember(x => x.Id, cfg => cfg.MapFrom(x => ObjectId.Parse(x.Id)));
		#endregion
	}
}