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

    }
}
