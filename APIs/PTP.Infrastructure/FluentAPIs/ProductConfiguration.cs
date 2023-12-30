using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
        builder.HasMany(x => x.ProductImages).WithOne(x => x.Product).HasForeignKey(x => x.ProductId);
        builder.HasMany(x => x.ProductInMenus).WithOne(x => x.Product).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.ClientNoAction);
        builder.HasMany(x => x.OrderDetails).WithOne(x => x.Product).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Store).WithMany(x => x.Products).HasForeignKey(x => x.StoreId);
    }
}
