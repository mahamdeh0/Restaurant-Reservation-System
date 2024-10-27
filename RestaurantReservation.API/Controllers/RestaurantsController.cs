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

    }
}
