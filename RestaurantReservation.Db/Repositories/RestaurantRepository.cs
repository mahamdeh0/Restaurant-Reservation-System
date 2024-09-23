using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories
{
    public class RestaurantRepository : Repository<Restaurant>, IRepository<Restaurant>
    {
        public RestaurantRepository(RestaurantReservationDbContext context) : base(context) { }

    }
}
