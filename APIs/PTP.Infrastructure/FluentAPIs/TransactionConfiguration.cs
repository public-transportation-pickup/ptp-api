using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Payment).WithMany(x => x.Transactions).HasForeignKey(x => x.PaymentId);
        builder.HasOne(x => x.Wallet).WithMany(x => x.Transactions).HasForeignKey(x => x.WalletId);
    }
}