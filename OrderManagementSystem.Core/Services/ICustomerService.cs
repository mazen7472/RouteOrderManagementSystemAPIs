using OrderManagementSystem.Core.DTOs;
using OrderManagementSystem.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OrderManagementSystem.Core.Services
{
    public interface ICustomerService
    {

   
        Task<Customer> CreateCustomerAsync(CustomerDTO customerDto);
        Task<List<OrderDTO>> GetCustomerOrdersAsync(int customerId);

    }
}
