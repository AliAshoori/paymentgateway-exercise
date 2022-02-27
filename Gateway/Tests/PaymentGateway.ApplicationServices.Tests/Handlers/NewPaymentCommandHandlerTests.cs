using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
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
using PaymentGateway.Domain.DTOs;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.ApplicationServices.Tests.Handlers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("APIs.Handlers")]
    public class NewPaymentCommandHandlerTests
    {
        [TestMethod]
        public async Task Handle_HappyScenario_ReturnsNewPaymentCommandResponse()
        {
            // Arrange
            var command = new NewPaymentCommand(
                merchantId: 909,
                amount: 444.567m,
                currency: "GBP",
                cardNumber: "1233 1233 1233 1233",
                cardCvv: "123",
                cardExpiry: DateTime.Now.AddMonths(9));

            var mockBankService = new Mock<IMockAcquiringBank>();
            mockBankService.Setup(m => m.ProcessAsync(It.IsAny<BankPaymentProcessRequest>()))
                .ReturnsAsync(BankPaymentProcessResponse.SuccessPayload());

            var payment = new Payment
            {
                MerchantId = command.MerchantId,
                Currency = command.Currency,
                Amount = command.Amount,
                CardNumber = command.CardNumber,
                CardCvv = command.CardCvv,
                CardExpiry = command.CardExpiry
            };
            var repoResponse = payment;
            repoResponse.CreatedAt = DateTime.Now;
            repoResponse.Succeeded = true;
            var mockRepo = new Mock<IPaymentRepository>();
            mockRepo.Setup(m => m.PersistPaymentAsync(repoResponse)).ReturnsAsync(1);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<Payment>(command)).Returns(payment);

            const long paymentId = 1;

            var mockLinkBuilder = new Mock<IApiDiscoveryLinkBuilder<NewPaymentCommandResponse>>();
            var apiDiscoveryLinks = new List<ApiDiscoveryLink>
            {
                new()
                {
                    Rel = "self",
                    Action = "POST",
                    Href = new Uri($"https://localhost:1100/merchants/{command.MerchantId}/payments")
                },
                new()
                {
                    Rel = "get payment",
                    Action = "GET",
                    Href = new Uri($"https://localhost:1100/merchants/{command.MerchantId}/payments/{paymentId}")
                }
            };

            var expected = new NewPaymentCommandResponse
            {
                MerchantId = command.MerchantId,
                PaymentId = paymentId,
                Succeeded = true,
                DiscoveryLink = apiDiscoveryLinks
            };

            mockLinkBuilder.Setup(m => m.GenerateLinks(It.IsAny<NewPaymentCommandResponse>()))
                .Returns(apiDiscoveryLinks);

            var mockLogger = new Mock<ILogger<NewPaymentCommandHandler>>();

            var handler = new NewPaymentCommandHandler(
                mockBankService.Object,
                mockRepo.Object, 
                mockMapper.Object,
                mockLinkBuilder.Object,
                mockLogger.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task Handle_WithBankFailsTheProcess_ReturnsNewPaymentCommandResponse()
        {
            // Arrange
            var command = new NewPaymentCommand(
                merchantId: 909,
                amount: 444.567m,
                currency: "GBP",
                cardNumber: "1233 1233 1233 1233",
                cardCvv: "123",
                cardExpiry: DateTime.Now.AddMonths(9));

            var mockBankService = new Mock<IMockAcquiringBank>();
            mockBankService.Setup(m => m.ProcessAsync(It.IsAny<BankPaymentProcessRequest>()))
                .ReturnsAsync(BankPaymentProcessResponse.FailurePayload());

            var payment = new Payment
            {
                MerchantId = command.MerchantId,
                Currency = command.Currency,
                Amount = command.Amount,
                CardNumber = command.CardNumber,
                CardCvv = command.CardCvv,
                CardExpiry = command.CardExpiry
            };
            var repoResponse = payment;
            repoResponse.CreatedAt = DateTime.Now;
            repoResponse.Succeeded = false;
            var mockRepo = new Mock<IPaymentRepository>();
            mockRepo.Setup(m => m.PersistPaymentAsync(repoResponse)).ReturnsAsync(1);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<Payment>(command)).Returns(payment);

            const long paymentId = 1;

            var mockLinkBuilder = new Mock<IApiDiscoveryLinkBuilder<NewPaymentCommandResponse>>();
            var apiDiscoveryLinks = new List<ApiDiscoveryLink>
            {
                new()
                {
                    Rel = "self",
                    Action = "POST",
                    Href = new Uri($"https://localhost:1100/merchants/{command.MerchantId}/payments")
                },
                new()
                {
                    Rel = "get payment",
                    Action = "GET",
                    Href = new Uri($"https://localhost:1100/merchants/{command.MerchantId}/payments/{paymentId}")
                }
            };

            var expected = new NewPaymentCommandResponse
            {
                MerchantId = command.MerchantId,
                PaymentId = paymentId,
                Succeeded = false,
                DiscoveryLink = apiDiscoveryLinks
            };

            mockLinkBuilder.Setup(m => m.GenerateLinks(It.IsAny<NewPaymentCommandResponse>()))
                .Returns(apiDiscoveryLinks);

            var mockLogger = new Mock<ILogger<NewPaymentCommandHandler>>();

            var handler = new NewPaymentCommandHandler(
                mockBankService.Object,
                mockRepo.Object,
                mockMapper.Object,
                mockLinkBuilder.Object,
                mockLogger.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task Handle_WithBankThrowsException_ThrowsException()
        {
            // Arrange
            var command = new NewPaymentCommand(
                merchantId: 909,
                amount: 444.567m,
                currency: "GBP",
                cardNumber: "1233 1233 1233 1233",
                cardCvv: "123",
                cardExpiry: DateTime.Now.AddMonths(9));

            var mockBankService = new Mock<IMockAcquiringBank>();
            mockBankService.Setup(m => m.ProcessAsync(It.IsAny<BankPaymentProcessRequest>())).Throws<HttpRequestException>();

            var mockRepo = new Mock<IPaymentRepository>();

            var mockMapper = new Mock<IMapper>();
            var mockLinkBuilder = new Mock<IApiDiscoveryLinkBuilder<NewPaymentCommandResponse>>();

            var mockLogger = new Mock<ILogger<NewPaymentCommandHandler>>();

            var handler = new NewPaymentCommandHandler(
                mockBankService.Object,
                mockRepo.Object,
                mockMapper.Object,
                mockLinkBuilder.Object,
                mockLogger.Object);

            // Act
            Func<Task<NewPaymentCommandResponse>> result = async () => await handler.Handle(command, default);

            // Assert
            await result.Should().ThrowAsync<HttpRequestException>();
        }

        [TestMethod]
        public async Task Handle_WithRepoThrowsException_ThrowsException()
        {
            // Arrange
            var command = new NewPaymentCommand(
                merchantId: 909,
                amount: 444.567m,
                currency: "GBP",
                cardNumber: "1233 1233 1233 1233",
                cardCvv: "123",
                cardExpiry: DateTime.Now.AddMonths(9));

            var mockBankService = new Mock<IMockAcquiringBank>();
            mockBankService.Setup(m => m.ProcessAsync(It.IsAny<BankPaymentProcessRequest>()))
                .ReturnsAsync(BankPaymentProcessResponse.FailurePayload());

            var payment = new Payment
            {
                MerchantId = command.MerchantId,
                Currency = command.Currency,
                Amount = command.Amount,
                CardNumber = command.CardNumber,
                CardCvv = command.CardCvv,
                CardExpiry = command.CardExpiry
            };

            var mockRepo = new Mock<IPaymentRepository>();
            mockRepo.Setup(m => m.PersistPaymentAsync(It.IsAny<Payment>())).Throws<InvalidOperationException>();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<Payment>(command)).Returns(payment);

            var mockLinkBuilder = new Mock<IApiDiscoveryLinkBuilder<NewPaymentCommandResponse>>();

            var mockLogger = new Mock<ILogger<NewPaymentCommandHandler>>();

            var handler = new NewPaymentCommandHandler(
                mockBankService.Object,
                mockRepo.Object,
                mockMapper.Object,
                mockLinkBuilder.Object,
                mockLogger.Object);

            // Act
            Func<Task<NewPaymentCommandResponse>> result = async () => await handler.Handle(command, default);

            // Assert
            await result.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestMethod]
        public void NewPaymentCommandHandler_WithNullMapper_ThrowsArgumentNullException()
        {
            // Arrange
            var mockBankService = new Mock<IMockAcquiringBank>();
            var mockRepo = new Mock<IPaymentRepository>();
            IMapper mapper = null;
            var mockLinkBuilder = new Mock<IApiDiscoveryLinkBuilder<NewPaymentCommandResponse>>();
            var mockLogger = new Mock<ILogger<NewPaymentCommandHandler>>();

            // Act
            Action init = () => 
                new NewPaymentCommandHandler(
                    mockBankService.Object, 
                    mockRepo.Object, mapper,
                    mockLinkBuilder.Object,
                    mockLogger.Object);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(mapper));
        }

        [TestMethod]
        public void NewPaymentCommandHandler_WithNullRepo_ThrowsArgumentNullException()
        {
            // Arrange
            var mockBankService = new Mock<IMockAcquiringBank>();
            IPaymentRepository paymentRepository = null;
            var mockMapper = new Mock<IMapper>();
            var mockLinkBuilder = new Mock<IApiDiscoveryLinkBuilder<NewPaymentCommandResponse>>();
            var mockLogger = new Mock<ILogger<NewPaymentCommandHandler>>();

            // Act
            Action init = () => 
                new NewPaymentCommandHandler(
                    mockBankService.Object, 
                    paymentRepository, 
                    mockMapper.Object,
                    mockLinkBuilder.Object,
                    mockLogger.Object);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(paymentRepository));
        }


        [TestMethod]
        public void NewPaymentCommandHandler_WithNullLinkBuilder_ThrowsArgumentNullException()
        {
            // Arrange
            var mockBankService = new Mock<IMockAcquiringBank>();
            var mockRepo = new Mock<IPaymentRepository>();
            var mockMapper = new Mock<IMapper>();
            IApiDiscoveryLinkBuilder<NewPaymentCommandResponse> discoveryLinkBuilder = null;
            var mockLogger = new Mock<ILogger<NewPaymentCommandHandler>>();

            // Act
            Action init = () =>
                new NewPaymentCommandHandler(
                    mockBankService.Object,
                    mockRepo.Object,
                    mockMapper.Object,
                    discoveryLinkBuilder,
                    mockLogger.Object);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(discoveryLinkBuilder));
        }

        [TestMethod]
        public void NewPaymentCommandHandler_WithNullBankService_ThrowsArgumentNullException()
        {
            // Arrange
            IMockAcquiringBank acquireBankService = null;
            var mockRepo = new Mock<IPaymentRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockLinkBuilder = new Mock<IApiDiscoveryLinkBuilder<NewPaymentCommandResponse>>();
            var mockLogger = new Mock<ILogger<NewPaymentCommandHandler>>();

            // Act
            Action init = () => 
                new NewPaymentCommandHandler(
                    acquireBankService, 
                    mockRepo.Object, 
                    mockMapper.Object,
                    mockLinkBuilder.Object,
                    mockLogger.Object);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(acquireBankService));
        }

        [TestMethod]
        public void NewPaymentCommandHandler_WithNullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            var mockBank = new Mock<IMockAcquiringBank>();
            var mockRepo = new Mock<IPaymentRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockLinkBuilder = new Mock<IApiDiscoveryLinkBuilder<NewPaymentCommandResponse>>();
            ILogger<NewPaymentCommandHandler> logger = null;

            // Act
            Action init = () =>
                new NewPaymentCommandHandler(
                    mockBank.Object,
                    mockRepo.Object,
                    mockMapper.Object,
                    mockLinkBuilder.Object,
                    logger);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(logger));
        }
    }
}
