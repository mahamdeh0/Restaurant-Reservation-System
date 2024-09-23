using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories
{
    public class EmployeeRepository : Repository<Employee>
    {
        public EmployeeRepository(RestaurantReservationDbContext context) : base(context) { }

    }
}
