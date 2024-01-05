using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class ProductInMenuConfiguration : IEntityTypeConfiguration<ProductInMenu>
{
    public void Configure(EntityTypeBuilder<ProductInMenu> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Menu).WithMany(x => x.ProductInMenus).HasForeignKey(x => x.MenuId);
        builder.HasOne(x => x.Product).WithMany(x => x.ProductInMenus).HasForeignKey(x => x.ProductId)
        .OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(x => x.OrderDetails).WithOne(x => x.ProductInMenu).HasForeignKey(x => x.ProductInMenuId).OnDelete(DeleteBehavior.NoAction);
    }
}