using PTP.Application;
using PTP.Application.Repositories.Interfaces;
using PTP.Domain.Entities;
using PTP.Infrastructure.Repositories;

namespace PTP.Infrastructure;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    public UnitOfWork(AppDbContext dbcontext, IRoleRepository roleRepository, ICategoryRepository categoryRepository,
    IMenuRepository menuRepository, IOrderDetailRepository orderDetailRepository, IOrderRepository orderRepository,
    IPaymentRepository paymentRepository, IProductImageRepository productImageRepository, IProductInMenuRepository productInMenuRepository,
    IProductRepository productRepository, IRouteRepository routeRepository, IRouteStationRepository routeStationRepository,
    IScheduleRepository scheduleRepository, IStationRepository stationRepository, IStoreRepository storeRepository,
    IStoreStationRepository storeStationRepository, ITransactionRepository transactionRepository, ITripRepository tripRepository,
    IUserRepository userRepository, IWalletRepository walletRepository, IWalletLogRepository walletLogRepository,
    IRouteVarRepository routeVarRepository, ITimeTableRepository timeTableRepository)
    {
        _dbContext = dbcontext;
        RouteVarRepository = routeVarRepository;
        TimeTableRepository = timeTableRepository;
        CategoryRepository = categoryRepository;
        MenuRepository = menuRepository;
        OrderDetailRepository = orderDetailRepository;
        OrderRepository = orderRepository;
        PaymentRepository = paymentRepository;
        ProductImageRepository = productImageRepository;
        ProductInMenuRepository = productInMenuRepository;
        ProductRepository = productRepository;
        RouteRepository = routeRepository;
        RouteStationRepository = routeStationRepository;
        ScheduleRepository = scheduleRepository;
        StationRepository = stationRepository;
        StoreRepository = storeRepository;
        StoreStationRepository = storeStationRepository;
        TransactionRepository = transactionRepository;
        TripRepository = tripRepository;
        UserRepository = userRepository;
        WalletRepository = walletRepository;
        RoleRepository = roleRepository;
        WalletLogRepository = walletLogRepository;
    }
    public IRoleRepository RoleRepository { get; }

    public ICategoryRepository CategoryRepository { get; }

    public IMenuRepository MenuRepository { get; }

    public IOrderDetailRepository OrderDetailRepository { get; }

    public IOrderRepository OrderRepository { get; }

    public IPaymentRepository PaymentRepository { get; }

    public IProductRepository ProductRepository { get; }

    public IProductImageRepository ProductImageRepository { get; }

    public IProductInMenuRepository ProductInMenuRepository { get; }

    public IRouteRepository RouteRepository { get; }

    public IRouteStationRepository RouteStationRepository { get; }

    public IScheduleRepository ScheduleRepository { get; }

    public IStationRepository StationRepository { get; }

    public IStoreRepository StoreRepository { get; }

    public IStoreStationRepository StoreStationRepository { get; }

    public ITransactionRepository TransactionRepository { get; }

    public ITripRepository TripRepository { get; }

    public IUserRepository UserRepository { get; }

    public IWalletRepository WalletRepository { get; }
    public IWalletLogRepository WalletLogRepository { get; }

    public IRouteVarRepository RouteVarRepository { get; }

    public ITimeTableRepository TimeTableRepository { get; }

    public async Task<bool> SaveChangesAsync() => (await _dbContext.SaveChangesAsync()) > 0;



}