using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.OrderId).HasColumnName("order_id");
            builder.Property(o => o.ReservationId).HasColumnName("reservation_id");
            builder.Property(o => o.EmployeeId).HasColumnName("employee_id");
            builder.Property(o => o.OrderDate).HasColumnName("order_date");
            builder.Property(o => o.TotalAmount).HasColumnName("total_amount");

            builder.HasOne(e => e.Employee)
              .WithMany(o => o.Orders)
              .HasForeignKey(e => e.EmployeeId)
              .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(r => r.Reservation)
              .WithMany(o => o.Orders)
              .HasForeignKey(r => r.ReservationId)
              .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
