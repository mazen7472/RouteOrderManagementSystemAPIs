using AutoMapper;
using OrderManagementSystem.Core.DTOs;
using OrderManagementSystem.Core.Entites;
using OrderManagementSystem.Core.Repositories;
using OrderManagementSystem.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo _customerRepository;
        private readonly IOrderRepo _orderRepository;
        private readonly IMapper _map;

        public CustomerService(ICustomerRepo customerRepository, IOrderRepo orderRepository, IMapper map)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _map = map;
        }

        public async Task<Customer> CreateCustomerAsync(CustomerDTO customerDto)
        {
            if (string.IsNullOrEmpty(customerDto.Name))
            {
                throw new ArgumentException("Customer name is required.");
            }

            var customer = new Customer
            {
                Name = customerDto.Name,
                Email = customerDto.Email
            };
            await _customerRepository.AddCustomerAsync(customer);
            await _customerRepository.SaveChangesAsync();
            return customer;


        }

        public async Task<List<OrderDTO>> GetCustomerOrdersAsync(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
            return _map.Map<List<OrderDTO>>(orders);
        }
    }
}
