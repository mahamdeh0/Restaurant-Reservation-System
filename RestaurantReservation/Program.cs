using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestaurantReservation.Db;

namespace RestaurantReservation
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("AppSettings.json")
                                                   .Build();

            var connectionString = config.GetSection("constr").Value;
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(connectionString);
            var options = optionsBuilder.Options;

            using (var context = new RestaurantReservationDbContext(options))
            {
                //Test Connection

            }


        }
    }
}