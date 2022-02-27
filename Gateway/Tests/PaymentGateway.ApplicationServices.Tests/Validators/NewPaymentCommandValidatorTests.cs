using System;
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
    public class NewPaymentCommandValidatorTests
    {
        [TestMethod]
        public void Validate_HappyScenario_PassesValidationRules()
        {
            // Arrange
            var request = new NewPaymentCommand(1, 22.34m, "GBP", "1233 1233 1233 1233", "123", DateTime.Now.AddMonths(9));
            var validator = new NewPaymentCommandValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.Errors.Should().HaveCount(0);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("1234 5648 5321")]
        [DataRow("1234 5648 5321 532A")]
        public void Validate_WithInvalidCardNumber_FailsValidation(string cardNumber)
        {
            // Arrange
            var request = new NewPaymentCommand(1, 22.34m, "GBP", cardNumber, "123", DateTime.Now.AddMonths(9));
            var validator = new NewPaymentCommandValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().Be(ValidatorErrors.InvalidCardNumber);
        }

        [TestMethod]
        public void Validate_WithInvalidCardExpiryDate_FailsValidation()
        {
            // Arrange
            var request = new NewPaymentCommand(1, 22.34m, "GBP", "1233 1233 1233 1233", "123", DateTime.Now.AddMonths(-9));
            var validator = new NewPaymentCommandValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().Be(ValidatorErrors.ExpiredCard);
        }

        [TestMethod]
        [DataRow(" ")]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("44")]
        [DataRow("4A")]
        public void Validate_WithInvalidCvv_FailsValidation(string cvv)
        {
            // Arrange
            var request = new NewPaymentCommand(1, 22.34m, "GBP", "1233 1233 1233 1233", "cvv", DateTime.Now.AddMonths(9));
            var validator = new NewPaymentCommandValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().Be(ValidatorErrors.InvalidCvv);
        }

        [TestMethod]
        [DataRow(" ")]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("USD1")]
        [DataRow("YEN1")]
        public void Validate_WithInvalidCurrency_FailsValidation(string currency)
        {
            // Arrange
            var request = new NewPaymentCommand(1, 22.34m, currency, "1233 1233 1233 1233", "123", DateTime.Now.AddMonths(9));
            var validator = new NewPaymentCommandValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().Be(ValidatorErrors.InvalidCurrency);
        }

        [TestMethod]
        public void Validate_WithInvalidAmount_FailsValidation()
        {
            // Arrange
            var request = new NewPaymentCommand(1, -22.34m, "GBP", "1233 1233 1233 1233", "123", DateTime.Now.AddMonths(9));
            var validator = new NewPaymentCommandValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().Be(ValidatorErrors.InvalidAmount);
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public void Validate_WithInvalidMerchantId_FailsValidation(long merchantId)
        {
            // Arrange
            var request = new NewPaymentCommand(merchantId, 22.34m, "GBP", "1233 1233 1233 1233", "123", DateTime.Now.AddMonths(9));
            var validator = new NewPaymentCommandValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().Be(ValidatorErrors.InvalidMerchantId);
        }
    }
}
