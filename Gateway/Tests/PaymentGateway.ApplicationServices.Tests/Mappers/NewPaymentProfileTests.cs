using System;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentGateway.ApplicationServices.Mappers;
using PaymentGateway.ApplicationServices.Requests;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.ApplicationServices.Tests.Mappers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("APIs.Mappers")]
    public class NewPaymentProfileTests
    {
        [TestMethod]
        public void NewPaymentProfile_HappyScenario_ReturnsPaymentModel()
        {
            // Arrange
            var command = new NewPaymentCommand(1, 22.34m, "GBP", "1233 1233 1233 1233", "123", DateTime.Now.AddMonths(9));
            var expected = new Payment
            {
                Amount   = command.Amount,
                CardCvv = command.CardCvv,
                CardExpiry = command.CardExpiry,
                CardNumber = command.CardNumber,
                Currency = command.Currency,
                MerchantId = command.MerchantId
            };

            var config = new MapperConfiguration(cfg => cfg.AddProfile<NewPaymentProfile>());
            var mapper = config.CreateMapper();

            // Act
            var paymentResult = mapper.Map<Payment>(command);

            // Assert
            paymentResult.Should().BeEquivalentTo(expected);
        }
    }
}