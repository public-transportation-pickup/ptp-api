using System.Diagnostics;
using PTP.Domain.Enums;

namespace PTP.Domain.Entities;
public class Payment : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Status { get; set; } = nameof(PaymentStatusEnum.Created);

    #region Relationship
    public Guid OrderId { get; set; } = default!;
    public Order Order { get; set; } = default!;
    public ICollection<Transaction> Transactions = default!;
    #endregion
}