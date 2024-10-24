using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Customers;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/Customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepository customerRepository, IMapper Mapper)
        {
            _customerRepository = customerRepository;
            _mapper = Mapper;
        }

        /// <summary>
        /// Gets all customers.
        /// </summary>
        /// <returns>A list of customer DTOs.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllAsync();

            if (customers == null || !customers.Any())
            {
                return NoContent();
            }

            return Ok(_mapper.Map<IEnumerable<CustomerDto>>(customers));
        }


    }
}
