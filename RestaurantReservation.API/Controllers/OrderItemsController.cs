using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.OrderItems;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderItemsController(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all Order Items.
        /// </summary>
        /// <returns>A list of Order Item DTOs if available; otherwise, a 204 No Content response if no Order Items exist.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItems(int orderId)
        {
            if (!await _orderRepository.OrderItemExistsAsync(orderId))
            {
                return NotFound("No order found with the specified ID.");
            }

            var orderItems = await _orderItemRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<OrderItemDto>>(orderItems));
        }


    }
}
