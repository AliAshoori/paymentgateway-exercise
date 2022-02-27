using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentGateway.ApplicationServices.Mappers;
using PaymentGateway.ApplicationServices.Responses;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.ApplicationServices.Tests.Mappers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("APIs.Mappers")]
    public class GetPaymentProfileTests
    {
        [TestMethod]
        public void GetPaymentProfile_HappyScenario_ReturnGetPaymentResponse()
        {
            // Arrange
            var payment = new Payment
            {
                MerchantId = 1001,
                Id = 300,
                Succeeded = true,
                CardNumber = "1233 1233 1233 1233",
                Currency = "GBP",
                Amount = 3455.234m,
                CardCvv = "123",
                CreatedAt = DateTime.Now
            };

            var expectedResponse = new GetMerchantPaymentByIdQueryResponse
            {
                MerchantId = payment.MerchantId,
                PaymentId = payment.Id,
                Succeeded = payment.Succeeded,
                CardNumber = "****",
                CardCvv = "****",
                Currency = payment.Currency,
                Amount = payment.Amount,
                CreatedAt = payment.CreatedAt
            };

            var config = new MapperConfiguration(cfg => cfg.AddProfile<GetPaymentProfile>());
            var mapper = config.CreateMapper();

            // Act
            var paymentResult = mapper.Map<GetMerchantPaymentByIdQueryResponse>(payment);

            // Assert
            paymentResult.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
