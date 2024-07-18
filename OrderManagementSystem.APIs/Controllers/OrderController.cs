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
    public class OrderController : ControllerBase
    {
        private IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO orderCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdOrder = await _orderService.CreateOrderAsync(orderCreateDto);
                return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.OrderId }, createdOrder);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{orderId}")]
        
        public async Task<ActionResult<OrderDTO>> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            if (!orders.Any())
            {
                return NotFound(new ApiResponse(404, $"Not Found!"));
            }
            return Ok(orders);
        }

        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest(new ApiResponse(404, "Status is Not Valid!"));
            }

            await _orderService.UpdateOrderStatusAsync(orderId, status);
            return Ok($"Status updated to {status}");
        }
    }
}
