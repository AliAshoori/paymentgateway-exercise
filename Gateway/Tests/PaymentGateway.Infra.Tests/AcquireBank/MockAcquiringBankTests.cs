using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.Domain.DTOs;
using PaymentGateway.Infrastructure.Bank;

namespace PaymentGateway.Infra.Tests.AcquireBank
{
    [ExcludeFromCodeCoverage]
    [TestCategory("Infra.MockBank")]
    [TestClass]
    public class MockAcquiringBankTests
    {
        [TestMethod]
        public async Task ProcessAsync_HappyScenario_ReturnsPaymentResponse()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<MockAcquiringBank>>();
            var request =
                 new BankPaymentProcessRequest
                 {
                     CardNumber = "1111 2222 3333 4444 5555",
                     CardCvv = "444",
                     Amount = 45.54m,
                     Currency = "GBP",
                     CardExpiry = DateTime.Now.AddMonths(6),
                     MerchantId = 1
                 };

            var response = BankPaymentProcessResponse.SuccessPayload();

            var mockNormalProcessing = new Mock<IMockBankRandomResponseGenerator>();
            mockNormalProcessing.Setup(m => m.IsMatch(It.IsAny<decimal>())).Returns(true);
            mockNormalProcessing.Setup(m => m.GenerateAsync(request)).ReturnsAsync(response);

            var mockServiceUnavailableProcessing = new Mock<IMockBankRandomResponseGenerator>();
            mockServiceUnavailableProcessing.Setup(m => m.IsMatch(It.IsAny<decimal>())).Returns(false);
            mockServiceUnavailableProcessing.Setup(m => m.GenerateAsync(request)).ReturnsAsync(It.IsAny<BankPaymentProcessResponse>());

            var mockSlowProcessing = new Mock<IMockBankRandomResponseGenerator>();
            mockSlowProcessing.Setup(m => m.IsMatch(It.IsAny<decimal>())).Returns(false);
            mockSlowProcessing.Setup(m => m.GenerateAsync(request)).ReturnsAsync(It.IsAny<BankPaymentProcessResponse>());

            var responseGenerators = new List<IMockBankRandomResponseGenerator>
            {
                mockNormalProcessing.Object,
                mockSlowProcessing.Object,
                mockServiceUnavailableProcessing.Object
            };

            var bank = new MockAcquiringBank(mockLogger.Object, responseGenerators);

            // Act
            var result = await bank.ProcessAsync(request);

            // Assert
            result.Should().BeEquivalentTo(response);
        }

        [TestMethod]
        public void MockAcquiringBank_WithNullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            ILogger<MockAcquiringBank> logger = null;
            var mockResponseGenerators = new Mock<IEnumerable<IMockBankRandomResponseGenerator>>();

            // Act
            Action init = () => new MockAcquiringBank(logger, mockResponseGenerators.Object);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(logger));
        }

        [TestMethod]
        public void MockAcquiringBank_WithNullResponseGenrators_ThrowsArgumentNullException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<MockAcquiringBank>>();
            IEnumerable<IMockBankRandomResponseGenerator> responseGenrators = null;

            // Act
            Action init = () => new MockAcquiringBank(mockLogger.Object, responseGenrators);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(responseGenrators));
        }
    }
}
