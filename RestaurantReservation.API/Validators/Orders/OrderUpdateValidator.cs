using FluentValidation;
using RestaurantReservation.API.Models.Orders;

namespace RestaurantReservation.API.Validators.Orders
{
    public class OrderUpdateValidator : AbstractValidator<OrderUpdateDto>
    {
        public OrderUpdateValidator()
        {
            RuleFor(x => x.ReservationId)
                .NotEmpty().WithMessage("Reservation ID is required."); 

            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Employee ID is required."); 

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("Total amount must be greater than zero.");
        }
    }
}