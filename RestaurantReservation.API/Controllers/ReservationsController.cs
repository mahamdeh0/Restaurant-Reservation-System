using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Reservations;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

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


    }
}
