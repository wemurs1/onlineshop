using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Models.Database;

public partial class OnlineShopContext(DbContextOptions options) : DbContext(options)
{
    public virtual DbSet<Banner> Banners { get; set; }
    public virtual DbSet<Menu> Menus { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menu>(entity =>
       {
           entity.Property(e => e.Link).HasMaxLength(300);
           entity.Property(e => e.MenuTitle).HasMaxLength(50);
           entity.Property(e => e.Type).HasMaxLength(20);
       });
        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Banner");

            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.Link).HasMaxLength(100);
            entity.Property(e => e.Position).HasMaxLength(50);
            entity.Property(e => e.SubTitle).HasMaxLength(1000);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        base.OnModelCreating(modelBuilder);
    }
}
