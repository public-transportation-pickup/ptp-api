using Hangfire;
using PTP.Application.GlobalExceptionHandling;
using PTP.Application.IntergrationServices.Interfaces;
using PTP.WebAPI;

var builder = WebApplication.CreateBuilder(args);



builder.AddWebAPIServices();



var app = builder.Build();
// Add Hangfire
app.MapHangfireDashboard("/HangfireDashBoard");
RecurringJob.AddOrUpdate<IBusRouteService>("check-routes", interService => interService.CheckNewCreatedRoute(), Cron.Monthly());
// Configure the HTTP request pipeline.
app.UseMiddleware<GlobalErrorHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
