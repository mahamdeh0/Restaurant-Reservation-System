using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Configurations
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.HasKey(m => m.ItemId);
            builder.Property(m => m.ItemId).HasColumnName("item_id");
            builder.Property(m => m.RestaurantId).HasColumnName("restaurant_id");
            builder.Property(m => m.Description).HasColumnName("description").HasMaxLength(200);
            builder.Property(m => m.Name).HasColumnName("name").HasMaxLength(50);
            builder.Property(m => m.Price).HasColumnName("price").HasColumnType("decimal(3, 2)");

            builder.HasOne(r => r.Restaurant)
              .WithMany(m => m.MenuItems)
              .HasForeignKey(r => r.RestaurantId)
              .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
