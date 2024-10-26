using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Orders;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of all orders.
        /// </summary>
        /// <returns>An ActionResult containing a collection of order DTOs; returns a 200 OK response with the list of orders or a 204 No Content response if no orders are found.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _orderRepository.GetAllAsync();

            if (orders == null || !orders.Any())
                return NoContent(); 

            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders)); 
        }

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>An ActionResult containing the order DTO if found; otherwise, returns a 404 Not Found response.</returns>
        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                return NotFound(new { Message = $"Order with ID {id} not found." });

            return Ok(_mapper.Map<OrderDto>(order));
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="orderCreationDto">The order creation data.</param>
        /// <returns>A 201 Created response with the created order DTO if successful; otherwise, a 400 Bad Request response if the creation fails.</returns>
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(OrderCreationDto orderCreationDto)
        {
            if (orderCreationDto == null)
                return BadRequest(new { Message = "Order creation data is required." });


            var orderToAdd = _mapper.Map<Order>(orderCreationDto);

            var addedOrder = await _orderRepository.CreateAsync(orderToAdd);

            if (addedOrder == null)
                return BadRequest(new { Message = "Failed to create the order." });

            var orderDto = _mapper.Map<OrderDto>(addedOrder);

            return CreatedAtRoute(
                "GetOrder",
                new { id = addedOrder.OrderId },
                orderDto
            );
        }


    }
}
