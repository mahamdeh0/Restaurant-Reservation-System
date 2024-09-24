using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces
{
    public interface IOrderRepository
    {
        public Task<decimal> CalculateAverageOrderAmountAsync(int employeeId);
        public Task<List<Order>> ListOrdersAndMenuItemsAsync(int reservationId);


    }
}
