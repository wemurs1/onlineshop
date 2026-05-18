using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Models.Database;

public class OnlineShopContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Banner> Banners { get; set; }
    public DbSet<Menu> Menus { get; set; }

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
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.Link).HasMaxLength(100);
            entity.Property(e => e.Position).HasMaxLength(50);
            entity.Property(e => e.SubTitle).HasMaxLength(1000);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        base.OnModelCreating(modelBuilder);
    }
}
