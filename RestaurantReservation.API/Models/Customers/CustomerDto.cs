namespace RestaurantReservation.API.Models.Customers
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Phone {  get; set; }
    }   
}
