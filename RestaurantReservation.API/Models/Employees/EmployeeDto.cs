using RestaurantReservation.Db.Models.Enum;

namespace RestaurantReservation.API.Models.Employees
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public int RestaurantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public EmployeePosition Position { get; set; }
        public string PositionName => ((EmployeePosition)Position).ToString();


    }
}
