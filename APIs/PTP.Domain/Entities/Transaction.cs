using Microsoft.EntityFrameworkCore;
using PTP.Domain.Enums;

namespace PTP.Domain.Entities;
public class Transaction : BaseEntity
{
    public string Name { get; set; } = default!;
    [Precision(18, 2)]
    public decimal Amount { get; set; } = default!;
    public string TransactionType { get; set; } = nameof(TransactionTypeEnum.None);


    #region Relationship
    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; } = default!;
    public Guid? PaymentId { get; set; }
    public Payment? Payment { get; set; } = default!;
    #endregion
}