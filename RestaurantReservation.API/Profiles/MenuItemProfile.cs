﻿using AutoMapper;
using RestaurantReservation.API.Models.MenuItems;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Profiles
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItem, MenuItemDto>();
            CreateMap<MenuItemCreationDto, MenuItem>();
            CreateMap<MenuItemUpdateDto, MenuItem>().ReverseMap();

        }
    }
}
