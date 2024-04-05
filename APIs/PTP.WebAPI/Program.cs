using System.Collections.Immutable;
using System.Reflection;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using PTP.Application.GlobalExceptionHandling;
using PTP.Application.IntergrationServices.Interfaces;
using PTP.Infrastructure;
using PTP.WebAPI;
using Serilog;
using Serilog.Exceptions;
using FluentValidation;
using Serilog.Sinks.Elasticsearch;
using WebAPI.Middlewares;
using PTP.Application.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);



builder.AddWebAPIServices();

//Log.Logger = new LoggerConfiguration()
//	.ReadFrom.Configuration(builder.Configuration)
//	.CreateLogger();




var app = builder.Build();
app.UseCors();
app.UseHangfireDashboard("/hangfire", new DashboardOptions { IgnoreAntiforgeryToken = true, Authorization = new[] { new DashboardAuthorizationFilter() } }, null);
RecurringJob.AddOrUpdate<IBusRouteService>("check-routes", interService => interService.CheckNewCreatedRoute(), Cron.Monthly());
RecurringJob.AddOrUpdate<IProductService>("Update-product-quantity", interService => interService.UpdateProduct(), Cron.Daily());
// Configure the HTTP request pipeline.
app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseMiddleware<PerformanceMiddleware>();
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


ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string enviroment)
{
	return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]!))
	{
		AutoRegisterTemplate = true,
		IndexFormat = $"{Assembly.GetExecutingAssembly().GetName()?.Name?.ToLower().Replace(".", "-")}-{enviroment.ToLower()}-{DateTime.UtcNow:yyyy-MM}"
	};
}