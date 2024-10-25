using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Employees;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeRepository employeeRepository, IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _restaurantRepository = restaurantRepository;
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

    }
}
