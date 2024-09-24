namespace RestaurantReservation.Db.Interfaces
{
    public interface IRestaurantRepository
    {
        public Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId);

    }
}
