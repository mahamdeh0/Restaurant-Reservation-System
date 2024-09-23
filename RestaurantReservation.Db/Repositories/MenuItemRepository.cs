using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories
{
    public class MenuItemRepository : Repository<MenuItem>
    {
        public MenuItemRepository(RestaurantReservationDbContext context) : base(context) { }

    }
}
