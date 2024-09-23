using RestaurantReservation.Db;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Models.Enum;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var context = new RestaurantReservationDbContext();
            var reservationTester = new ReservationTester(context);
            await reservationTester.RunTests();
        }
    }
}