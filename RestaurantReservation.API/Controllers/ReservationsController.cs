using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Reservations;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public ReservationsController(IReservationRepository reservationRepository, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
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


    }
}
