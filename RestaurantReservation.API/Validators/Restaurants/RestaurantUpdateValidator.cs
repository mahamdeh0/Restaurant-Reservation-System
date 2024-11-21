using FluentValidation;
using RestaurantReservation.API.Models.Restaurants;

namespace RestaurantReservation.API.Validators.Restaurants
{
    public class RestaurantUpdateValidator : AbstractValidator<RestaurantUpdateDto>
    {
        public RestaurantUpdateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Restaurant name is required.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be in a valid format."); 

            RuleFor(x => x.OpeningHours)
                .NotEmpty().WithMessage("Opening hours are required.");
        }
    }
}