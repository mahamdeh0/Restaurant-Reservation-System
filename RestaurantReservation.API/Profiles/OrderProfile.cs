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


        }
    }
}
