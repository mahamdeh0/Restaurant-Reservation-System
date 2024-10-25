namespace RestaurantReservation.API.Models.MenuItems
{
    public class MenuItemUpdateDto
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
