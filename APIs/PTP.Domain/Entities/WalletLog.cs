using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PTP.Domain.Enums;

namespace PTP.Domain.Entities;
public class WalletLog : BaseEntity
{
    public string Source { get; set; } = string.Empty;

    [Precision(18, 2)]
    public decimal Amount { get; set; } = default!;
    public string Type { get; set; } = nameof(WalletLogTypeEnum.Deposit);

    #region  Relationship Config
    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; } = default!;
    #endregion
}