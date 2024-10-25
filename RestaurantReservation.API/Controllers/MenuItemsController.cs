using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.MenuItems;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/menu-items")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public MenuItemsController(IMenuItemRepository menuItemRepository, IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }



        /// <summary>
        /// Retrieves all menu items.
        /// </summary>
        /// <returns>A list of menu item DTOs if available; otherwise, a 204 No Content response if no menu items exist.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItems()
        {
            var menuItems = await _menuItemRepository.GetAllAsync();

            if (!menuItems.Any())
            {
                return NoContent();
            }

            return Ok(_mapper.Map<IEnumerable<MenuItemDto>>(menuItems));
        }

        /// <summary>
        /// Creates a new menu item.
        /// </summary>
        /// <param name="menuItemCreationDto">The menu item creation data.</param>
        /// <returns>A 201 Created response with the created menu item DTO if successful; otherwise, a 404 Not Found response if the restaurant does not exist.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MenuItemDto>> CreateMenuItem(MenuItemCreationDto menuItemCreationDto)
        {
            if (!await _restaurantRepository.RestaurantExistsAsync(menuItemCreationDto.RestaurantId))
            {
                return NotFound(new { Message = "Restaurant not found." });
            }

            var menuItemToAdd = _mapper.Map<MenuItem>(menuItemCreationDto);
            var addedMenuItem = await _menuItemRepository.CreateAsync(menuItemToAdd);
            var createdMenuItemDto = _mapper.Map<MenuItemDto>(addedMenuItem);

            return Ok(createdMenuItemDto);
        }


    }
}
