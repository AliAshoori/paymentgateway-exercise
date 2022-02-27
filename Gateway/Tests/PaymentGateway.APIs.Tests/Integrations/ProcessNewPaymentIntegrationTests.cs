using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PaymentGateway.ApplicationServices.Requests;
using PaymentGateway.ApplicationServices.Resources;
using PaymentGateway.ApplicationServices.Responses;

namespace PaymentGateway.APIs.Tests.Integrations
{
    [ExcludeFromCodeCoverage]
    [TestCategory("APIs.Integrations")]
    [TestClass]
    public class ProcessNewPaymentIntegrationTests
    {
        private TestsWebApplicationFactory<Startup> _factory;

        [TestMethod]
        public async Task ProcessAsync_HappyScenario_ReturnsPaymentResponse()
        {
            // Arrange
            var command = new NewPaymentCommand(
                merchantId: 909,
                amount: 444.567m,
                currency: "GBP",
                cardNumber: "1233 1233 1233 1233",
                cardCvv: "123",
                cardExpiry: DateTime.Now.AddMonths(9));

            _factory = new TestsWebApplicationFactory<Startup>();

            var client = _factory.CreateClient();

            using var content =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // Act
            var apiResponse = await client.PostAsync($"merchants/{command.MerchantId}/payments", content);

            // Assert
            apiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var data = await apiResponse.Content.ReadAsStringAsync();
            var deserializedContent = JsonConvert.DeserializeObject<NewPaymentCommandResponse>(data);
            deserializedContent.Succeeded.Should().BeTrue();
        }

        [TestMethod]
        public async Task ProcessAsync_WithInvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var command = new NewPaymentCommand(
                merchantId: 909,
                amount: -444.567m,
                currency: "GBP",
                cardNumber: "1233 1233 1233 1233",
                cardCvv: "123",
                cardExpiry: DateTime.Now.AddMonths(9));

            _factory = new TestsWebApplicationFactory<Startup>();

            var client = _factory.CreateClient();

            using var content =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            var expected = new BadRequestResponsePayload
            {
                Message = "Validation Failed. The payload request is not valid.",
                Details = new List<BadRequestErrorDetail>
                {
                    new()
                    {
                        PropertyName = "Amount",
                        AttemptedValue = "-444.567",
                        ErrorMessage = ValidatorErrors.InvalidAmount
                    }
                }
            };

            // Act
            var apiResponse = await client.PostAsync($"merchants/{command.MerchantId}/payments", content);

            // Assert
            apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var data = JsonConvert.DeserializeObject<BadRequestResponsePayload>(await apiResponse.Content.ReadAsStringAsync());
            data.Should().BeEquivalentTo(expected);
        }
    }
}
