using RestaurantReservation.Db;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Models.Enum;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation
{
    class ReservationTester
    {
        private readonly RestaurantReservationDbContext _context;

        public ReservationTester(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        public async Task RunTests()
        {
            await TestCustomerRepository();
            await TestEmployeeRepository();
            await TestMenuItemRepository();
            await TestOrderRepository();
            await TestOrderItemRepository();
            await TestReservationRepository();
            await TestRestaurantRepository();
            await TestTableRepository();
            await TestListManagers();
            await TestCalculateAverageOrderAmount();
            await TestListOrderedMenuItems();
            await TestListOrdersAndMenuItemsAsync();


        }


        private async Task TestCustomerRepository()
        {
            var customerRepo = new CustomerRepository(_context);
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
        }

        private async Task TestEmployeeRepository()
        {
            var employeeRepo = new EmployeeRepository(_context);
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
        }

        private async Task TestMenuItemRepository()
        {
            var menuItemRepo = new MenuItemRepository(_context);
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
        }

        private async Task TestOrderRepository()
        {
            var orderRepo = new OrderRepository(_context);
            var newOrder = new Order { EmployeeId = 2, ReservationId = 2, OrderDate = DateTime.Now, TotalAmount = 50 };
            await orderRepo.CreateAsync(newOrder);
            Console.WriteLine("Order created!");

            newOrder.TotalAmount = 75;
            await orderRepo.UpdateAsync(newOrder);
            Console.WriteLine("Order updated!");

            var retrievedOrder = await orderRepo.GetByIdAsync(newOrder.OrderId);
            Console.WriteLine($"Retrieved Order ID: {retrievedOrder.OrderId} - Total: ${retrievedOrder.TotalAmount}");

            await orderRepo.DeleteAsync(newOrder.OrderId);
            Console.WriteLine("Order deleted! \n");
        }

        private async Task TestOrderItemRepository()
        {
            var orderItemRepo = new OrderItemRepository(_context);
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
        }

        private async Task TestReservationRepository()
        {
            var reservationRepo = new ReservationRepository(_context);
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
        }

        private async Task TestRestaurantRepository()
        {
            var restaurantRepo = new RestaurantRepository(_context);
            var newRestaurant = new Restaurant { Name = "The Great Restaurant", Address = "123 Food St.", PhoneNumber = "123-456-7890", OpeningHours = "9 AM - 10 PM" };
            await restaurantRepo.CreateAsync(newRestaurant);
            Console.WriteLine("Restaurant created!");

            newRestaurant.Name = "The Amazing Restaurant";
            await restaurantRepo.UpdateAsync(newRestaurant);
            Console.WriteLine("Restaurant updated!");

            var retrievedRestaurant = await restaurantRepo.GetByIdAsync(newRestaurant.RestaurantId);
            Console.WriteLine($"Retrieved Restaurant: {retrievedRestaurant.Name}, Address: {retrievedRestaurant.Address}");

            await restaurantRepo.DeleteAsync(newRestaurant.RestaurantId);
            Console.WriteLine("Restaurant deleted! \n");
        }

        private async Task TestTableRepository()
        {
            var tableRepo = new TableRepository(_context);
            var newTable = new RestaurantReservation.Db.Models.Entities.Table { RestaurantId = 2, Capacity = 10 };
            await tableRepo.CreateAsync(newTable);
            Console.WriteLine("Table created!");

            newTable.Capacity = 8;
            await tableRepo.UpdateAsync(newTable);
            Console.WriteLine("Table updated!");

            var retrievedTable = await tableRepo.GetByIdAsync(newTable.TableId);
            Console.WriteLine($"Retrieved Table: Capacity: {retrievedTable.Capacity}");

            await tableRepo.DeleteAsync(newTable.TableId);
            Console.WriteLine("Table deleted!\n");
        }
        private async Task TestListManagers()
        {
            IEmployeeRepository employeeRepo = new EmployeeRepository(_context);
            var managers = await employeeRepo.ListManagersAsync();

            Console.WriteLine("List of Managers:");
            foreach (var manager in managers)
            {
                Console.WriteLine($"Manager: {manager.FirstName} {manager.LastName}");
            }

        }
        private async Task TestCalculateAverageOrderAmount()
        {
            var OrderRepo = new OrderRepository(_context);

            var orders = new List<Order>
                {
                     new Order { EmployeeId = 34,ReservationId = 2, TotalAmount = 50 },
                     new Order { EmployeeId = 34,ReservationId = 2, TotalAmount = 100 },
                     new Order { EmployeeId = 34,ReservationId = 2, TotalAmount = 150 },
                  };

            foreach (var order in orders)
            {
                await _context.Orders.AddAsync(order);
            }
            await _context.SaveChangesAsync();

            var EmployeeId = 34;
            var averageAmount = await OrderRepo.CalculateAverageOrderAmountAsync(EmployeeId);
            Console.WriteLine($"\nAverage Order Amount for Employee 1: {averageAmount}");

            if (averageAmount == 100m)
            {
                Console.WriteLine("Test for CalculateAverageOrderAmountAsync passed");
            }
            else
            {
                Console.WriteLine($"Test for CalculateAverageOrderAmountAsync failed! Expected 100 but got {averageAmount}");
            }

            _context.Orders.RemoveRange(orders);
            await _context.SaveChangesAsync();

        }

        private async Task TestListOrderedMenuItems()
        {
            var reservationId = 1; 

            var menuItems = await new MenuItemRepository(_context).ListOrderedMenuItemsAsync(reservationId);

            if (menuItems != null && menuItems.Any())
            {
                Console.WriteLine($"\nMenu items ordered for Reservation ID {reservationId}:");
                foreach (var item in menuItems)
                {
                    Console.WriteLine($"{item.Name} --- {item.Price}$");
                }
            }
            else
            {
                Console.WriteLine($"\nNo menu items found for Reservation ID {reservationId}");
            }
        }

        private async Task TestListOrdersAndMenuItemsAsync()
        {
            var reservationId = 3;

            var ordersWithItems = await new OrderRepository(_context).ListOrdersAndMenuItemsAsync(reservationId);

            if (ordersWithItems != null && ordersWithItems.Any())
            {
                Console.WriteLine($"\nOrders and menu items for Reservation ID {reservationId}:\n");

                foreach (var order in ordersWithItems)
                {
                    Console.WriteLine($"Order ID: {order.OrderId}");

                    foreach (var orderItem in order.OrderItems)
                    {
                        Console.WriteLine($"* Menu Item: {orderItem.Item.Name} --- Price: {orderItem.Item.Price}$");
                    }
                }
            }
            else
            {
                Console.WriteLine($"No orders found for Reservation ID {reservationId}");
            }
        }
    }
}


