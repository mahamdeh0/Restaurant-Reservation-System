using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Restaurants;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepository;

        private readonly IMapper _mapper;

        public RestaurantsController(IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }


        /// <summary>
        /// Gets all restaurants.
        /// </summary>
        /// <returns>A list of RestaurantDto.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants()
        {

            var restaurants = await _restaurantRepository.GetAllAsync();

            if (restaurants == null || !restaurants.Any())
                return NotFound("No restaurants found.");

            return Ok(_mapper.Map<IEnumerable<RestaurantDto>>(restaurants));
        }

        /// <summary>
        /// Retrieves a restaurant by its ID.
        /// </summary>
        /// <param name="id">The ID of the restaurant to retrieve.</param>
        /// <returns>An ActionResult containing the restaurant DTO if found; otherwise, a 404 Not Found response.</returns>
        [HttpGet("{id}", Name = "GetRestaurant")]
        public async Task<ActionResult<RestaurantDto>> GetRestaurant(int id)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);

            if (restaurant == null)
                return NotFound(new { Message = $"Restaurant with ID {id} not found." }); 


            return Ok(_mapper.Map<RestaurantDto>(restaurant));
        }

    }
}
