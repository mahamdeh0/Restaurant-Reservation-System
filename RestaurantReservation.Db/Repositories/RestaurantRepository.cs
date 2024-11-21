using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories
{
    public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public RestaurantRepository(RestaurantReservationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId)
        {

            var revenue = await _context.Restaurants
              .Where(r => r.RestaurantId == restaurantId)
              .Select(r => _context.CalculateRestaurantRevenue(r.RestaurantId))
              .FirstOrDefaultAsync();

            return revenue;
        }

        public async Task<bool> RestaurantExistsAsync(int restaurantId)
        {
            return await _context.Restaurants.AnyAsync(r => r.RestaurantId == restaurantId);
        }


    }
}
