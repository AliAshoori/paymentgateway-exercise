using System;
using System.Globalization;
using System.Linq;
using FluentValidation;
using PaymentGateway.ApplicationServices.Requests;
using PaymentGateway.ApplicationServices.Resources;

namespace PaymentGateway.ApplicationServices.Validators
{
    public class NewPaymentCommandValidator : AbstractValidator<NewPaymentCommand>
    {
        public NewPaymentCommandValidator()
        {
            RuleFor(r => r.CardNumber).Must(cardNumber =>
            {
                const int expectedDigits = 16;
                const int expectedWhiteSpaces = 3;

                return !string.IsNullOrWhiteSpace(cardNumber) &&
                       cardNumber.Length == expectedDigits + expectedWhiteSpaces &&
                       cardNumber.All(c => char.IsDigit(c) || char.IsWhiteSpace(c)) &&
                       cardNumber.Count(char.IsDigit) == expectedDigits &&
                       cardNumber.Count(char.IsWhiteSpace) == expectedWhiteSpaces;

            }).WithMessage(ValidatorErrors.InvalidCardNumber);

            RuleFor(r => r.CardExpiry).Must(cardExpiry => cardExpiry.Date > DateTime.Now.Date)
                .WithMessage(ValidatorErrors.ExpiredCard);

            RuleFor(r => r.CardCvv).Must(cvv =>
            {
                const int expectedCvvLength = 3;

                return !string.IsNullOrWhiteSpace(cvv) &&
                       cvv.Length == expectedCvvLength &&
                       cvv.All(char.IsDigit);

            }).WithMessage(ValidatorErrors.InvalidCvv);

            RuleFor(r => r.MerchantId).Must(merchantId => merchantId > 0)
                .WithMessage(ValidatorErrors.InvalidMerchantId);

            RuleFor(r => r.Amount).Must(amount => amount > 0)
                .WithMessage(ValidatorErrors.InvalidAmount);

            RuleFor(r => r.Currency).Must(currency =>
            {
                if (string.IsNullOrWhiteSpace(currency))
                {
                    return false;
                }

                var currencySymbols = CultureInfo.GetCultures(CultureTypes.AllCultures)
                    .Where(c => !c.IsNeutralCulture)
                    .Select(culture =>
                    {
                        try
                        {
                            return new RegionInfo(culture.Name);
                        }
                        catch
                        {
                            return null;
                        }
                    })
                    .Where(ri => ri != null)
                    .GroupBy(ri => ri.ISOCurrencySymbol)
                    .ToDictionary(x => x.Key, x => x.First().CurrencySymbol);


                return currencySymbols.TryGetValue(currency, out _);

            }).WithMessage(ValidatorErrors.InvalidCurrency);
        }
    }
}
