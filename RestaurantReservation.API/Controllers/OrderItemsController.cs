﻿using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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

        /// <summary>
        /// Retrieves a specific order item by its ID for a given order ID.
        /// </summary>
        /// <param name="orderId">The ID of the order that the order item belongs to.</param>
        /// <param name="id">The ID of the order item to retrieve.</param>
        /// <returns>A 200 OK response with the order item DTO if found; otherwise, a 404 Not Found response if the order or order item does not exist, or if the order item does not belong to the specified order.</returns>
        [HttpGet("{id}", Name = "GetOrderItem")]
        public async Task<ActionResult<OrderItemDto>> GetOrderItem(int orderId, int id)
        {
            if (!await _orderRepository.OrderItemExistsAsync(orderId))
            {
                return NotFound("No order found with the specified ID.");
            }

            var orderItem = await _orderItemRepository.GetByIdAsync(id);

            if (orderItem is null)
                return NotFound("No order item found with the specified ID.");

            if (orderItem.OrderId != orderId)
                return NotFound("The order item does not belong to the specified order. Please verify the order ID.");

            return Ok(_mapper.Map<OrderItemDto>(orderItem));
        }

        /// <summary>
        /// Updates an existing order item within a specified order.
        /// </summary>
        /// <param name="orderId">The ID of the order that contains the order item.</param>
        /// <param name="id">The ID of the order item to update.</param>
        /// <param name="orderItemUpdateDto">The updated data for the order item.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the order or order item does not exist, or if the order item does not belong to the specified order.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateOrderItem(int orderId, int id, OrderItemUpdateDto orderItemUpdateDto)
        {
            if (!await _orderRepository.OrderItemExistsAsync(orderId))
                return NotFound("No order found with the specified ID.");


            var orderItem = await _orderItemRepository.GetByIdAsync(id);

            if (orderItem is null)
                return NotFound("Order item with the given ID is not found.");

            if (orderItem.OrderId != orderId)
                return NotFound("The specified order does not contain an item with the given ID.");

            _mapper.Map(orderItemUpdateDto, orderItem);

            await _orderItemRepository.UpdateAsync(orderItem);

            return NoContent();
        }

        /// <summary>
        /// Partially updates an existing order item within a specified order using a JSON patch document.
        /// </summary>
        /// <param name="orderId">The ID of the order that contains the order item.</param>
        /// <param name="id">The ID of the order item to partially update.</param>
        /// <param name="patchDocument">The JSON patch document containing the updates to apply.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the order or order item does not exist, or if the order item does not belong to the specified order, or a 400 Bad Request if the model state is invalid.</returns>
        [HttpPatch]
        public async Task<IActionResult> PartiallyUpdateOrderItem(int orderId, int id, JsonPatchDocument<OrderItemUpdateDto> patchDocument)
        {
            if (!await _orderRepository.OrderItemExistsAsync(orderId))
                return NotFound("No order found with the specified ID.");

            var orderItem = await _orderItemRepository.GetByIdAsync(id);

            if (orderItem is null)
                return NotFound("Order item with the given ID is not found.");

            if (orderItem.OrderId != orderId)
                return NotFound("The specified order does not contain an item with the given ID.");

            var orderItemToPatch = _mapper.Map<OrderItemUpdateDto>(orderItem);

            patchDocument.ApplyTo(orderItemToPatch, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(orderItemToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(orderItemToPatch, orderItem);

            await _orderItemRepository.UpdateAsync(orderItem);

            return NoContent();
        }



    }
}
