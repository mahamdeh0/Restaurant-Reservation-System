using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Models.Views;

namespace RestaurantReservation.Db.Interfaces
{
    public interface IEmployeeRepository
    {
        public Task<List<Employee>> ListManagersAsync();
        public Task<List<EmployeeWithRestaurantDetails>> GetEmployeesWithRestaurantDetailsAsync();

    }
}
