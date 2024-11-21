using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories
{
    public class MenuItemRepository : Repository<MenuItem> , IMenuItemRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public MenuItemRepository(RestaurantReservationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<MenuItem>> ListOrderedMenuItemsAsync(int reservationId)
        {
            return await _context.OrderItems
             .Include(o => o.Order)
             .Include(i => i.Item)
             .Where(o => o.Order.ReservationId == reservationId)
             .Select(i => i.Item)
             .Distinct()
             .ToListAsync();

        }
        public async Task<bool> MenuItemExistsAsync(int itemId)
        {
            return await _context.MenuItems.AnyAsync(m => m.ItemId == itemId);
        }
    }
}
