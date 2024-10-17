using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.CustomerId).HasColumnName("customer_id");
            builder.Property(c => c.FirstName).HasColumnName("first_name").HasMaxLength(50);
            builder.Property(c => c.LastName).HasColumnName("last_name").HasMaxLength(50);
            builder.Property(c => c.Email).HasColumnName("email").HasMaxLength(100);
            builder.Property(c => c.PhoneNumber).HasColumnName("phone_number").HasMaxLength(13);

        }
    }
}
