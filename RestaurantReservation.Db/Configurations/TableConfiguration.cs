using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Configurations
{
    public class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.Property(t => t.TableId).HasColumnName("table_id");
            builder.Property(t => t.RestaurantId).HasColumnName("restaurant_id");
            builder.Property(t => t.Capacity).HasColumnName("capacity").IsRequired();

            builder.HasOne(r => r.Restaurant)
              .WithMany(t => t.Tables)
              .HasForeignKey(r => r.RestaurantId)
              .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
