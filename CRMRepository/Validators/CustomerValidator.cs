using CRMRepository.Entities;
using FluentValidation;
using System;

namespace CRMRepository.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.Id)
                .NotEmpty()
                .WithMessage("Id is required");
            RuleFor(customer => customer.FirstName)
                .NotEmpty()
                .WithMessage("FirstName is required");
            RuleFor(customer => customer.LastName)
                .NotEmpty()
                .WithMessage("LastName is required");

            RuleFor(customer => customer.DateOfBirth)
                .NotEmpty()
                .WithMessage("DateOfBirth is required");

            RuleFor(customer => customer.DateOfBirth)
               .LessThanOrEqualTo(DateTime.Now.AddYears(-18))
                .WithMessage("Age must be 18 or older");
        }
    }
}
