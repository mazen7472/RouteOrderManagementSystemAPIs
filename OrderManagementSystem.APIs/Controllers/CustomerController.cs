using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.APIs.Errors;
using OrderManagementSystem.Core.DTOs;
using OrderManagementSystem.Core.Entites;
using OrderManagementSystem.Core.Services;

namespace OrderManagementSystem.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;


        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> CreateCustomer([FromBody] CustomerDTO customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(404, "Not a Valid Customer"));
            }

            var createdCustomer = await _customerService.CreateCustomerAsync(customerDto);
            return Ok(createdCustomer);
        }

        [HttpGet("{customerId}/orders")]
        public async Task<ActionResult<OrderDTO>> GetCustomerOrders(int customerId)
        {
            var orders = await _customerService.GetCustomerOrdersAsync(customerId);
            if (!orders.Any())
            {
                return NotFound(new ApiResponse(404 , $"No Customer Orders with ID {customerId}"));
            }

            return Ok(orders);
        }
    }
}
