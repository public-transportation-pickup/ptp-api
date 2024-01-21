using Hangfire;
using PTP.Application;
using PTP.Application.GlobalExceptionHandling;
using PTP.Application.IntergrationServices.Interfaces;
using PTP.Infrastructure;
using Scrutor;
using System.Text.Json.Serialization;

namespace PTP.WebAPI;
public static class DependencyInjection
{
	public static WebApplicationBuilder AddWebAPIServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddHttpContextAccessor();
		builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddHttpClient();
		builder.Services.AddRouting(x =>
		{
			x.LowercaseQueryStrings = true;
			x.LowercaseUrls = true;
		});
		var configuration = builder.Configuration.Get<AppSettings>() ?? throw new Exception("Null configuration");
		// DI AppSettings
		builder.Services.AddSingleton(configuration);

		builder.Services.AddInfrastructureServices(configuration.ConnectionStrings.DefaultConnection);
		builder.Services.AddSingleton<GlobalErrorHandlingMiddleware>();
		//Register to connect Redis

		builder.Services.AddStackExchangeRedisCache(redisOptions =>
		{
			redisOptions.Configuration = configuration.ConnectionStrings.RedisConnection;
		});

		// Register To Handle Query/Command of MediatR
		builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
		// Scan and register all interfaces --> implementations 
		builder.Services.Scan(scan => scan
		 .FromAssemblies(PTP.Infrastructure.AssemblyReference.Assembly,
		 PTP.Application.AssemblyReference.Assembly,
		 AssemblyReference.Assembly)
		 .AddClasses()
		 .UsingRegistrationStrategy(RegistrationStrategy.Skip)
		 .AsMatchingInterface()
		 .WithScopedLifetime());
		
		// Add Hangfire
		builder.Services.AddHangfire(config => config
						.UseSimpleAssemblyNameTypeSerializer()
						.UseRecommendedSerializerSettings()
						.UseInMemoryStorage());
		builder.Services.AddHangfireServer();


        return builder;
	}
}