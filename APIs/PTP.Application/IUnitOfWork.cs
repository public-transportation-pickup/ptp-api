using PTP.Application.Repositories.Interfaces;

namespace PTP.Application;
public interface IUnitOfWork
{
    #region Properties
    ICategoryRepository CategoryRepository { get; }
    IMenuRepository MenuRepository { get; }
    IOrderDetailRepository OrderDetailRepository { get; }
    IOrderRepository OrderRepository { get; }
    IPaymentRepository PaymentRepository { get; }
    IProductRepository ProductRepository { get; }
    IProductImageRepository ProductImageRepository { get; }
    IProductInMenuRepository ProductInMenuRepository { get; }
    IRoleRepository RoleRepository { get; }
    IRouteRepository RouteRepository { get; }
    IRouteStationRepository RouteStationRepository { get; }
    IScheduleRepository ScheduleRepository { get; }
    IStationRepository StationRepository { get; }
    IStoreRepository StoreRepository { get; }
    
    ITransactionRepository TransactionRepository { get; }
    ITripRepository TripRepository { get; }
    IUserRepository UserRepository { get; }
    IWalletRepository WalletRepository { get; }
    IWalletLogRepository WalletLogRepository { get; }
    IRouteVarRepository RouteVarRepository { get; }
    ITimeTableRepository TimeTableRepository { get; }
    #endregion

    Task<bool> SaveChangesAsync();
}