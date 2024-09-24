using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public OrderRepository(RestaurantReservationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<decimal> CalculateAverageOrderAmountAsync(int employeeId)
        {
            var average = await _context.Orders
                          .Where(e => e.EmployeeId == employeeId)
                          .AverageAsync(o => o.TotalAmount);
            return (decimal)average;
        }

        public async Task<List<Order>> ListOrdersAndMenuItemsAsync(int reservationId)
        {
            return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Item)
            .Where(o => o.ReservationId == reservationId)
            .ToListAsync();
        }
    }
}
