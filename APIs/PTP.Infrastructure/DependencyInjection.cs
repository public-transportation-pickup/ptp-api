using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PTP.Application.Profiles;

namespace PTP.Infrastructure;
public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string dbConnection)
	{
		services.AddAutoMapper(typeof(MapperConfigurationProfile));
		services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(dbConnection));
		return services;
	}
}