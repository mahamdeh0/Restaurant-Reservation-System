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
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
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

    }
}
