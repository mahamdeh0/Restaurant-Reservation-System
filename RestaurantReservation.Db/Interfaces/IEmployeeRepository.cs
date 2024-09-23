using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces
{
    public interface IEmployeeRepository
    {
        public Task<List<Employee>> ListManagersAsync(); 

    }
}
