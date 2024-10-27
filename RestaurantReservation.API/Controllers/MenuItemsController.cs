using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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

        /// <summary>
        /// Updates an existing menu item by ID.
        /// </summary>
        /// <param name="id">The ID of the menu item to update.</param>
        /// <param name="menuItemUpdateDto">The menu item update data.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the menu item or restaurant does not exist.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMenuItem(int id, MenuItemUpdateDto menuItemUpdateDto)
        {
            var existingMenuItem = await _menuItemRepository.GetByIdAsync(id);

            if (existingMenuItem is null)
                return NotFound();

            if (!await _restaurantRepository.RestaurantExistsAsync(menuItemUpdateDto.RestaurantId))
            {
                return NotFound(new { Message = "Restaurant not found." });
            }

            _mapper.Map(menuItemUpdateDto, existingMenuItem);
            await _menuItemRepository.UpdateAsync(existingMenuItem);

            return NoContent();
        }


        /// <summary>
        /// Partially updates an existing menu item by ID using a JSON patch document.
        /// </summary>
        /// <param name="id">The ID of the menu item to update.</param>
        /// <param name="patchDocument">The JSON patch document containing the updates.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the menu item or restaurant does not exist, or a 400 Bad Request response if the model state is invalid.</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PartiallyUpdateMenuItem(int id, JsonPatchDocument<MenuItemUpdateDto> patchDocument)
        {
            var existingMenuItem = await _menuItemRepository.GetByIdAsync(id);

            if (existingMenuItem is null)
                return NotFound();

            var menuItemToPatch = _mapper.Map<MenuItemUpdateDto>(existingMenuItem);
            patchDocument.ApplyTo(menuItemToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(menuItemToPatch))
                return BadRequest(ModelState);

            if (!await _restaurantRepository.RestaurantExistsAsync(menuItemToPatch.RestaurantId))
            {
                return NotFound(new { Message = "Restaurant not found." });
            }

            _mapper.Map(menuItemToPatch, existingMenuItem);
            await _menuItemRepository.UpdateAsync(existingMenuItem);

            return NoContent();
        }

        /// <summary>
        /// Deletes a menu item by its ID.
        /// </summary>
        /// <param name="id">The ID of the menu item to delete.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the menu item does not exist.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var existingMenuItem = await _menuItemRepository.GetByIdAsync(id);

            if (existingMenuItem == null)
                return NotFound(new { Message = "Menu item not found." });

            await _menuItemRepository.DeleteAsync(id);
            return NoContent();
        }

    }

}

