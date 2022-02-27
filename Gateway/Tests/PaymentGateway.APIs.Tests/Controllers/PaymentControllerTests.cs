using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.APIs.Controllers;
using PaymentGateway.ApplicationServices.Requests;
using PaymentGateway.ApplicationServices.Responses;

namespace PaymentGateway.APIs.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    [TestCategory("APIs.Controllers")]
    [TestClass]
    public class PaymentControllerTests
    {
        [TestMethod]
        public async Task GetMerchantPaymentByIdAsync_HappyScenario_ReturnsPaymentResponse()
        {
            // Arrange
            const long merchantId = 1001;
            const long paymentId = 44;

            var expectedResponse = new GetMerchantPaymentByIdQueryResponse
            {
                MerchantId = merchantId,
                PaymentId = paymentId,
                Succeeded = true,
                CardNumber = "1233 1233 1233 1233",
                Currency = "GBP",
                Amount = 3455.234m,
                CardCvv = "123",
                CreatedAt = DateTime.Now,
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

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(It.IsAny<GetMerchantPaymentByIdQuery>(), default)).ReturnsAsync(expectedResponse);

            var controller = new MerchantPaymentsController(mockMediator.Object);

            // Act
            var result = (OkObjectResult)(await controller.GetMerchantPaymentByIdAsync(merchantId, paymentId)).Result;

            // Assert
            result.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [TestMethod]
        public async Task GetMerchantPaymentByIdAsync_WithPaymentNotFound_ReturnsNull()
        {
            // Arrange
            const long merchantId = 1001;
            const long paymentId = 44;

            GetMerchantPaymentByIdQueryResponse expectedResponse = null;

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(It.IsAny<GetMerchantPaymentByIdQuery>(), default)).ReturnsAsync(expectedResponse);

            var expectedApiResponse = new GetMerchantPaymentByIdQueryResponse().NotFoundResponse(merchantId, paymentId);

            var controller = new MerchantPaymentsController(mockMediator.Object);

            // Act
            var result = (NotFoundObjectResult)(await controller.GetMerchantPaymentByIdAsync(merchantId, paymentId)).Result;

            // Assert
            result.Value.Should().BeEquivalentTo(expectedApiResponse);
        }

        [TestMethod]
        public void PaymentsController_WithNullMediator_ThrowsArgNullException()
        {
            // Arrange
            IMediator mediator = null;

            // Act
            Action init = () => new MerchantPaymentsController(mediator);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(mediator));
        }

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

            var expected = new NewPaymentCommandResponse
            {
                PaymentId = 1,
                Succeeded = true,
                DiscoveryLink = new List<ApiDiscoveryLink>
                {
                    new()
                    {
                        Action = "POST",
                        Href = new Uri($"http://localhost/merchants/{command.MerchantId}/payments"),
                        Rel = "Process a new payment"
                    },
                    new()
                    {
                        Action = "GET",
                        Href = new Uri($"http://localhost/merchants/{command.MerchantId}/payments/1"),
                        Rel = "self"
                    }
                }
            };

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(command, default)).ReturnsAsync(expected);

            var controller = new MerchantPaymentsController(mockMediator.Object);

            // Act
            var result = (OkObjectResult)(await controller.ProcessAsync(command)).Result;

            // Assert
            result.Value.Should().BeEquivalentTo(expected);
        }
    }
}
