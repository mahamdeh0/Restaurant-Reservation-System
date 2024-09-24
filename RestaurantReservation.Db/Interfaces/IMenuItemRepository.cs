using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces
{
    public interface IMenuItemRepository
    {
        public Task<List<MenuItem>> ListOrderedMenuItemsAsync(int reservationId);

    }
}
