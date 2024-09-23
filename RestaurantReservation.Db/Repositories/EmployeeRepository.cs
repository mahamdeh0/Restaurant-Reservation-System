using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Models.Enum;

namespace RestaurantReservation.Db.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public EmployeeRepository(RestaurantReservationDbContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<List<Employee>> ListManagersAsync()
        {
            return await _context.Employees
                .Where(e => e.Position == EmployeePosition.Manager)
                .ToListAsync();
        }
    }
}
