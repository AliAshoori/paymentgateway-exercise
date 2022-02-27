using System;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentGateway.ApplicationServices.Mappers;
using PaymentGateway.ApplicationServices.Requests;
using PaymentGateway.Domain.DTOs;

namespace PaymentGateway.ApplicationServices.Tests.Mappers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("APIs.Mappers")]
    public class AcquireBankPaymentProfileTests
    {
        [TestMethod]
        public void AcquireBankPaymentProfile_HappyScenario_ReturnsPaymentModel()
        {
            // Arrange
            var command = new NewPaymentCommand(1, 22.34m, "GBP", "1233 1233 1233 1233", "123", DateTime.Now.AddMonths(9));
            var expected = new BankPaymentProcessRequest
            {
                Amount   = command.Amount,
                CardCvv = command.CardCvv,
                CardExpiry = command.CardExpiry,
                CardNumber = command.CardNumber,
                Currency = command.Currency,
                MerchantId = command.MerchantId
            };

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AcquireBankPaymentProfile>());
            var mapper = config.CreateMapper();

            // Act
            var paymentResult = mapper.Map<BankPaymentProcessRequest>(command);

            // Assert
            paymentResult.Should().BeEquivalentTo(expected);
        }
    }
}