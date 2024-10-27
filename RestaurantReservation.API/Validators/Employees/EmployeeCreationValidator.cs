using FluentValidation;
using RestaurantReservation.API.Models.Employees;

namespace RestaurantReservation.API.Validators.Employees
{
    public class EmployeeCreationValidator : AbstractValidator<EmployeeCreationDto>
    {
        public EmployeeCreationValidator()
        {
            RuleFor(x => x.RestaurantId)
                .NotEmpty().WithMessage("Restaurant ID must be greater than zero.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

            RuleFor(x => x.Position)
                .IsInEnum().WithMessage("Position must be a valid enum value.");
        }
    }
}