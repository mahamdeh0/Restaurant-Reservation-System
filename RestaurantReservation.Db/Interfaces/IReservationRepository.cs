using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetReservationsByCustomerAsync(int customerId);

    }
}
