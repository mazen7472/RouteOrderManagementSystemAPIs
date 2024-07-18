using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.DTOs
{
    public class OrderCreateDTO
    {
        public int CustomerId { get; set; }
        public List<OrderItemCreateDTO> OrderItems { get; set; }
        public string PaymentMethod { get; set; }
    }
}
