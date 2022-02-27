using System;
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
    public class MockBankServiceUnavailableResponseGeneratorTests
    {
        [TestMethod]
        public async Task GenerateAsync_HappyScenario_ReturnsSuccessResponse()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<MockBankServiceUnavailableResponseGenerator>>();

            var request =
                new BankPaymentProcessRequest
                {
                    CardNumber = "4444 5555 4444 5555",
                    CardCvv = "456",
                    Amount = 2000000.00m,
                    Currency = "GBP",
                    CardExpiry = DateTime.Now.AddMonths(2),
                    MerchantId = 1
                };

            var responseGenerator = new MockBankServiceUnavailableResponseGenerator(mockLogger.Object);

            // Act
            var response = await responseGenerator.GenerateAsync(request);

            // Assert
            response.Should().BeEquivalentTo(BankPaymentProcessResponse.FailurePayload());
        }

        [TestMethod]
        public void IsMatch_WithConditionNotMet_ReturnsFalse()
        {
            // Arrange
            var threshold = MockBankThresholds.ProcessableAmountThreshold;

            var mockLogger = new Mock<ILogger<MockBankServiceUnavailableResponseGenerator>>();

            var responseGenerator = new MockBankServiceUnavailableResponseGenerator(mockLogger.Object);

            // Act
            var isMatch = responseGenerator.IsMatch(threshold);

            // Assert
            isMatch.Should().BeFalse();
        }

        [TestMethod]
        public void IsMatch_WithConditionMet_ReturnsFalse()
        {
            // Arrange
            var threshold = MockBankThresholds.ServiceUnAvailableAmountThreshold;

            var mockLogger = new Mock<ILogger<MockBankServiceUnavailableResponseGenerator>>();

            var responseGenerator = new MockBankServiceUnavailableResponseGenerator(mockLogger.Object);

            // Act
            var isMatch = responseGenerator.IsMatch(threshold);

            // Assert
            isMatch.Should().BeTrue();
        }

        [TestMethod]
        public void MockBankServiceUnavailableResponseGenerator_WithNullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            ILogger<MockBankServiceUnavailableResponseGenerator> logger = null;

            // Act
            Action init = () => new MockBankServiceUnavailableResponseGenerator(logger);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(logger));
        }
    }
}