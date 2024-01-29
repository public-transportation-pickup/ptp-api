using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Order).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderId);
        builder.HasOne(x => x.ProductInMenu).WithMany(x => x.OrderDetails).HasForeignKey(x => x.ProductMenuId).OnDelete(DeleteBehavior.Cascade);
    }
}