using System.Net;
using PTP.Domain.Enums;

namespace PTP.Application.ViewModels.Users;
public class UserCreateModel
{
    public string FullName { get; set; } = default!;
    public string? Email { get; set; } = string.Empty;
    public string? Password { get; set; } = string.Empty;
    public string? FCMToken { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = default!;
    public DateTime DateOfBirth { get; set; } = default!;
    public string? RoleName { get; set; } = nameof(RoleEnum.Customer);
}