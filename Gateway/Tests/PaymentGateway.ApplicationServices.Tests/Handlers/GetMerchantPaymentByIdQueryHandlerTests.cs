using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.ApplicationServices.Handlers;
using PaymentGateway.ApplicationServices.Helpers;
using PaymentGateway.ApplicationServices.Requests;
using PaymentGateway.ApplicationServices.Responses;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.ApplicationServices.Tests.Handlers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("APIs.Handlers")]
    public class GetMerchantPaymentByIdQueryHandlerTests
    {
        [TestMethod]
        public async Task Handle_HappyScenario_ReturnsPayment()
        {
            // Arrange
            var query = new GetMerchantPaymentByIdQuery(
                merchantId: 100,
                paymentId: 98);

            var payment = new Payment
            {
                MerchantId = query.MerchantId,
                Id = query.PaymentId,
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
                CardNumber = payment.CardNumber,
                Currency = payment.Currency,
                Amount = payment.Amount,
                CardCvv = payment.CardCvv,
                CreatedAt = payment.CreatedAt,
                DiscoveryLink = Enumerable.Empty<ApiDiscoveryLink>()
            };

            var mockRepository = new Mock<IPaymentRepository>();
            mockRepository.Setup(m => m.GetMerchantPaymentById(query.MerchantId, query.PaymentId))
                .ReturnsAsync(payment);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<GetMerchantPaymentByIdQueryResponse>(payment))
                .Returns(expectedResponse);

            var mockLinkBuilder = new Mock<IApiDiscoveryLinkBuilder<GetMerchantPaymentByIdQueryResponse>>();
            mockLinkBuilder.Setup(m => m.GenerateLinks(expectedResponse))
                .Returns(new List<ApiDiscoveryLink>
                {
                    new()
                    {
                        Rel = "Process a new payment",
                        Action = "POST",
                        Href = new Uri($"https://localhost:1100/merchants/{query.MerchantId}/payments")
                    },
                    new()
                    {
                        Rel = "self",
                        Action = "GET",
                        Href = new Uri($"https://localhost:1100/merchants/{query.MerchantId}/payments/{query.PaymentId}")
                    }
                });

            var mockLogger = new Mock<ILogger<GetMerchantPaymentByIdQueryHandler>>();

            var handler = 
                new GetMerchantPaymentByIdQueryHandler(
                    mockRepository.Object, 
                    mockMapper.Object,
                    mockLinkBuilder.Object,
                    mockLogger.Object);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [TestMethod]
        public async Task Handle_WithPaymentNotFound_ReturnsNull()
        {
            // Arrange
            var query = new GetMerchantPaymentByIdQuery(
                merchantId: 100,
                paymentId: 98);

            Payment payment = null;
            GetMerchantPaymentByIdQueryResponse expectedResponse = null;

            var mockRepository = new Mock<IPaymentRepository>();
            mockRepository.Setup(m => m.GetMerchantPaymentById(query.MerchantId, query.PaymentId))
                .ReturnsAsync(payment);

            var mockMapper = new Mock<IMapper>();

            var mockLinkBuilder = new Mock<IApiDiscoveryLinkBuilder<GetMerchantPaymentByIdQueryResponse>>();
            mockLinkBuilder.Setup(m => m.GenerateLinks(expectedResponse))
                .Returns(new List<ApiDiscoveryLink>
                {
                    new()
                    {
                        Rel = "Process a new payment",
                        Action = "POST",
                        Href = new Uri($"https://localhost:1100/merchants/{query.MerchantId}/payments")
                    },
                    new()
                    {
                        Rel = "self",
                        Action = "GET",
                        Href = new Uri($"https://localhost:1100/merchants/{query.MerchantId}/payments/{query.PaymentId}")
                    }
                });

            var mockLogger = new Mock<ILogger<GetMerchantPaymentByIdQueryHandler>>();

            var handler = 
                new GetMerchantPaymentByIdQueryHandler(
                    mockRepository.Object, 
                    mockMapper.Object,
                    mockLinkBuilder.Object,
                    mockLogger.Object);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void Handle_WithRepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var query = new GetMerchantPaymentByIdQuery(
                merchantId: 100,
                paymentId: 98);

            var mockRepository = new Mock<IPaymentRepository>();
            mockRepository.Setup(m => m.GetMerchantPaymentById(query.MerchantId, query.PaymentId))
                .Throws<InvalidOperationException>();

            var mockMapper = new Mock<IMapper>();
            var mockLinkBuilder = new Mock<IApiDiscoveryLinkBuilder<GetMerchantPaymentByIdQueryResponse>>();

            var mockLogger = new Mock<ILogger<GetMerchantPaymentByIdQueryHandler>>();

            var handler = new GetMerchantPaymentByIdQueryHandler(
                mockRepository.Object, 
                mockMapper.Object,
                mockLinkBuilder.Object,
                mockLogger.Object);

            // Act
            Func<Task<GetMerchantPaymentByIdQueryResponse>> result = async () => await handler.Handle(query, default);

            // Assert
            result.Should().ThrowExactlyAsync<InvalidOperationException>();
        }

        [TestMethod]
        public void GetMerchantPaymentByIdQueryHandler_WithNullLinkBuilder_ThrowsArgNullException()
        {
            // Arrange
            var mockRepository = new Mock<IPaymentRepository>();
            var mockMapper = new Mock<IMapper>();
            IApiDiscoveryLinkBuilder<GetMerchantPaymentByIdQueryResponse> discoveryLinkBuilder = null;
            var mockLogger = new Mock<ILogger<GetMerchantPaymentByIdQueryHandler>>();

            // Act
            Action init = () => new GetMerchantPaymentByIdQueryHandler(
                mockRepository.Object,
                mockMapper.Object,
                discoveryLinkBuilder,
                mockLogger.Object);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(discoveryLinkBuilder));
        }

        [TestMethod]
        public void GetMerchantPaymentByIdQueryHandler_WithNullMapper_ThrowsArgNullException()
        {
            // Arrange
            var mockRepository = new Mock<IPaymentRepository>();
            IMapper mapper = null;
            var mockLinkBuilder = new Mock<IApiDiscoveryLinkBuilder<GetMerchantPaymentByIdQueryResponse>>();
            var mockLogger = new Mock<ILogger<GetMerchantPaymentByIdQueryHandler>>();

            // Act
            Action init = () => new GetMerchantPaymentByIdQueryHandler(
                mockRepository.Object, 
                mapper,
                mockLinkBuilder.Object,
                mockLogger.Object);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(mapper));
        }

        [TestMethod]
        public void GetMerchantPaymentByIdQueryHandler_WithNullLogger_ThrowsArgNullException()
        {
            // Arrange
            var mockRepository = new Mock<IPaymentRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockLinkBuilder = new Mock<IApiDiscoveryLinkBuilder<GetMerchantPaymentByIdQueryResponse>>();
            ILogger<GetMerchantPaymentByIdQueryHandler> logger = null;

            // Act
            Action init = () =>
                new GetMerchantPaymentByIdQueryHandler(
                    mockRepository.Object,
                    mockMapper.Object,
                    mockLinkBuilder.Object,
                    logger);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(logger));
        }

        [TestMethod]
        public void GetMerchantPaymentByIdQueryHandler_WithNullRepo_ThrowsArgNullException()
        {
            // Arrange
            IPaymentRepository paymentRepository = null;
            var mockMapper = new Mock<IMapper>();
            var mockLinkBuilder = new Mock<IApiDiscoveryLinkBuilder<GetMerchantPaymentByIdQueryResponse>>();
            var mockLogger = new Mock<ILogger<GetMerchantPaymentByIdQueryHandler>>();

            // Act
            Action init = () => 
                new GetMerchantPaymentByIdQueryHandler(
                    paymentRepository, 
                    mockMapper.Object,
                    mockLinkBuilder.Object,
                    mockLogger.Object);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(paymentRepository));
        }
    }
}