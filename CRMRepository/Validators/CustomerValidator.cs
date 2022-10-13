using CRMRepository.Entities;
using FluentValidation;

namespace CRMRepository.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.Age)
                .GreaterThanOrEqualTo(18)
                .WithMessage("Age must be 18 or older");
        }
    }
}
