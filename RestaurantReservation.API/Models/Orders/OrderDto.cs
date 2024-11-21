namespace RestaurantReservation.API.Models.Orders
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int ReservationId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalAmount { get; set; }
    }
}
