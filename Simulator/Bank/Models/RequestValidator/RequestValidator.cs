using System;
using FluentValidation;
using Bank.Models.Request;

namespace Bank.Models.RequestValidator
{
    public class RequestValidator : AbstractValidator<PaymentRequest>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("Amount Required")
            .GreaterThanOrEqualTo(1)
            .WithMessage("Amount must be greater then 0")
            .LessThan(Double.MaxValue)
            .WithMessage($"Your amount cannot be greater than {Double.MaxValue}");

            RuleFor(x => x.CardNumber)
            .NotEmpty()
            .WithMessage("Card Number Required")
            .Length(1,19)
            .WithMessage("Card Number length must be in between 1 and 19");

            RuleFor(x => x.CVV)
            .NotEmpty()
            .WithMessage("CVV Required")
            .GreaterThan(0)
            .WithMessage("CVV value has be greater than 0")
            .LessThan(1000)
            .WithMessage("CVV value cannot be greater than 999");

            RuleFor(x => x.ExpiryYear)
            .NotEmpty()
            .WithMessage("ExpiryYear Required")
            .GreaterThanOrEqualTo(2021)
            .WithMessage("ExpiryYear must be greater than or equal to 2021")
            .LessThanOrEqualTo(2999)
            .WithMessage($"Expiry year cannot be greater than or equal to 2999");

            RuleFor(x => x.ExpiryMonth)
            .NotEmpty()
            .WithMessage("ExpiryMonth Required")
            .GreaterThanOrEqualTo(1)
            .WithMessage("ExpiryMonth must be greater or equal to 1")
            .LessThanOrEqualTo(12)
            .WithMessage($"ExpiryMonth must be less than or equal to 1");

            RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .WithMessage("CurrencyCode Required")
            .Length(3)
            .WithMessage("Currency Code must be a valid ISO 4217 code");
        }
    }
}