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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepository;
        private readonly IProductRepo _productRepository;
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;
        private readonly ICustomerRepo _customerRepo;
        private readonly IInvoiceRepo  _invoiceRepo;
        private readonly IEmailSenderService _emailService;

        public OrderService(IOrderRepo orderRepository, IProductRepo productRepository, IInvoiceService invoiceService, IInvoiceRepo invoiceRepo, IMapper mapper, ICustomerRepo customerRepo, IEmailSenderService emailService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _invoiceService = invoiceService;
            _invoiceRepo = invoiceRepo;
            _mapper = mapper;
            _customerRepo = customerRepo;
            _emailService = emailService;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var order = await _orderRepository.GetOrdersAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(order);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            return _mapper.Map<OrderDTO>(order);

        }



        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }

            order.Status = status;
            await _orderRepository.UpdateOrderAsync(order);
            await _orderRepository.SaveChangesAsync();

            // Send Email Logic
            var customer = await _customerRepo.GetCustomerByIdAsync(order.CustomerId);
            var subject = $"Order ID({orderId}) status";
            var body = $"Order ID({orderId}) status has been updated to {order.Status}";
            await _emailService.SendEmailAsync(customer.Email, subject, body);
            
            
        }
        public async Task<OrderDTO> CreateOrderAsync(OrderCreateDTO orderCreateDto)
        {
            var order = new Order
            {
                CustomerId = orderCreateDto.CustomerId,
                OrderDate = DateTime.UtcNow,
                PaymentMethod = orderCreateDto.PaymentMethod,
                Status = "Pending",
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach (var item in orderCreateDto.OrderItems)
            {
                var customer = await _customerRepo.GetCustomerByIdAsync(orderCreateDto.CustomerId);
                if (customer == null)
                {
                    throw new Exception($"Customer with ID {orderCreateDto.CustomerId} does not exist.");
                }
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                {
                    throw new Exception($"Product with ID {item.ProductId} not available or insufficient stock.");
                }

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    Discount = item.Discount
                };

                totalAmount += (orderItem.UnitPrice - orderItem.Discount) * orderItem.Quantity;
                order.OrderItems.Add(orderItem);

                // Update the stock
                product.Stock -= item.Quantity;
            }

            // Apply tiered discounts based on the total amount
            if (totalAmount > 200)
            {
                totalAmount *= 0.90m; // 10% discount
            }
            else if (totalAmount > 100)
            {
                totalAmount *= 0.95m; // 5% discount
            }

            order.TotalAmount = totalAmount;

            await _orderRepository.AddOrderAsync(order);
            await _orderRepository.SaveChangesAsync();
            await _invoiceService.GenerateInvoiceAsync(order.OrderId);
            await _invoiceRepo.SaveChangesAsync();

            return _mapper.Map<OrderDTO>(order);
        }



    }
}
