using FluentValidation;
using RestaurantReservation.API.Models.MenuItems;

namespace RestaurantReservation.API.Validators.MenuItems
{
    public class MenuItemCreationValidator : AbstractValidator<MenuItemCreationDto>
    {
        public MenuItemCreationValidator()
        {
            RuleFor(x => x.RestaurantId)
                .NotEmpty().WithMessage("Restaurant ID must be greater than zero.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .Length(5, 500).WithMessage("Description must be between 5 and 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
}