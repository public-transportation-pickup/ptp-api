using PTP.Application.ViewModels.Users;

namespace PTP.Application.Services.Interfaces;
public interface IAuthService
{
    Task<LoginResponseModel> LoginAsync(string token, string role);
}