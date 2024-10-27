using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces
{
    public interface IMenuItemRepository : IRepository<MenuItem>
    {
        public Task<List<MenuItem>> ListOrderedMenuItemsAsync(int reservationId);
        public Task<bool> MenuItemExistsAsync(int itemId);


    }
}
