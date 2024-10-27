using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.MenuItems;
using RestaurantReservation.API.Models.Orders;
using RestaurantReservation.API.Models.Reservations;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {

        private readonly IReservationRepository _reservationRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public ReservationsController(IReservationRepository reservationRepository, IRestaurantRepository restaurantRepository, ICustomerRepository customerRepository, IMenuItemRepository menuItemRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _restaurantRepository = restaurantRepository;
            _customerRepository = customerRepository;
            _menuItemRepository = menuItemRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of all reservations.
        /// </summary>
        /// <returns>An ActionResult containing a collection of reservation DTOs; returns a 200 OK response with the list of reservations or a 204 No Content response if no reservations are found.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations()
        {
            var reservations = await _reservationRepository.GetAllAsync();

            if (reservations == null || !reservations.Any())
                return NoContent();

            return Ok(_mapper.Map<IEnumerable<ReservationDto>>(reservations));
        }

        /// <summary>
        /// Retrieves a reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation to retrieve.</param>
        /// <returns>An ActionResult containing the reservation DTO if found; otherwise, returns a 404 Not Found response.</returns>
        [HttpGet("{id}", Name = "GetReservation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReservationDto>> GetReservation(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);

            if (reservation == null)
                return NotFound(new { Message = $"Reservation with ID {id} not found." });

            return Ok(_mapper.Map<ReservationDto>(reservation));
        }

        /// <summary>
        /// Retrieves a list of reservations for a specific customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer whose reservations are to be retrieved.</param>
        /// <returns>A list of reservation DTOs for the specified customer; or a 404 Not Found response if the customer does not exist.</returns>
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservationsForCustomer(int customerId)
        {
            if (!await _customerRepository.CustomerExistsAsync(customerId))
            {
                return NotFound(new { Message = "Customer not found." });
            }

            var reservations = await _reservationRepository.GetReservationsByCustomerAsync(customerId);

            return Ok(_mapper.Map<IEnumerable<ReservationDto>>(reservations));
        }

        /// <summary>
        /// Retrieves a list of orders associated with a specific reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation for which to retrieve orders.</param>
        /// <returns>A list of orders with their associated menu items; or a 404 Not Found response if no orders are found for the specified reservation.</returns>
        [HttpGet("{id}/orders")]
        public async Task<ActionResult<IEnumerable<OrderWithMenuItemsDto>>> GetOrdersReservation(int id)
        {
            var orders = await _orderRepository.ListOrdersAndMenuItemsAsync(id);

            if (orders == null || !orders.Any())
                return NotFound(new { Message = "No orders found for the specified reservation." });

            return Ok(_mapper.Map<IEnumerable<OrderWithMenuItemsDto>>(orders));
        }

        /// <summary>
        /// Retrieves a list of menu items that were ordered for a specific order.
        /// </summary>
        /// <param name="id">The ID of the order for which to retrieve ordered menu items.</param>
        /// <returns>A list of ordered menu items; or a 404 Not Found response if no menu items are found for the specified order.</returns>
        [HttpGet("{id}/menu-items")]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetOrderedMenuItemsForReservation(int id)
        {
            var menuItems = await _menuItemRepository.ListOrderedMenuItemsAsync(id);

            if (menuItems == null || !menuItems.Any())
                return NotFound(new { Message = "No menu items found for the specified order." });

            return Ok(_mapper.Map<IEnumerable<MenuItemDto>>(menuItems));
        }


        /// <summary>
        /// Creates a new reservation.
        /// </summary>
        /// <param name="reservationCreationDto">The DTO containing the reservation details.</param>
        /// <returns>A 201 Created response with the created reservation DTO if successful; otherwise, a 400 Bad Request response if the creation fails.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReservationDto>> CreateReservation(ReservationCreationDto reservationCreationDto)
        {
            if (reservationCreationDto == null)
                return BadRequest(new { Message = "Reservation creation data is required." });

            var reservationToAdd = _mapper.Map<Reservation>(reservationCreationDto);

            var addedReservation = await _reservationRepository.CreateAsync(reservationToAdd);

            if (addedReservation == null)
                return BadRequest(new { Message = "Failed to create the reservation." });

            var reservationDto = _mapper.Map<ReservationDto>(addedReservation);

            return CreatedAtRoute(
                "GetReservation",
                new { id = addedReservation.ReservationId },
                reservationDto
            );
        }

        /// <summary>
        /// Fully updates an existing reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation to update.</param>
        /// <param name="reservationUpdateDto">The DTO containing updated reservation details.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the reservation does not exist.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateReservation(int id, ReservationUpdateDto reservationUpdateDto)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);

            if (reservation is null)
                return NotFound(new { Message = "Reservation not found." });

            _mapper.Map(reservationUpdateDto, reservation);

            await _reservationRepository.UpdateAsync(reservation);

            return NoContent();
        }

        /// <summary>
        /// Partially updates an existing reservation by its ID using a JSON patch document.
        /// </summary>
        /// <param name="id">The ID of the reservation to partially update.</param>
        /// <param name="patchDocument">The JSON patch document containing the updates.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found if the reservation does not exist, or a 400 Bad Request if the patch document is invalid.</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PartiallyUpdateReservation(int id, JsonPatchDocument<ReservationUpdateDto> patchDocument)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);

            if (reservation is null)
                return NotFound(new { Message = "Reservation not found." });

            var reservationToPatch = _mapper.Map<ReservationUpdateDto>(reservation);

            patchDocument.ApplyTo(reservationToPatch, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(reservationToPatch))
                return BadRequest(ModelState);

            _mapper.Map(reservationToPatch, reservation);

            await _reservationRepository.UpdateAsync(reservation);

            return NoContent();
        }

        /// <summary>
        /// Deletes a reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation to delete.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the reservation does not exist.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);

            if (reservation is null)
                return NotFound(new { Message = "Reservation not found." });

            await _reservationRepository.DeleteAsync(id);

            return NoContent();
        }

    }
}
