using Microsoft.EntityFrameworkCore;

namespace PTP.Domain.Entities;
public class User : BaseEntity
{
    public string FullName { get; set; } = default!;
    public string? Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = default!;
    public DateTime DateOfBirth { get; set; } = default!;
    public string FCMToken { get; set; } = default!;

    #region  Relationship Configuration
    public Guid RoleId { get; set; } = default!;
    public Role Role { get; set; } = default!;
    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; } = default!;
    public ICollection<Order> Orders { get; set; } = default!;
    #endregion

    #region Location Config 
    [Precision(18, 2)]
    public decimal Latitude { get; set; } = default!;
    [Precision(18, 2)]
    public decimal Longitude { get; set; } = default!;
    #endregion
}