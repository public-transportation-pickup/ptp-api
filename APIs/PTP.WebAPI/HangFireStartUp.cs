using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace PTP.WebAPI;
public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        return true;
    }
}