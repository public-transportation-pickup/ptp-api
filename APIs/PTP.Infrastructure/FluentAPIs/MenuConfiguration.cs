using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Store).WithMany(x => x.Menus).HasForeignKey(x => x.StoreId);
        builder.HasMany(x => x.ProductInMenus).WithOne(x => x.Menu).HasForeignKey(x => x.MenuId);
        builder.HasMany(x => x.MenuCategories).WithOne(x => x.Menu).HasForeignKey(x => x.MenuId);
        
    }
}