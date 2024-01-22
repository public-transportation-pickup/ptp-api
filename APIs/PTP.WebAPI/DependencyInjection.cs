using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PTP.Application;
using PTP.Application.GlobalExceptionHandling;
using PTP.Infrastructure;
using Scrutor;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace PTP.WebAPI;
public static class DependencyInjection
{
	public static WebApplicationBuilder AddWebAPIServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddHttpContextAccessor();
		builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
		builder.Services.AddHttpClient();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddControllers();
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
		builder.Services.AddSwaggerGen(opt =>
					{
						opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Public-Transportation-Pickup", Version = "v1" });
						var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
						var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
						opt.IncludeXmlComments(xmlPath);
						opt.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
						{
							Name = "Authorization",
							Description = "Bearer Generated JWT-Token",
							In = ParameterLocation.Header,
							Type = SecuritySchemeType.ApiKey,
							Scheme = "Bearer"

						});
						opt.AddSecurityRequirement(new OpenApiSecurityRequirement
						{
							{
								new OpenApiSecurityScheme
								{
									Reference = new OpenApiReference
									{
										Type = ReferenceType.SecurityScheme,
										Id = JwtBearerDefaults.AuthenticationScheme
									},
									Scheme = "oauth2",
									Name = "Bearer",
									In = ParameterLocation.Header,
								}, Array.Empty<string>()
							}
						});
					});

		var key = Encoding.ASCII.GetBytes(configuration.JWTOptions.Secret);
		builder.Services.AddAuthentication(x =>
		{
			x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(x =>
		{
			x.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = true,
				ValidIssuer = configuration.JWTOptions.Issuer,
				ValidAudience = configuration.JWTOptions.Audience,
				ValidateAudience = true
			};
		});
		return builder;
	}
}