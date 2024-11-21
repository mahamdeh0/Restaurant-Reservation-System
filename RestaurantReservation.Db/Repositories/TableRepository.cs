using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories
{
    public class TableRepository : Repository<Table> , ITableRepository
    {
        public TableRepository(RestaurantReservationDbContext context) : base(context) { }

    }
}
