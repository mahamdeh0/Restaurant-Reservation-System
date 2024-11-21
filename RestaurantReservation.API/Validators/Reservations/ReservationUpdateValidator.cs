using FluentValidation;
using RestaurantReservation.API.Models.Reservations;

namespace RestaurantReservation.API.Validators.Reservations
{
    public class ReservationUpdateValidator : AbstractValidator<ReservationUpdateDto>
    {
        public ReservationUpdateValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required."); 

            RuleFor(x => x.RestaurantId)
                .NotEmpty().WithMessage("Restaurant ID is required."); 

            RuleFor(x => x.TableId)
                .NotEmpty().WithMessage("Table ID is required.");

            RuleFor(x => x.PartySize)
                .GreaterThan(0).WithMessage("Party size must be greater than zero.");
        }
    }
}