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
    public class MockBankNormalResponseGeneratorTests
    {
        [TestMethod]
        public async Task GenerateAsync_HappyScenario_ReturnsSuccessResponse()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<MockBankNormalResponseGenerator>>();

            var request =
                new BankPaymentProcessRequest
                {
                    CardNumber = "4444 5555 4444 5555",
                    CardCvv = "456",
                    Amount = 452.05m,
                    Currency = "GBP",
                    CardExpiry = DateTime.Now.AddMonths(2),
                    MerchantId = 1
                };

            var responseGenerator = new MockBankNormalResponseGenerator(mockLogger.Object);

            // Act
            var response = await responseGenerator.GenerateAsync(request);

            // Assert
            response.Should().BeEquivalentTo(BankPaymentProcessResponse.SuccessPayload());
        }

        [TestMethod]
        public void IsMatch_WithConditionNotMet_ReturnsFalse()
        {
            // Arrange
            var threshold = MockBankThresholds.ProcessableAmountThreshold + 1;

            var mockLogger = new Mock<ILogger<MockBankNormalResponseGenerator>>();

            var responseGenerator = new MockBankNormalResponseGenerator(mockLogger.Object);

            // Act
            var isMatch = responseGenerator.IsMatch(threshold);

            // Assert
            isMatch.Should().BeFalse();
        }

        [TestMethod]
        public void IsMatch_WithConditionMet_ReturnsFalse()
        {
            // Arrange
            var threshold = MockBankThresholds.ProcessableAmountThreshold - 1;

            var mockLogger = new Mock<ILogger<MockBankNormalResponseGenerator>>();

            var responseGenerator = new MockBankNormalResponseGenerator(mockLogger.Object);

            // Act
            var isMatch = responseGenerator.IsMatch(threshold);

            // Assert
            isMatch.Should().BeTrue();
        }

        [TestMethod]
        public void MockBankNormalResponseGenerator_WithNullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            ILogger<MockBankNormalResponseGenerator> logger = null;

            // Act
            Action init = () => new MockBankNormalResponseGenerator(logger);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(logger));
        }
    }
}