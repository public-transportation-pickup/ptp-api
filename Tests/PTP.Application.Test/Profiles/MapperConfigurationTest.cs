using AutoFixture;
using FluentAssertions;
using PTP.Application.ViewModels.Categories;
using PTP.Application.ViewModels.Routes;
using PTP.Domain.Entities;
using PTP.Domain.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Application.Test.Profiles
{
    public class MapperConfigurationTest:SetupTest
    {
        [Fact]
        public void TestMapper()
        {
            //arrange
            var cate = _fixture.Build<Category>().Without(x=>x.Products).Create();

            //act
            var result = _mapperConfig.Map<CategoryViewModel>(cate);

            //assert
            result.Id.Should().Be(cate.Id.ToString());
        }
    }
}
