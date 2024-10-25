using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.MenuItems;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IMapper _mapper;

        public MenuItemsController(IMenuItemRepository menuItemRepository, IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
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


    }
}
