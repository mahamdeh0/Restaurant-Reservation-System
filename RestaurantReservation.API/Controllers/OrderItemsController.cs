﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.OrderItems;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;
using System.Text.Json;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/orders/{orderId}/order-items")]
    [ApiController]
    [Authorize]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private const int MaxPageSize = 5;


        public OrderItemsController(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all Order Items for a specific order with pagination.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve items for.</param>
        /// <param name="pageNumber">The number of the page to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A list of Order Item DTOs if available; otherwise, a 204 No Content response if no Order Items exist.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItems(int orderId, int pageNumber = 1, int pageSize = 10)
        {
            if (!await _orderRepository.OrderItemExistsAsync(orderId))
                return NotFound("No order found with the specified ID.");

            if (pageNumber < 1 || pageSize < 1)
                return BadRequest($"'{nameof(pageNumber)}' and '{nameof(pageSize)}' must be greater than 0.");

            pageSize = Math.Min(pageSize, MaxPageSize);

            var (orderItems, paginationMetadata) = await _orderItemRepository.GetAllAsync(
                item => item.OrderId == orderId,
                pageNumber,
                pageSize
            );

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            if (!orderItems.Any())
                return NoContent();

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
        /// Creates a new order item for a specified order.
        /// </summary>
        /// <param name="orderId">The ID of the order to add the item to.</param>
        /// <param name="orderItemCreationDto">The data for the new order item.</param>
        /// <returns>A 201 Created response with the created order item DTO, or a 404 Not Found if the order does not exist.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderItemDto>> CreateOrderItem(int orderId, OrderItemCreationDto orderItemCreationDto)
        {
            if (!await _orderRepository.OrderItemExistsAsync(orderId))
            {
                return NotFound("No order found with the specified ID.");
            }

            var orderItem = _mapper.Map<OrderItem>(orderItemCreationDto);
            orderItem.OrderId = orderId; 

            await _orderItemRepository.CreateAsync(orderItem);

            var orderItemDto = _mapper.Map<OrderItemDto>(orderItem);

            return CreatedAtRoute("GetOrderItem", new { orderId = orderId, id = orderItemDto.ItemId }, orderItemDto);
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

        /// <summary>
        /// Deletes a specific order item from a specified order.
        /// </summary>
        /// <param name="orderId">The ID of the order that contains the order item.</param>
        /// <param name="id">The ID of the order item to delete.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the order or order item does not exist, or if the order item does not belong to the specified order.</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteOrderItem(int orderId, int id)
        {
            if (!await _orderRepository.OrderItemExistsAsync(orderId))
                return NotFound("No order found with the specified ID.");

            var orderItem = await _orderItemRepository.GetByIdAsync(id);

            if (orderItem is null || orderItem.OrderId != orderId)
            {
                return NotFound("Order item with the specified ID was not found for the provided order.");
            }

            await _orderItemRepository.DeleteAsync(id);

            return NoContent();
        }


    }
}
