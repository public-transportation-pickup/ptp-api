using AutoFixture;
using FluentAssertions;
using PTP.Application.Repositories.Interfaces;
using PTP.Domain.Entities;
using PTP.Domain.Test;
using PTP.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Infrastructure.Test.Repositories
{
    public class GenericRepositoyTests:SetupTest
    {
        private readonly IGenericRepository<Role> _genericRepository;

        public GenericRepositoyTests()
        {
            _genericRepository = new GenericRepository<Role>(
                _dbContext,
                _currentTimeMock.Object,
                _claimsServiceMock.Object
                );

        }

        [Fact]
        public async Task GenericRepository_GetAllAsync_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Role>()
                .Without(x => x.Users)
                .CreateMany(10).ToList();
            for (int i = 0; i < 10; i++)
            {
                mockData[i].IsDeleted = false;
            }
            await _dbContext.Role.AddRangeAsync(mockData);

            await _dbContext.SaveChangesAsync();


            var result = await _genericRepository.GetAllAsync();

            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task GenericRepository_GetByIdAsync_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Role>()
                .Without(x => x.Users)
                .Create();
            mockData.IsDeleted = false;

            await _dbContext.Role.AddAsync(mockData);

            await _dbContext.SaveChangesAsync();


            var result = await _genericRepository.GetByIdAsync(mockData.Id);

            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task GenericRepository_AddAsync_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Role>().Without(x=>x.Users).Create();


            await _genericRepository.AddAsync(mockData);
            var result = await _dbContext.SaveChangesAsync();

            result.Should().Be(1);
        }

        [Fact]
        public async Task GenericRepository_AddRangeAsync_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Role>().Without(x=>x.Users).CreateMany(10).ToList();


            await _genericRepository.AddRangeAsync(mockData);
            var result = await _dbContext.SaveChangesAsync();

            result.Should().Be(10);
        }

        [Fact]
        public async Task GenericRepository_SoftRemove_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Role>().Without(x=>x.Users).Create();
            _dbContext.Role.Add(mockData);
            await _dbContext.SaveChangesAsync();


            _genericRepository.SoftRemove(mockData);
            var result = await _dbContext.SaveChangesAsync();

            result.Should().Be(1);
        }

        [Fact]
        public async Task GenericRepository_Update_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Role>().Without(x => x.Users).Create();
            _dbContext.Role.Add(mockData);
            await _dbContext.SaveChangesAsync();


            _genericRepository.Update(mockData);
            var result = await _dbContext.SaveChangesAsync();

            result.Should().Be(1);
        }

        [Fact]
        public async Task GenericRepository_SoftRemoveRange_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Role>().Without(x => x.Users).CreateMany(10).ToList();
            await _dbContext.Role.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();


            _genericRepository.SoftRemoveRange(mockData);
            var result = await _dbContext.SaveChangesAsync();

            result.Should().Be(10);
        }

        [Fact]
        public async Task GenericRepository_UpdateRange_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Role>().Without(x => x.Users).CreateMany(10).ToList();
            await _dbContext.Role.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();


            _genericRepository.UpdateRange(mockData);
            var result = await _dbContext.SaveChangesAsync();

            result.Should().Be(10);
        }

        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnCorrectDataFirstsPage()
        {
            var mockData = _fixture.Build<Role>()
                .Without(x=>x.Users)
                .CreateMany(45).ToList();
            await _dbContext.Role.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();


            var paginasion = await _genericRepository.ToPagination();


            paginasion.Previous.Should().BeFalse();
            paginasion.Next.Should().BeTrue();
            paginasion.Items!.Count.Should().Be(10);
            paginasion.TotalItemsCount.Should().Be(45);
            paginasion.TotalPagesCount.Should().Be(5);
            paginasion.PageIndex.Should().Be(0);
            paginasion.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnCorrectDataSecoundPage()
        {
            var mockData = _fixture.Build<Role>()
                .Without(x => x.Users)
                .CreateMany(45).ToList();
            await _dbContext.Role.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();


            var paginasion = await _genericRepository.ToPagination(1, 20);


            paginasion.Previous.Should().BeTrue();
            paginasion.Next.Should().BeTrue();
            paginasion.Items!.Count.Should().Be(20);
            paginasion.TotalItemsCount.Should().Be(45);
            paginasion.TotalPagesCount.Should().Be(3);
            paginasion.PageIndex.Should().Be(1);
            paginasion.PageSize.Should().Be(20);
        }


        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnCorrectDataLastPage()
        {
            var mockData = _fixture.Build<Role>()
                .Without(x => x.Users)
                .CreateMany(45).ToList();
            await _dbContext.Role.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();


            var paginasion = await _genericRepository.ToPagination(2, 20);


            paginasion.Previous.Should().BeTrue();
            paginasion.Next.Should().BeFalse();
            paginasion.Items!.Count.Should().Be(5);
            paginasion.TotalItemsCount.Should().Be(45);
            paginasion.TotalPagesCount.Should().Be(3);
            paginasion.PageIndex.Should().Be(2);
            paginasion.PageSize.Should().Be(20);
        }

        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnWithoutData()
        {
            var paginasion = await _genericRepository.ToPagination();


            paginasion.Previous.Should().BeFalse();
            paginasion.Next.Should().BeFalse();
            paginasion.Items!.Count.Should().Be(0);
            paginasion.TotalItemsCount.Should().Be(0);
            paginasion.TotalPagesCount.Should().Be(0);
            paginasion.PageIndex.Should().Be(0);
            paginasion.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task GenericRepository_WhereAsync_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Role>()
                .Without(x => x.Users)
                .CreateMany(10).ToList();
            for (int i = 0; i < 10; i++)
            {
                mockData[i].IsDeleted = false;
            }
            await _dbContext.Role.AddRangeAsync(mockData);

            await _dbContext.SaveChangesAsync();


            var result = await _genericRepository.WhereAsync(x => x.Id == mockData[1].Id);

            result.Should().NotBeNull();
        }
        [Fact]
        public async Task GenericRepository_FirstOrDefaultAsync_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Role>()
                .Without(x => x.Users)
                .Create();
            mockData.IsDeleted = false;

            await _dbContext.Role.AddAsync(mockData);

            await _dbContext.SaveChangesAsync();


            var result = await _genericRepository.FirstOrDefaultAsync(x=>x.Id==mockData.Id);

            result.Should().BeEquivalentTo(mockData);
        }

    }
}
