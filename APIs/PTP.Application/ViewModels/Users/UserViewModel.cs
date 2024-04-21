namespace PTP.Application.ViewModels.Users;
public class UserViewModel
{
    public Guid Id { get; set; } = Guid.Empty;
    public string FullName { get; set; } = default!;
    public string? Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = default!;
    public DateTime DateOfBirth { get; set; } = default!;
    public Guid? StoreId { get; set; }
    public string RoleName { get; set; } = default!;
    public string? StoreName { get; set; }
   
}