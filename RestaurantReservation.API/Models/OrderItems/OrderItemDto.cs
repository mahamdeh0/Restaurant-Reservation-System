namespace RestaurantReservation.API.Models.OrderItems
{
    public class OrderItemDto
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
