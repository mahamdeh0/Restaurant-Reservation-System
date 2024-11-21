using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Task<decimal> CalculateAverageOrderAmountAsync(int employeeId);
        public Task<List<Order>> ListOrdersAndMenuItemsAsync(int reservationId);
        public Task<bool> OrderItemExistsAsync(int orderId);


    }
}
