using FluentValidation;
using RestaurantReservation.API.Models.Orders;

namespace RestaurantReservation.API.Validators.Orders
{
    public class OrderCreationValidator : AbstractValidator<OrderCreationDto>
    {
        public OrderCreationValidator()
        {
            RuleFor(x => x.ReservationId)
                .NotEmpty().WithMessage("Reservation ID is required.");

            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Employee ID is required."); 

            RuleFor(x => x.OrderDate)
                .GreaterThan(DateTime.Now).WithMessage("Order date must be in the future.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("Total amount must be greater than zero.");
        }
    }
}