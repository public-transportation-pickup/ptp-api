using AutoFixture;
using FluentAssertions;
using Moq;
using PTP.Application;
using PTP.Domain.Entities;
using PTP.Domain.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Infrastructure.Test
{
    public class UnitOfWorkTests:SetupTest
    {
        private readonly IUnitOfWork _unitOfWork;
        public UnitOfWorkTests()
        {
            _unitOfWork = new UnitOfWork(
                _dbContext,
                _roleRepositoryMock.Object,
                _cateRepositoryMock.Object,
                _menuRepositoryMock.Object,
                _orderDetailRepositoryMock.Object,
                _orderRepositoryMock.Object,
                _paymentRepositoryMock.Object,
                _productInMenuRepositoryMock.Object,
                _productRepositoryMock.Object,
                _routeRepositoryMock.Object,
                _routeStationRepositoryMock.Object,
                _scheduleRepositoryMock.Object,
                _stationRepositoryMock.Object,
                _storeRepositoryMock.Object,
                _transactionRepositoryMock.Object,
                _tripRepositoryMock.Object,
                _userRepositoryMock.Object,
                _walletRepositoryMock.Object,
                _walletLogRepositoryMock.Object,
                _routeVarRepositoryMock.Object,
                _timeTableRepositoryMock.Object,
                _mapperMock.Object,
                _connectionConfigurationMock.Object
                );
        }

        [Fact]
        public async Task TestUnitOfWork()
        {
            // arrange
            var mockData = _fixture.Build<Role>().Without(x=>x.Users).CreateMany(10).ToList();

            _roleRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(mockData);

            // act
            var items = await _unitOfWork.RoleRepository.GetAllAsync();

            // assert
            items.Should().BeEquivalentTo(mockData);
        }
    }
}
