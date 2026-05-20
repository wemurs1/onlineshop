using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Models.Database;

public class OnlineShopContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Banner> Banners { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductGallery> ProductGallery { get; set; }

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
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.FullDesc).HasMaxLength(4000);
            entity.Property(e => e.Price).HasPrecision(19, 4);
            entity.Property(e => e.Discount).HasPrecision(19, 4);
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.Tags).HasMaxLength(1000);
            entity.Property(e => e.VideoUrl).HasMaxLength(300);
        });
        modelBuilder.Entity<ProductGallery>(entity =>
        {
            entity.Property(e => e.ImageName).HasMaxLength(50);
        });

        base.OnModelCreating(modelBuilder);
    }
}
