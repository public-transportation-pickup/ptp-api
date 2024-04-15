using System.Diagnostics;
using Amazon.SecurityToken.Model;
using Microsoft.EntityFrameworkCore;
using PTP.Domain.Enums;

namespace PTP.Domain.Entities;
public class WalletLog : BaseEntity
{
    public string Source { get; set; } = string.Empty;

    [Precision(18, 2)]
    public decimal Amount { get; set; } = default!;
    public string Status { get; set; } = nameof(WalletLogStatusEnum.Sucess);
    public string Type { get; set; } = nameof(WalletLogTypeEnum.Deposit);
    public string? TxnRef { get; set; } = string.Empty;
    public string TransactionNo { get; set; } = string.Empty;

    #region  Relationship Config
    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; } = default!;
    #endregion
}