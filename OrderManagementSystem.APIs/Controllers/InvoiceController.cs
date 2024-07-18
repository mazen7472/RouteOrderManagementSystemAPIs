using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.APIs.Errors;
using OrderManagementSystem.Core.DTOs;
using OrderManagementSystem.Core.Entites;
using OrderManagementSystem.Core.Services;
using OrderManagementSystem.Services;

namespace OrderManagementSystem.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
            
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<InvoiceDTO>> GetInvoiceById(int id)
        { 
            var Invoice= await _invoiceService.GetInvoiceByIdAsync(id);
            if (Invoice is null)
            {
                return NotFound(new ApiResponse(404, $"No Invoice with ID {id}"));
            }
            return Ok(Invoice);
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<IEnumerable<InvoiceDTO>>> GetAllInvoices()
        {
            var Invoices = await _invoiceService.GetInvoicesAsync();
            if (!Invoices.Any())
            {
                return NotFound(new ApiResponse(404, $"Not Found!"));
            }
            return Ok(Invoices);
        }

    }
}
