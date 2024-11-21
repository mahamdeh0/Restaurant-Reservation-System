using FluentValidation;
using RestaurantReservation.API.Models.Customers;

namespace RestaurantReservation.API.Validators.Customers
{
    public class CustomerUpdateValidator : AbstractValidator<CustomerUpdateDto>
    {
        public CustomerUpdateValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?\d{10,15}$").WithMessage("Phone number must be between 10 and 15 digits and can include a leading +.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber)); 
        }
    }
}
