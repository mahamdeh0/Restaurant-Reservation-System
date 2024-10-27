using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Tables;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

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

        /// <summary>
        /// Creates a new table.
        /// </summary>
        /// <param name="tableCreationDto">The table creation data.</param>
        /// <returns>A 201 Created response with the created table DTO if successful; otherwise, a 404 Not Found response if the restaurant does not exist.</returns>
        [HttpPost]
        public async Task<ActionResult<TableDto>> CreateTable(TableCreationDto tableCreationDto)
        {
            if (!await _restaurantRepository.RestaurantExistsAsync(tableCreationDto.RestaurantId))
                return NotFound(new { Message = "Restaurant not found." });

            var tableToAdd = _mapper.Map<Table>(tableCreationDto);

            var addedTable = await _tableRepository.CreateAsync(tableToAdd);

            var tableDto = _mapper.Map<TableDto>(addedTable);

            return CreatedAtRoute(
                "GetTable",
                new { id = addedTable.TableId },
                tableDto
            );
        }

        /// <summary>
        /// Updates an existing table.
        /// </summary>
        /// <param name="id">The ID of the table to update.</param>
        /// <param name="tableUpdateDto">The updated table data.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the table or restaurant does not exist.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTable(int id, TableUpdateDto tableUpdateDto)
        {
            var table = await _tableRepository.GetByIdAsync(id);

            if (table == null)
                return NotFound();

            if (!await _restaurantRepository.RestaurantExistsAsync(tableUpdateDto.RestaurantId))
                return NotFound(new { Message = "Restaurant not found." });

            _mapper.Map(tableUpdateDto, table);
            await _tableRepository.UpdateAsync(table);

            return NoContent();
        }

        /// <summary>
        /// Partially updates an existing table.
        /// </summary>
        /// <param name="id">The ID of the table to update.</param>
        /// <param name="patchDocument">The JSON patch document containing the updates.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the table or restaurant does not exist.</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateTable(int id, JsonPatchDocument<TableUpdateDto> patchDocument)
        {
            var table = await _tableRepository.GetByIdAsync(id);

            if (table == null)
                return NotFound();

            var tableToPatch = _mapper.Map<TableUpdateDto>(table);
            patchDocument.ApplyTo(tableToPatch, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(tableToPatch))
                return BadRequest(ModelState);

            if (!await _restaurantRepository.RestaurantExistsAsync(tableToPatch.RestaurantId))
                return NotFound(new { Message = "Restaurant not found." });

            _mapper.Map(tableToPatch, table);
            await _tableRepository.UpdateAsync(table);

            return NoContent();
        }

        /// <summary>
        /// Deletes a table by its ID.
        /// </summary>
        /// <param name="id">The ID of the table to delete.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the table does not exist.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);

            if (table == null)
                return NotFound(new { Message = "Table not found." });

            await _tableRepository.DeleteAsync(id);

            return NoContent();
        }

    }
}
