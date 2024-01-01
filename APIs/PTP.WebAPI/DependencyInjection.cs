using PTP.Infrastructure;
using Scrutor;

namespace PTP.WebAPI;
public static class DependencyInjection
{
	public static WebApplicationBuilder AddWebAPIServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddHttpContextAccessor();
		builder.Services.AddControllers();
		builder.Services.AddRouting(x =>
		{
			x.LowercaseQueryStrings = true;
			x.LowercaseUrls = true;
		});
		var configuration = builder.Configuration.Get<AppSettings>() ?? throw new Exception("Null configuration");
		builder.Services.AddSingleton(configuration);
		builder.Services.AddInfrastructureServices(configuration.ConnectionStrings.DefaultConnection);


		builder.Services.Scan(scan => scan
		 .FromAssemblies(PTP.Infrastructure.AssemblyReference.Assembly,
		 PTP.Application.AssemblyReference.Assembly,
		 AssemblyReference.Assembly)
		 .AddClasses()
		 .UsingRegistrationStrategy(RegistrationStrategy.Skip)
		 .AsMatchingInterface()
		 .WithScopedLifetime());

		return builder;
	}
}