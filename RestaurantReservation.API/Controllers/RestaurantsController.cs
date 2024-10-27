using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Reservations;
using RestaurantReservation.API.Models.Restaurants;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

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

        /// <summary>
        /// Creates a new restaurant.
        /// </summary>
        /// <param name="restaurantCreationDto">The restaurant creation data.</param>
        /// <returns>A 201 Created response with the created restaurant DTO if successful; otherwise, a 400 Bad Request response if the creation fails.</returns>
        [HttpPost]
        public async Task<ActionResult<RestaurantDto>> CreateRestaurant(RestaurantCreationDto restaurantCreationDto)
        {
            if (restaurantCreationDto == null) 
                return BadRequest(new { Message = "Restaurant creation data is required." });

            var restaurantToAdd = _mapper.Map<Restaurant>(restaurantCreationDto);

            var addedRestaurant = await _restaurantRepository.CreateAsync(restaurantToAdd);

            if (addedRestaurant == null) 
                return BadRequest(new { Message = "Failed to create the restaurant." });

            var restaurantDto = _mapper.Map<RestaurantDto>(addedRestaurant);

            return CreatedAtRoute(
                "GetRestaurant",
                new { id = addedRestaurant.RestaurantId },
                restaurantDto
            );
        }

        /// <summary>
        /// Updates an existing restaurant.
        /// </summary>
        /// <param name="id">The ID of the restaurant to update.</param>
        /// <param name="restaurantUpdateDto">The updated restaurant data.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the restaurant does not exist.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant(int id, RestaurantUpdateDto restaurantUpdateDto)
        {
            if (restaurantUpdateDto == null) 
                return BadRequest(new { Message = "Restaurant update data is required." });

            var restaurant = await _restaurantRepository.GetByIdAsync(id);

            if (restaurant == null)
                return NotFound();

            _mapper.Map(restaurantUpdateDto, restaurant);

            await _restaurantRepository.UpdateAsync(restaurant);

            return NoContent();
        }

        /// <summary>
        /// Partially updates an existing restaurant.
        /// </summary>
        /// <param name="id">The ID of the restaurant to update.</param>
        /// <param name="patchDocument">The patch document containing the changes.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the restaurant does not exist, or a 400 Bad Request response if the patch document is invalid.</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateRestaurant(int id, JsonPatchDocument<RestaurantUpdateDto> patchDocument)
        {
            if (patchDocument == null) 
                return BadRequest(new { Message = "Patch document is required." });

            var restaurant = await _restaurantRepository.GetByIdAsync(id);

            if (restaurant == null)
                return NotFound();

            var restaurantToPatch = _mapper.Map<RestaurantUpdateDto>(restaurant);

            patchDocument.ApplyTo(restaurantToPatch, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(restaurantToPatch))
                return BadRequest(ModelState);

            _mapper.Map(restaurantToPatch, restaurant);

            await _restaurantRepository.UpdateAsync(restaurant);

            return NoContent();
        }

    }
}
