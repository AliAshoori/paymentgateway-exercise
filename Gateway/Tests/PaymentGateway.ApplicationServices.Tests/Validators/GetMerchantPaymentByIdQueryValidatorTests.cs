using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentGateway.ApplicationServices.Requests;
using PaymentGateway.ApplicationServices.Resources;
using PaymentGateway.ApplicationServices.Validators;

namespace PaymentGateway.ApplicationServices.Tests.Validators
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("APIs.Validators")]
    public class GetMerchantPaymentByIdQueryValidatorTests
    {
        [TestMethod]
        public void Validate_HappyScenario_PassesValidation()
        {
            // Arrange
            var query = new GetMerchantPaymentByIdQuery(merchantId: 919, paymentId: 121);
            var validator = new GetMerchantPaymentByIdQueryValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            result.Errors.Should().HaveCount(0);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void Validate_WithInvalidPaymentId_FailsValidation(long paymentId)
        {
            // Arrange
            var query = new GetMerchantPaymentByIdQuery(merchantId: 909, paymentId);
            var validator = new GetMerchantPaymentByIdQueryValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().BeEquivalentTo(ValidatorErrors.InvalidPaymentId);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void Validate_WithInvalidMerchantId_FailsValidation(long merchantId)
        {
            // Arrange
            var query = new GetMerchantPaymentByIdQuery(merchantId, paymentId: 121);
            var validator = new GetMerchantPaymentByIdQueryValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().BeEquivalentTo(ValidatorErrors.InvalidMerchantId);
        }
    }
}