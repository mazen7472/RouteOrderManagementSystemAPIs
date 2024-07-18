﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Entites
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; } = "Pending";
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
