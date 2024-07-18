﻿using AutoMapper;
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
    public class InvoiceService:IInvoiceService
    {
        private readonly IInvoiceRepo _invoiceRepository;
        private readonly IOrderRepo _orderRepository;
        private readonly IMapper _mapper;

        public InvoiceService(IInvoiceRepo invoiceRepository, IOrderRepo orderRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task GenerateInvoiceAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }

            var invoice = new Invoice
            {
                OrderId = orderId,
                InvoiceDate = DateTime.Now,
                TotalAmount = order.TotalAmount,
                Order = await _orderRepository.GetOrderByIdAsync(orderId),
            };

            await _invoiceRepository.AddInvoiceAsync(invoice);
            await _invoiceRepository.SaveChangesAsync();
        }

        public async Task<InvoiceDTO> GetInvoiceByIdAsync(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(invoiceId);

            return _mapper.Map<InvoiceDTO>(invoice);
        }

        public async Task<IEnumerable<InvoiceDTO>> GetInvoicesAsync()
        {
            var invoice = await _invoiceRepository.GetInvoicesAsync();
            return _mapper.Map<IEnumerable<InvoiceDTO>>(invoice);
        }
    }
}
