using System.Reflection;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using PTP.Application.GlobalExceptionHandling;
using PTP.Application.IntergrationServices.Interfaces;
using PTP.Infrastructure;
using PTP.WebAPI;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);



builder.AddWebAPIServices();

if (!builder.Environment.IsDevelopment())
{
	ConfigureLogging();
	builder.Host.UseSerilog();
}

var app = builder.Build();
app.UseCors();
app.UseHangfireDashboard("/hangfire", new DashboardOptions { IgnoreAntiforgeryToken = true, Authorization = new[] { new DashboardAuthorizationFilter() } }, null);
RecurringJob.AddOrUpdate<IBusRouteService>("check-routes", interService => interService.CheckNewCreatedRoute(), Cron.Monthly());
// Configure the HTTP request pipeline.
app.UseMiddleware<GlobalErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

ApplyMigration();
app.MapControllers();

app.Run();


void ApplyMigration()
{
	using (var scope = app!.Services.CreateScope())
	{
		var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		if (_db.Database.GetPendingMigrations().Count() > 0)
		{

			_db.Database.Migrate();
		}
	}
}

void ConfigureLogging()
{
	var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
	var configuration = new ConfigurationBuilder()
		.AddJsonFile("appsettings.json",
			optional: false,
			reloadOnChange: true)
		.AddJsonFile($"appsettings.{env}.json",
			optional: true)
			.Build();
	Log.Logger = new LoggerConfiguration()
		.Enrich.FromLogContext()
		.Enrich.WithExceptionDetails()
		.WriteTo.Console()
		.WriteTo.Elasticsearch(ConfigureElasticSink(configuration, env ?? throw new Exception()))
		.Enrich.WithProperty("Environment", env)
		.ReadFrom.Configuration(configuration)
		.CreateLogger();
}
ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string enviroment)
{
	return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]!))
	{
		AutoRegisterTemplate = true,
		IndexFormat = $"{Assembly.GetExecutingAssembly().GetName()?.Name?.ToLower().Replace(".", "-")}-{enviroment.ToLower()}-{DateTime.UtcNow:yyyy-MM}"
	};
}