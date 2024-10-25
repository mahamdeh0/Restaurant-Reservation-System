namespace RestaurantReservation.API.Models.Customers
{
    public class CustomerUpdateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
