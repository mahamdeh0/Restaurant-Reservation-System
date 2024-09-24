namespace RestaurantReservation.Db.Interfaces
{
    public interface IOrderRepository
    {
        Task<decimal> CalculateAverageOrderAmountAsync(int employeeId);

    }
}
