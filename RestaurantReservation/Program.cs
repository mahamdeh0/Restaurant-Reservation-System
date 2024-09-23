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
            var customerRepo = new CustomerRepository(context);

            // Customer Repository Test
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
            Console.WriteLine();

            // Employee Repository Test
            var employeeRepo = new EmployeeRepository(context);
            var newEmployee = new Employee { RestaurantId = 1, FirstName = "Hiba", LastName = "Kurd", Position = EmployeePosition.Manager };
            await employeeRepo.CreateAsync(newEmployee);
            Console.WriteLine("Employee created!");

            newEmployee.LastName = "Al-Kurd";
            await employeeRepo.UpdateAsync(newEmployee);
            Console.WriteLine("Employee updated!");

            var retrievedEmployee = await employeeRepo.GetByIdAsync(newEmployee.EmployeeId);
            Console.WriteLine($"Retrieved Employee: {retrievedEmployee.FirstName} {retrievedEmployee.LastName}");

            await employeeRepo.DeleteAsync(newEmployee.EmployeeId);
            Console.WriteLine("Employee deleted!");
            Console.WriteLine();

            // MenuItem Repository Test
            var menuItemRepo = new MenuItemRepository(context);
            var newMenuItem = new MenuItem { RestaurantId = 1, Name = "Pasta", Description = "Delicious spaghetti", Price = 12.99M };
            await menuItemRepo.CreateAsync(newMenuItem);
            Console.WriteLine("MenuItem created!");

            newMenuItem.Price = 14.99M;
            await menuItemRepo.UpdateAsync(newMenuItem);
            Console.WriteLine("MenuItem updated!");

            var retrievedMenuItem = await menuItemRepo.GetByIdAsync(newMenuItem.ItemId);
            Console.WriteLine($"Retrieved MenuItem: {retrievedMenuItem.Name} - ${retrievedMenuItem.Price}");

            await menuItemRepo.DeleteAsync(newMenuItem.ItemId);
            Console.WriteLine("MenuItem deleted!");

        }
    }
}