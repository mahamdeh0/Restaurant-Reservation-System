using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Tables;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/tables")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly ITableRepository _tableRepository;

        private readonly IRestaurantRepository _restaurantRepository;

        private readonly IMapper _mapper;

        public TablesController(ITableRepository tableRepository, IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _tableRepository = tableRepository;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of all tables.
        /// </summary>
        /// <returns>A list of table DTOs.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TableDto>>> GetTables()
        {
            var tables = await _tableRepository.GetAllAsync();

            if (tables == null || !tables.Any())
                return NoContent();

            return Ok(_mapper.Map<IEnumerable<TableDto>>(tables));
        }

        /// <summary>
        /// Retrieves a table by its ID.
        /// </summary>
        /// <param name="id">The ID of the table to retrieve.</param>
        /// <returns>The table DTO if found; otherwise, a 404 Not Found response.</returns>
        [HttpGet("{id}", Name = "GetTable")]
        public async Task<ActionResult<TableDto>> GetTable(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);

            if (table == null)
                return NotFound();

            return Ok(_mapper.Map<TableDto>(table));
        }

    }
}
