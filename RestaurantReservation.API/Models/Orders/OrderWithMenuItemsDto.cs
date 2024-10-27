using RestaurantReservation.API.Models.MenuItems;

namespace RestaurantReservation.API.Models.Orders
{
    public class OrderWithMenuItemsDto
    {
        public int OrderId { get; set; }
        public int ReservationId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalAmount { get; set; }
        public List<MenuItemDto> MenuItems { get; set; }

    }
}
