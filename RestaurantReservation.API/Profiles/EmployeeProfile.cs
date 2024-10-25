using AutoMapper;
using RestaurantReservation.API.Models.Employees;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Models.Enum;

namespace RestaurantReservation.API.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => ((EmployeePosition)src.Position).ToString())); ;

        }
    }
}
