using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Employees;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/employees")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeRepository employeeRepository, IRestaurantRepository restaurantRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _restaurantRepository = restaurantRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        /// <returns>A list of employee DTOs.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var allEmployees = await _employeeRepository.GetAllAsync();

            if (allEmployees == null || !allEmployees.Any())
            {
                return NoContent();
            }

            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(allEmployees));
        }

        /// <summary>
        /// Retrieves an employee by their ID.
        /// </summary>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>The employee DTO if found; otherwise, a 404 Not Found response.</returns>
        [HttpGet("{id}", Name = "GetEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        /// <summary>
        /// Retrieves a list of managers.
        /// </summary>
        /// <returns>A list of employee DTOs representing managers.</returns>
        [HttpGet("managers")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetManagers()
        {
            var employees = await _employeeRepository.ListManagersAsync();

            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
        }

        /// <summary>
        /// Calculates the average order amount handled by a specific employee.
        /// </summary>
        /// <param name="employeeId">The ID of the employee for whom to calculate the average order amount.</param>
        /// <returns>A 200 OK response with the average order amount; or a 404 Not Found response if no orders exist for the specified employee.</returns>
        [HttpGet("{employeeId}/average-order-amount")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAverageOrderAmount(int employeeId)
        {
            try
            {
                var averageOrderAmount = await _orderRepository.CalculateAverageOrderAmountAsync(employeeId);

                return Ok(new { averageOrderAmount });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employeeCreationDto">The employee creation data.</param>
        /// <returns>The created employee DTO.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(EmployeeCreationDto employeeCreationDto)
        {
            if (!await _restaurantRepository.RestaurantExistsAsync(employeeCreationDto.RestaurantId))
            {
                return NotFound(new { Message = "Restaurant not found." });
            }

            var employeeToAdd = _mapper.Map<Employee>(employeeCreationDto);
            var addedEmployee = await _employeeRepository.CreateAsync(employeeToAdd);
            var createdEmployeeReturn = _mapper.Map<EmployeeDto>(addedEmployee);

            return CreatedAtRoute(
                "GetEmployee",
                new { id = addedEmployee.EmployeeId },
                createdEmployeeReturn
            );
        }

        /// <summary>
        /// Updates an existing employee by ID.
        /// </summary>
        /// <param name="id">The ID of the employee to update.</param>
        /// <param name="employeeUpdateDto">The employee update data.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the employee does not exist.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeUpdateDto employeeUpdateDto)
        {

            var existingEmployee = await _employeeRepository.GetByIdAsync(id);

            if (existingEmployee == null)
                return NotFound();

            if (!await _restaurantRepository.RestaurantExistsAsync(employeeUpdateDto.RestaurantId))
            {
                return NotFound(new { Message = "Restaurant not found." });
            }

            _mapper.Map(employeeUpdateDto, existingEmployee);
            await _employeeRepository.UpdateAsync(existingEmployee);

            return NoContent();
        }

        /// <summary>
        /// Partially updates an existing employee by ID.
        /// </summary>
        /// <param name="id">The ID of the employee to update.</param>
        /// <param name="patchDocument">The patch document containing the updates.</param>
        /// <returns>A 204 No Content response if successful; a 404 Not Found if the employee or restaurant does not exist; or a 400 Bad Request if the patch document is invalid.</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PartiallyUpdateEmployee(int id, JsonPatchDocument<EmployeeUpdateDto> patchDocument)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(id);

            if (existingEmployee is null)
            {
                return NotFound();
            }

            var employeeToPatch = _mapper.Map<EmployeeUpdateDto>(existingEmployee);

            patchDocument.ApplyTo(employeeToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(employeeToPatch))
                return BadRequest(ModelState);

            if (!await _restaurantRepository.RestaurantExistsAsync(employeeToPatch.RestaurantId))
            {
                return NotFound(new { Message = "Restaurant not found." });
            }

            _mapper.Map(employeeToPatch, existingEmployee);

            await _employeeRepository.UpdateAsync(existingEmployee);

            return NoContent();
        }

        /// <summary>
        /// Deletes a customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>A 204 No Content response if successful; otherwise, a 404 Not Found response if the customer does not exist.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _employeeRepository.GetByIdAsync(id);
            if (customer == null)
                return NotFound();

            await _employeeRepository.DeleteAsync(id);
            return NoContent();
        }


    }
}
