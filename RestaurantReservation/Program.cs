using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RestaurantReservation.Db;

namespace RestaurantReservation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<RestaurantReservationDbContext>(options =>
                        options.UseSqlServer(context.Configuration.GetConnectionString("RestaurantReservationDb")));
                  

                    services.AddTransient<ReservationTester>();
                })
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var reservationTester = services.GetRequiredService<ReservationTester>();
                await reservationTester.RunTests();
            }
        }
    }
}
