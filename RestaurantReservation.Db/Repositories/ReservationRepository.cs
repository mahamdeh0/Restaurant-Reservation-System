using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories
{
    public class ReservationRepository : Repository<Reservation>
    {
        public ReservationRepository(RestaurantReservationDbContext context) : base(context) { }

    }
}
