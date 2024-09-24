using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetCustomersWithReservationsAbovePartySizeAsync(int PartySize);

    }
}
