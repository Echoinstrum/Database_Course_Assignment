using Business.Models;
using Business.Services;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Database_Course_Assignment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomersController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        //Mixed help from Microsoft - web-api/action-return-types directly under Asynchronus action - where they had a simular method
        //And ChatGpt who had to help me understand the "CreatedAtAction(nameof(GetCustomers), new { id = newCustomer.Id}, newCustomer part.
        //CreatedAtAction gives a HTTP-respons telling me a new resource has been added(201 Created-status)
        public async Task<ActionResult<CustomerEntity>> CreateCustomer(CustomerRegistrationForm customerRegistrationForm)
        {
            var newCustomer = await _customerService.CreateCustomerAsync(customerRegistrationForm);
            if(newCustomer == null)
            {
                return BadRequest("Error in HttpPost: Could not create customer");
            }
            return CreatedAtAction( //CreatedAtAction returns a 201 -created-status and generates a URL to GetCustomer-method with the new customers Id
                nameof(GetCustomers), // THe name of the method that should be used to get Customers
                new { id = newCustomer.Id }, //Creates a new object with the new customers Id
                newCustomer); // Returns the created customer as a part of the HTTP-response
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetCustomers()
        {
            var customers = await _customerService.GetCustomersAsync();
            return Ok(customers);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerModel customerModel)
        {
            if (id != customerModel.Id)
            {
                return BadRequest("Customer Id not matching");
            }

            var updatedCustomer = await _customerService.UpdateCustomerAsync(customerModel);
            if (!updatedCustomer)
            {
                return BadRequest("Customer wasn't found");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var deletedCustomer = await _customerService.DeleteCustomerAsync(id);
            if (!deletedCustomer)
            {
                return NotFound("Customer wasn't found");
            }

            return NoContent();
        }
    }
}
