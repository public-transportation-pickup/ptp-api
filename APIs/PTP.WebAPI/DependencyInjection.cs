using System.Collections.Immutable;
using PTP.Infrastructure;
namespace PTP.WebAPI;
public static class DependencyInjection
{
    public static WebApplicationBuilder AddWebAPIServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AppSettings>(x =>
        {
            x.DbConnection = builder.Configuration.GetConnectionString("DefaultConnection")!;
        });
        var configuration = builder.Configuration.Get<AppSettings>();
        builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection")!);
        return builder;
    }
}