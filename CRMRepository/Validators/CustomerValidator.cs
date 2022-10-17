using CRMRepository.Entities;
using FluentValidation;
using System;

namespace CRMRepository.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.DateOfBirth)
                .LessThanOrEqualTo(DateTime.Now.AddYears(-18))
                .WithMessage("Age must be 18 or older");
        }
    }
}
