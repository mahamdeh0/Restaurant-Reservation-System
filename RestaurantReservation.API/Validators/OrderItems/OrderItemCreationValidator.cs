using FluentValidation;
using RestaurantReservation.API.Models.OrderItems;

namespace RestaurantReservation.API.Validators.OrderItems
{
    public class OrderItemCreationValidator : AbstractValidator<OrderItemCreationDto>
    {
        public OrderItemCreationValidator()
        {
            RuleFor(x => x.ItemId)
                .NotEmpty().WithMessage("Item ID must be greater than zero.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}