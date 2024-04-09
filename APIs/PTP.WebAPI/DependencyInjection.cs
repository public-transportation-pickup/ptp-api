using FluentValidation;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using PTP.Application;
using PTP.Application.GlobalExceptionHandling;
using PTP.Application.Validations;
using PTP.Infrastructure;
using Scrutor;
using StackExchange.Redis;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using WebAPI.Middlewares;
namespace PTP.WebAPI;
public static class DependencyInjection
{

	public static async Task<WebApplicationBuilder> AddWebAPIServicesAsync(this WebApplicationBuilder builder)
	{

		const string serviceName = "PTP_WebAPI";
		const string serviceVersion = "v1.0";
		// Add Tracing
		builder.Services.AddOpenTelemetry()
			.WithTracing(cfg =>
				cfg.AddSource(serviceName)
					.ConfigureResource(resource => resource.AddService(serviceName: serviceName,
						serviceVersion: serviceVersion))
					.AddAspNetCoreInstrumentation()
					.AddSqlClientInstrumentation()
					.AddHttpClientInstrumentation()
					.AddMongoDBInstrumentation()
					.AddRedisInstrumentation()
					.AddOtlpExporter(ex =>
					{
						ex.Endpoint = new("http://jaeger:4317");
						ex.ExportProcessorType = OpenTelemetry.ExportProcessorType.Simple;
						ex.TimeoutMilliseconds = 30;
						ex.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
					})
					.AddConsoleExporter());
		builder.Logging.AddSeq("http://seq:5341");
		builder.Services.AddHttpContextAccessor();
		builder.Services.AddCors(options
		=> options.AddDefaultPolicy(policy
		=> policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

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
		List<Assembly> assemblies = new List<Assembly>
		{
			typeof(Program).Assembly,
			Application.AssemblyReference.Assembly,
			Infrastructure.AssemblyReference.Assembly
		};
		builder.Services.AddSingleton(configuration);
		builder.Services.AddValidatorsFromAssemblies(assemblies: assemblies);
		builder.Services.AddInfrastructureServices(configuration.ConnectionStrings.DefaultConnection);
		builder.Services.AddSingleton<GlobalErrorHandlingMiddleware>();
		//Register to connect Redis
		IConnectionMultiplexer redisConnectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(configuration.ConnectionStrings.RedisConnection);
		builder.Services.AddSingleton(redisConnectionMultiplexer);
		builder.Services.AddStackExchangeRedisCache(options => options.ConnectionMultiplexerFactory = () => Task.FromResult(redisConnectionMultiplexer));
		// Register MongoDb
		var mongoUrl = MongoUrl.Create(configuration.ConnectionStrings.MongoDbConnection);
		var clientSettings = MongoClientSettings.FromUrl(mongoUrl);
		clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
		var mongoClient = new MongoClient(clientSettings);
		builder.Services.AddSingleton(mongoClient.GetDatabase("ptp-db"));
		// Register To Handle Query/Command of MediatR
		builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
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
						.UseInMemoryStorage()
						);
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

		builder.Services.AddSignalR();
		builder.Services.AddSingleton<PerformanceMiddleware>();
		builder.Services.AddSingleton<Stopwatch>();
		return builder;
	}
}