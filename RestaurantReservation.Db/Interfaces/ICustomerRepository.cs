using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<List<Customer>> GetCustomersWithReservationsAbovePartySizeAsync(int PartySize);
        public Task<bool> CustomerExistsAsync(int customerId);

    }
}
