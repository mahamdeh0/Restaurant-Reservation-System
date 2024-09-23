using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            Console.WriteLine("Customer deleted! \n");

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
            Console.WriteLine("Employee deleted! \n");

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
            Console.WriteLine("MenuItem deleted! \n");

            // Order Repository Test
            var orderRepo = new OrderRepository(context);
            var newOrder = new Order { EmployeeId = 2, ReservationId = 2 ,OrderDate = DateTime.Now, TotalAmount = 50 };
            await orderRepo.CreateAsync(newOrder);
            Console.WriteLine("Order created!");

            newOrder.TotalAmount = 75;
            await orderRepo.UpdateAsync(newOrder);
            Console.WriteLine("Order updated!");

            var retrievedOrder = await orderRepo.GetByIdAsync(newOrder.OrderId);
            Console.WriteLine($"Retrieved Order ID: {retrievedOrder.OrderId} - Total: ${retrievedOrder.TotalAmount}");

            await orderRepo.DeleteAsync(newOrder.OrderId);
            Console.WriteLine("Order deleted! \n");

            // OrderItem Repository Test
            var orderItemRepo = new OrderItemRepository(context);
            var newOrderItem = new OrderItem { ItemId = 1, OrderId = 1, Quantity = 2 }; 
            await orderItemRepo.CreateAsync(newOrderItem);
            Console.WriteLine("OrderItem created!");

            newOrderItem.Quantity = 3;
            await orderItemRepo.UpdateAsync(newOrderItem);
            Console.WriteLine("OrderItem updated!");

            var retrievedOrderItem = await orderItemRepo.GetByIdAsync(newOrderItem.OrderItemId);
            Console.WriteLine($"Retrieved OrderItem: Item ID: {retrievedOrderItem.ItemId}, Quantity: {retrievedOrderItem.Quantity}");

            await orderItemRepo.DeleteAsync(newOrderItem.OrderItemId);
            Console.WriteLine("OrderItem deleted! \n");

            // Reservation Repository Test
            var reservationRepo = new ReservationRepository(context);
            var newReservation = new Reservation { CustomerId = 1, RestaurantId = 2, TableId = 1, PartySize = 4, ReservationDate = DateTime.Now };
            await reservationRepo.CreateAsync(newReservation);
            Console.WriteLine("Reservation created!");

            newReservation.PartySize = 5;
            await reservationRepo.UpdateAsync(newReservation);
            Console.WriteLine("Reservation updated!");

            var retrievedReservation = await reservationRepo.GetByIdAsync(newReservation.ReservationId);
            Console.WriteLine($"Retrieved Reservation: Party Size: {retrievedReservation.PartySize}");

            await reservationRepo.DeleteAsync(newReservation.ReservationId);
            Console.WriteLine("Reservation deleted! \n");

            // Table Repository Test
            var tableRepo = new TableRepository(context);
            var newTable = new RestaurantReservation.Db.Models.Entities.Table { RestaurantId = 2 ,  Capacity = 10 };
            await tableRepo.CreateAsync(newTable);
            Console.WriteLine("Table created!");

            newTable.Capacity = 8;
            await tableRepo.UpdateAsync(newTable);
            Console.WriteLine("Table updated!");

            var retrievedTable = await tableRepo.GetByIdAsync(newTable.TableId);
            Console.WriteLine($"Retrieved Table: Capacity: {retrievedTable.Capacity}");

            await tableRepo.DeleteAsync(newTable.TableId);
            Console.WriteLine("Table deleted!");


        }
    }
}