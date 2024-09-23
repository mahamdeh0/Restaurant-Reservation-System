using RestaurantReservation.Db;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var context = new RestaurantReservationDbContext(); 
            var customerRepo = new CustomerRepository(context);

            var newCustomer = new Customer { FirstName = "Hiba", LastName = "Kurd", Email = "Hiba@Foothill.com", PhoneNumber = "1234567890" };
            await customerRepo.CreateAsync(newCustomer);
            Console.WriteLine("Customer created!");

            newCustomer.LastName = "AlKurd";
            await customerRepo.UpdateAsync(newCustomer);
            Console.WriteLine("Customer updated!");

            var retrievedCustomer = await customerRepo.GetByIdAsync(newCustomer.CustomerId);
            Console.WriteLine($"Retrieved Customer: {retrievedCustomer.FirstName} {retrievedCustomer.LastName}");

            await customerRepo.DeleteAsync(newCustomer.CustomerId);
            Console.WriteLine("Customer deleted!");
        }
    }
}