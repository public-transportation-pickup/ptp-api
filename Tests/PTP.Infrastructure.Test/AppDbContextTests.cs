using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PTP.Domain.Entities;
using PTP.Domain.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Infrastructure.Test
{
    public class AppDbContextTests:SetupTest
    {
        [Fact]
        public async Task AppDbContext_RoleDbSetShouldReturnCorrectData()
        {

            var mockData = _fixture.Build<Role>()
                .Without(x=>x.Users)
                .CreateMany(10).ToList();
            await _dbContext.Role.AddRangeAsync(mockData);

            await _dbContext.SaveChangesAsync();

            var result = await _dbContext.Role.ToListAsync();
            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task AppDbContext_RoleDbSetShouldReturnEmptyListWhenNotHavingData()
        {
            var result = await _dbContext.Role.ToListAsync();
            result.Should().BeEmpty();
        }
    }
}
