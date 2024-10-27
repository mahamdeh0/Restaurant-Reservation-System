using AutoMapper;
using RestaurantReservation.API.Models.Orders;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderCreationDto, Order>();
            CreateMap<OrderUpdateDto, Order>().ReverseMap();
            CreateMap<Order, OrderWithMenuItemsDto>().ForMember(destination => destination.MenuItems,options => options.MapFrom(src => src.OrderItems.Select(i => i.Item)));

        }
    }
}
