using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories
{
    public class OrderRepository : Repository<Order>
    {
        public OrderRepository(RestaurantReservationDbContext context) : base(context) { }

    }
}
