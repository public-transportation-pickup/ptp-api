using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PTP.Domain.Enums;

namespace PTP.Domain.Entities;
public class Payment : BaseEntity
{
    [Precision(18, 2)]
    public decimal Total { get; set; } = default!;
    public string PaymentType { get; set; } = default!;
    public string Status { get; set; } = nameof(PaymentStatusEnum.Paid);

    #region Relationship
    public Guid OrderId { get; set; } = default!;
    public Order Order { get; set; } = default!;
    public ICollection<Transaction> Transactions = default!;
    #endregion
}