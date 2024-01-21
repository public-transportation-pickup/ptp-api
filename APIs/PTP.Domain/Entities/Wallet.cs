using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PTP.Domain.Enums;

namespace PTP.Domain.Entities;
public class Wallet : BaseEntity
{
    public string Name { get; set; } = default!;
    [Precision(18, 2)]
    public decimal Amount { get; set; }
    public string WalletType { get; set; } = nameof(WalletTypeEnum.Customer);
    #region Relationship
    public Guid? UserId { get; set; }
    public User? User { get; set; } = default!;
    public Guid? StoreId { get; set; } = default!;
    public Store? Store { get; set; } = default!;
    //public ICollection<Store>? Stores { get; set; } = default!;
    public ICollection<Transaction> Transactions { get; set; } = default!;
    public ICollection<WalletLog> WalletLogs { get; set; } = default!;
    #endregion

}