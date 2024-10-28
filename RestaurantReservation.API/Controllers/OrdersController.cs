using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Orders;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;
using System.Text.Json;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private const int MaxPageSize = 5;

        public OrdersController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of all orders with pagination.
        /// </summary>
        /// <param name="pageNumber">The number of the page to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>An ActionResult containing a collection of order DTOs; returns a 200 OK response with the list of orders or a 204 No Content response if no orders are found.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest($"'{nameof(pageNumber)}' and '{nameof(pageSize)}' must be greater than 0.");


            pageSize = Math.Min(pageSize, MaxPageSize);

            var (orders, paginationMetadata) = await _orderRepository.GetAllAsync(
                _ => true, 
                pageNumber,
                pageSize
            );

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            if (!orders.Any())
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

        /// <summary>
        /// Fully updates an existing order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to update.</param>
        /// <param name="orderUpdateDto">The DTO containing updated order details.</param>
        /// <returns>A 204 No Content response if successful, or a 404 Not Found response if the order does not exist.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder(int id, OrderUpdateDto orderUpdateDto)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order is null)
                return NotFound(new { Message = "Order not found." });

            _mapper.Map(orderUpdateDto, order);

            await _orderRepository.UpdateAsync(order);

            return NoContent();
        }

        /// <summary>
        /// Partially updates an existing order by its ID using a JSON patch document.
        /// </summary>
        /// <param name="id">The ID of the order to partially update.</param>
        /// <param name="patchDocument">The JSON patch document with the updates to apply.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found if the order does not exist, or a 400 Bad Request if the patch document is invalid.</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PartiallyUpdateOrder(int id, JsonPatchDocument<OrderUpdateDto> patchDocument)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order is null)
                return NotFound(new { Message = "Order not found." });

            var orderToPatch = _mapper.Map<OrderUpdateDto>(order);

            patchDocument.ApplyTo(orderToPatch, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(orderToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(orderToPatch, order);

            await _orderRepository.UpdateAsync(order);

            return NoContent();
        }

        /// <summary>
        /// Deletes an order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the order does not exist.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order is null)
                return NotFound(new { Message = $"Order with ID {id} not found." });

            await _orderRepository.DeleteAsync(id);

            return NoContent();
        }

    }
}
