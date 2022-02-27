using FluentValidation;
using PaymentGateway.ApplicationServices.Requests;
using PaymentGateway.ApplicationServices.Resources;

namespace PaymentGateway.ApplicationServices.Validators
{
    public class GetMerchantPaymentByIdQueryValidator : AbstractValidator<GetMerchantPaymentByIdQuery>
    {
        public GetMerchantPaymentByIdQueryValidator()
        {
            RuleFor(r => r.MerchantId).Must(id => id > 0).WithMessage(ValidatorErrors.InvalidMerchantId);
            RuleFor(r => r.PaymentId).Must(id => id > 0).WithMessage(ValidatorErrors.InvalidPaymentId);
        }
    }
}