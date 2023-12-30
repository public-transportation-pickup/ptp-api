using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Order).WithOne(x => x.Payment).HasForeignKey<Payment>(x => x.OrderId);
        builder.HasMany(x => x.Transactions).WithOne(x => x.Payment).HasForeignKey(x => x.PaymentId);
    }
}