using PTP.Application.Services.Interfaces;
using System.Security.Claims;

namespace PTP.WebAPI.Services;
public class ClaimsService : IClaimsService
{
	public ClaimsService(IHttpContextAccessor httpContextAccessor)
	{
		var Id = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
		GetCurrentUser = string.IsNullOrEmpty(Id) ? Guid.Empty : Guid.Parse(Id);
	}
	public Guid GetCurrentUser { get; }
}