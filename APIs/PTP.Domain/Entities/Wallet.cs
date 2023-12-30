using Microsoft.EntityFrameworkCore;

namespace PTP.Domain.Entities;
public class Wallet : BaseEntity
{
    public string Name { get; set; } = default!;
    [Precision(18, 2)]
    public decimal Amount { get; set; }
    #region Relationship
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public ICollection<Transaction> Transactions { get; set; } = default!;
    #endregion

}