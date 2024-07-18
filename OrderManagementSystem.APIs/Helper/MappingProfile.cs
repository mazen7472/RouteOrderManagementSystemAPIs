using AutoMapper;
using OrderManagementSystem.Core.DTOs;
using OrderManagementSystem.Core.Entites;

namespace OrderManagementSystem.APIs.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Customer, CustomerDTO>().ReverseMap();


            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<ProductDTO, ProductCreateDTO>().ReverseMap();


            CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => CalculateOrderTotal(src.OrderItems)));

            CreateMap<OrderCreateDTO, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItemCreateDTO, OrderItem>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<Invoice, InvoiceDTO>().ReverseMap();


        }
        private decimal CalculateOrderTotal(List<OrderItem> orderItems)
        {


            var orders = orderItems.Sum(item => (item.Quantity * item.UnitPrice) - item.Discount);

            if (orders > 200)
            {
                orders = orders * 0.90m;
            }
            else if (orders > 100)
            {
                orders = orders * 0.95m;

            }
            return orders;
        }

    }
}
