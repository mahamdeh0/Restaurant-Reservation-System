using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Customers;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

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

        /// <summary>
        /// Retrieves a customer by their ID.
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve.</param>
        /// <returns>A customer DTO if found; otherwise, a 404 Not Found response.</returns>
        [HttpGet("{id}", Name = "GetCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(_mapper.Map<CustomerDto>(customer));
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="customerForCreationDto">The customer data to create.</param>
        /// <returns>The created customer DTO.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerCreationDto customerForCreationDto)
        {

            var customer = _mapper.Map<Customer>(customerForCreationDto);
            var addedCustomer = await _customerRepository.CreateAsync(customer);
            var createdCustomerReturn = _mapper.Map<CustomerDto>(addedCustomer);

            return CreatedAtRoute("GetCustomer", new { id = addedCustomer.CustomerId }, createdCustomerReturn);
        }

        /// <summary>
        /// Updates an existing customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer to update.</param>
        /// <param name="customerUpdateDto">The updated customer data.</param>
        /// <returns>No content if successful; otherwise, a 404 Not Found response.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerUpdateDto customerUpdateDto)
        {

            var existingCustomer = await _customerRepository.GetByIdAsync(id);

            if (existingCustomer == null)
                return NotFound();

            _mapper.Map(customerUpdateDto, existingCustomer);
            await _customerRepository.UpdateAsync(existingCustomer);

            return NoContent();
        }

        /// <summary>
        /// Partially updates an existing customer by ID using a JSON patch document.
        /// </summary>
        /// <param name="id">The ID of the customer to update.</param>
        /// <param name="patchDocument">The JSON patch document containing updates.</param>
        /// <returns>No content if successful; otherwise, a 404 Not Found response.</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PartiallyUpdateCustomer(int id, JsonPatchDocument<CustomerUpdateDto> patchDocument)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(id);

            if (existingCustomer == null)
                return NotFound();

            var customerToPatch = _mapper.Map<CustomerUpdateDto>(existingCustomer);
            patchDocument.ApplyTo(customerToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(customerToPatch))
                return BadRequest(ModelState);

            _mapper.Map(customerToPatch, existingCustomer);
            await _customerRepository.UpdateAsync(existingCustomer);

            return NoContent();
        }

        /// <summary>
        /// Deletes a customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>No content if successful; otherwise, a 404 Not Found response.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(id);
            if (existingCustomer == null)
                return NotFound();

            await _customerRepository.DeleteAsync(id);
            return NoContent();
        }


    }
}
