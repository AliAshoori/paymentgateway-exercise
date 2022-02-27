using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PaymentGateway.ApplicationServices.Responses;
using PaymentGateway.Domain.Models;
using PaymentGateway.Infrastructure.Repositories;

namespace PaymentGateway.APIs.Tests.Integrations
{
    [ExcludeFromCodeCoverage]
    [TestCategory("APIs.Integrations")]
    [TestClass]
    public class GetMerchantPaymentByIdIntegrationTests
    {
        private TestsWebApplicationFactory<Startup> _factory;

        [TestMethod]
        public async Task GetMerchantPaymentByIdAsync_HappyScenario_ReturnsPayment()
        {
            // Arrange
            const long merchantId = 110;

            var payment = new Payment
            {
                MerchantId = merchantId,
                Succeeded = true,
                CardNumber = "1234 1234 1234 1234",
                CardCvv = "123",
                CardExpiry = DateTime.Now.AddMonths(10),
                Currency = "GBP",
                Amount = 123.32m,
                CreatedAt = DateTime.Now
            };

            _factory = new TestsWebApplicationFactory<Startup>();

            var paymentId = await new PaymentRepository(_factory.InMemoryAppDbContext).PersistPaymentAsync(payment);

            var expected = new GetMerchantPaymentByIdQueryResponse
            {
                PaymentId = paymentId,
                MerchantId = payment.MerchantId,
                CreatedAt = payment.CreatedAt,
                Currency = payment.Currency,
                Amount = payment.Amount,
                Succeeded = payment.Succeeded,
                CardCvv = "****",
                CardNumber = "****",
                DiscoveryLink = new List<ApiDiscoveryLink>
                {
                    new()
                    {
                        Action = "POST",
                        Href = new Uri($"http://localhost/merchants/{merchantId}/payments"),
                        Rel = "Process a new payment"
                    },
                    new()
                    {
                        Action = "GET",
                        Href = new Uri($"http://localhost/merchants/{merchantId}/payments/{paymentId}"),
                        Rel = "self"
                    }
                }
            };

            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync($"merchants/{merchantId}/payments/{paymentId}");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            JsonConvert.DeserializeObject<GetMerchantPaymentByIdQueryResponse>(await result.Content.ReadAsStringAsync())
                .Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task GetMerchantPaymentByIdAsync_WithInvalidMerchant_ReturnsNotFound()
        {
            // Arrange
            const long merchantId = 110;

            var payment = new Payment
            {
                MerchantId = merchantId + 1,
                Succeeded = true,
                CardNumber = "1234 1234 1234 1234",
                CardCvv = "123",
                CardExpiry = DateTime.Now.AddMonths(10),
                Currency = "GBP",
                Amount = 123.32m,
                CreatedAt = DateTime.Now
            };

            _factory = new TestsWebApplicationFactory<Startup>();

            var paymentId = await new PaymentRepository(_factory.InMemoryAppDbContext).PersistPaymentAsync(payment);

            var expected =
                JsonConvert.SerializeObject(new GetMerchantPaymentByIdQueryResponse().NotFoundResponse(merchantId, paymentId));

            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync($"merchants/{merchantId}/payments/{paymentId}");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            (await result.Content.ReadAsStringAsync()).Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task GetMerchantPaymentByIdAsync_WithInvalidPaymentId_ReturnsNotFound()
        {
            // Arrange
            const long merchantId1 = 110;
            const long merchantId2 = 120;

            var payment = new Payment
            {
                MerchantId = merchantId1,
                Succeeded = true,
                CardNumber = "1234 1234 1234 1234",
                CardCvv = "123",
                CardExpiry = DateTime.Now.AddMonths(10),
                Currency = "GBP",
                Amount = 123.32m,
                CreatedAt = DateTime.Now
            };

            _factory = new TestsWebApplicationFactory<Startup>();

            await new PaymentRepository(_factory.InMemoryAppDbContext).PersistPaymentAsync(payment);

            var payment2 = new Payment
            {
                MerchantId = merchantId2,
                Succeeded = true,
                CardNumber = "1234 1234 1234 1234",
                CardCvv = "123",
                CardExpiry = DateTime.Now.AddMonths(10),
                Currency = "GBP",
                Amount = 123.32m,
                CreatedAt = DateTime.Now
            };

            var paymentId2 =
                await new PaymentRepository(_factory.InMemoryAppDbContext).PersistPaymentAsync(payment2);

            var expected =
                JsonConvert.SerializeObject(new GetMerchantPaymentByIdQueryResponse().NotFoundResponse(merchantId1, paymentId2));

            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync($"merchants/{merchantId1}/payments/{paymentId2}");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            (await result.Content.ReadAsStringAsync()).Should().BeEquivalentTo(expected);
        }

    }
}