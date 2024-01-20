namespace PTP.Application.ViewModels.Users;
public class UserUpdateModel
{
    public string FullName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; } = default!;
    public string? RoleName { get; set; }
}