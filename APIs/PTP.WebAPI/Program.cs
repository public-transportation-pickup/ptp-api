using Hangfire;
using Microsoft.EntityFrameworkCore;
using PTP.Application.GlobalExceptionHandling;
using PTP.Application.IntergrationServices.Interfaces;
using PTP.Infrastructure;
using PTP.WebAPI;

var builder = WebApplication.CreateBuilder(args);



builder.AddWebAPIServices();



var app = builder.Build();
app.UseHangfireDashboard("/hangfire", new DashboardOptions { IgnoreAntiforgeryToken = true }, null);
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