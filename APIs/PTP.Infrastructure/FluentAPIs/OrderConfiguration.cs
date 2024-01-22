using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.User).WithMany(x => x.Orders).HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.Payment).WithOne(x => x.Order).HasForeignKey<Payment>(x => x.OrderId);
        builder.HasMany(x => x.OrderDetails).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);
        builder.HasOne(x => x.Station).WithMany(x => x.Orders).HasForeignKey(x => x.StationId);
        builder.HasOne(x => x.Store).WithMany(x => x.Orders).HasForeignKey(x => x.StoreId);
    }
}
