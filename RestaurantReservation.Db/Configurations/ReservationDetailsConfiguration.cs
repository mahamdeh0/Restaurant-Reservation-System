using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models.Views;

namespace RestaurantReservation.Db.Configurations
{
    public class ReservationDetailsConfiguration : IEntityTypeConfiguration<ReservationDetails>
    {
        public void Configure(EntityTypeBuilder<ReservationDetails> builder)
        {
            builder.HasNoKey();
            builder.ToView("ReservationsWithDetails");
            builder.Property(e => e.ReservationId).HasColumnName("Reservation_Id");
            builder.Property(e => e.ReservationDate).HasColumnName("Reservation_Date");
            builder.Property(e => e.PartySize).HasColumnName("Party_Size");


        }
    }
}
