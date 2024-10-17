using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.EmployeeId).HasColumnName("employee_id");
            builder.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(50);
            builder.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(50);
            builder.Property(e => e.Position).HasColumnName("position").HasMaxLength(10).IsRequired();
            builder.Property(e => e.RestaurantId).HasColumnName("restaurant_id");

            builder.HasOne(r => r.Restaurant)
                .WithMany(e => e.Employees)
                .HasForeignKey(r => r.RestaurantId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
