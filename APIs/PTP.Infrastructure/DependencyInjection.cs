using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PTP.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string dbConnection)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(dbConnection));
        return services;
    }
}