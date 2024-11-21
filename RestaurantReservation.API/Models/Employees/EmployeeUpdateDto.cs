using RestaurantReservation.Db.Models.Enum;

namespace RestaurantReservation.API.Models.Employees
{
    public class EmployeeUpdateDto
    {
        public int RestaurantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public EmployeePosition Position { get; set; }
    }
}
