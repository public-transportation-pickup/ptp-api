using AutoFixture;
using AutoMapper;
using Moq;
using PTP.Application.Repositories.Interfaces;
using PTP.Application.Services.Interfaces;
using PTP.Application;
using PTP.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTP.Application.Profiles;
using Microsoft.EntityFrameworkCore;
using PTP.Application.Data.Configuration;

namespace PTP.Domain.Test
{
    public class SetupTest : IDisposable
    {
        public readonly IMapper _mapperConfig;
        protected readonly Fixture _fixture;
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
        protected readonly Mock<IClaimsService> _claimsServiceMock;
        protected readonly Mock<ICurrentTime> _currentTimeMock;
        protected readonly AppDbContext _dbContext;
        #region IRepo
        protected readonly Mock<ICategoryRepository> _cateRepositoryMock;
        protected readonly Mock<IRoleRepository> _roleRepositoryMock;
        protected readonly Mock<IMenuRepository> _menuRepositoryMock;
        protected readonly Mock<IOrderDetailRepository> _orderDetailRepositoryMock;
        protected readonly Mock<IOrderRepository> _orderRepositoryMock;
        protected readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        protected readonly Mock<IProductInMenuRepository> _productInMenuRepositoryMock;
        protected readonly Mock<IProductRepository> _productRepositoryMock;
        protected readonly Mock<IRouteRepository> _routeRepositoryMock;
        protected readonly Mock<IRouteStationRepository> _routeStationRepositoryMock;
        protected readonly Mock<IScheduleRepository> _scheduleRepositoryMock;
        protected readonly Mock<IStationRepository> _stationRepositoryMock;
        protected readonly Mock<IStoreRepository> _storeRepositoryMock;
        protected readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        protected readonly Mock<ITripRepository> _tripRepositoryMock;
        protected readonly Mock<IUserRepository> _userRepositoryMock;
        protected readonly Mock<IWalletRepository> _walletRepositoryMock;
        protected readonly Mock<IWalletLogRepository> _walletLogRepositoryMock;
        protected readonly Mock<IRouteVarRepository> _routeVarRepositoryMock;
        protected readonly Mock<ITimeTableRepository> _timeTableRepositoryMock;
        protected readonly Mock<IMapper> _mapperMock;
        protected readonly Mock<IConnectionConfiguration> _connectionConfigurationMock;

        #endregion
        public SetupTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperConfigurationProfile());
            });
            _mapperConfig = mappingConfig.CreateMapper();
            _fixture = new Fixture();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _currentTimeMock = new Mock<ICurrentTime>();
            _claimsServiceMock = new Mock<IClaimsService>();

            #region IRepo
            _connectionConfigurationMock = new Mock<IConnectionConfiguration>();
            _mapperMock = new Mock<IMapper>();
            _timeTableRepositoryMock = new Mock<ITimeTableRepository>();
            _routeVarRepositoryMock = new Mock<IRouteVarRepository>();
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _walletLogRepositoryMock = new Mock<IWalletLogRepository>();
            _userRepositoryMock=new Mock<IUserRepository>();
            _tripRepositoryMock = new Mock<ITripRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _storeRepositoryMock = new Mock<IStoreRepository>();
            _stationRepositoryMock = new Mock<IStationRepository>();
            _scheduleRepositoryMock = new Mock<IScheduleRepository>();
            _routeStationRepositoryMock = new Mock<IRouteStationRepository>();
            _routeRepositoryMock = new Mock<IRouteRepository>();
            _productInMenuRepositoryMock = new Mock<IProductInMenuRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _paymentRepositoryMock= new Mock<IPaymentRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _cateRepositoryMock = new Mock<ICategoryRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _menuRepositoryMock = new Mock<IMenuRepository>();
            _orderDetailRepositoryMock = new Mock<IOrderDetailRepository>();

            
            #endregion

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new AppDbContext(options);
            _currentTimeMock.Setup(x => x.GetCurrentTime()).Returns(DateTime.UtcNow);
            _claimsServiceMock.Setup(x => x.GetCurrentUser).Returns(Guid.Empty);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
