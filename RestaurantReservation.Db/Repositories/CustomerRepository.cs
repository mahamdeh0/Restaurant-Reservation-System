using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories
{
    public class CustomerRepository : Repository<Customer>
    {
        public CustomerRepository(RestaurantReservationDbContext context) : base(context) { }

    }
}
