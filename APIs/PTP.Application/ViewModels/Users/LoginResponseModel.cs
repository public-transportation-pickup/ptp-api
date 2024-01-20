namespace PTP.Application.ViewModels.Users;
public class LoginResponseModel
{
    public UserViewModel User { get; set; } = default!;
    public string Token { get; set; } = default!;
}