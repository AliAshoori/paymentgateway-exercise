using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.Domain.DTOs;
using PaymentGateway.Infrastructure.Bank;

namespace PaymentGateway.Infra.Tests.AcquireBank
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MockBankSlowProcessingResponseGeneratorTests
    {
        [TestMethod]
        public async Task GenerateAsync_HappyScenario_ReturnsSuccessResponse()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<MockBankSlowProcessingResponseGenerator>>();

            var request =
                new BankPaymentProcessRequest
                {
                    CardNumber = "4444 5555 4444 5555",
                    CardCvv = "456",
                    Amount = 1001.05m,
                    Currency = "GBP",
                    CardExpiry = DateTime.Now.AddMonths(2),
                    MerchantId = 1
                };

            var options = Options.Create(new MockBankConfigOptions { SlowProcessingWaitTime = 5000 });

            var responseGenerator = new MockBankSlowProcessingResponseGenerator(mockLogger.Object, options);

            // Act
            var response = await responseGenerator.GenerateAsync(request);

            // Assert
            response.Should().BeEquivalentTo(BankPaymentProcessResponse.SuccessPayload());
        }

        [TestMethod]
        public void IsMatch_WithConditionNotMetGreaterThankServiceUnAvailable_ReturnsFalse()
        {
            // Arrange
            var threshold = MockBankThresholds.ServiceUnAvailableAmountThreshold + 1;

            var mockLogger = new Mock<ILogger<MockBankSlowProcessingResponseGenerator>>();

            var options = Options.Create(new MockBankConfigOptions { SlowProcessingWaitTime = 5000 });

            var responseGenerator = new MockBankSlowProcessingResponseGenerator(mockLogger.Object, options);

            // Act
            var isMatch = responseGenerator.IsMatch(threshold);

            // Assert
            isMatch.Should().BeFalse();
        }

        [TestMethod]
        public void IsMatch_WithConditionNotMetLessThanNormalThreshold_ReturnsFalse()
        {
            // Arrange
            var threshold = MockBankThresholds.ProcessableAmountThreshold - 1;

            var mockLogger = new Mock<ILogger<MockBankSlowProcessingResponseGenerator>>();

            var options = Options.Create(new MockBankConfigOptions { SlowProcessingWaitTime = 5000 });

            var responseGenerator = new MockBankSlowProcessingResponseGenerator(mockLogger.Object, options);

            // Act
            var isMatch = responseGenerator.IsMatch(threshold);

            // Assert
            isMatch.Should().BeFalse();
        }

        [TestMethod]
        public void IsMatch_WithConditionMetGreaterThanNormalLessThanUnAvailableService_ReturnsFalse()
        {
            // Arrange
            var threshold = MockBankThresholds.ProcessableAmountThreshold + 1;

            var mockLogger = new Mock<ILogger<MockBankSlowProcessingResponseGenerator>>();

            var options = Options.Create(new MockBankConfigOptions { SlowProcessingWaitTime = 5000 });

            var responseGenerator = new MockBankSlowProcessingResponseGenerator(mockLogger.Object, options);

            // Act
            var isMatch = responseGenerator.IsMatch(threshold);

            // Assert
            isMatch.Should().BeTrue();
        }

        [TestMethod]
        public void MockBankSlowProcessingResponseGenerator_WithNullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            ILogger<MockBankSlowProcessingResponseGenerator> logger = null;
            var options = Options.Create(new MockBankConfigOptions { SlowProcessingWaitTime = 5000 });

            // Act
            Action init = () => new MockBankSlowProcessingResponseGenerator(logger, options);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(logger));
        }

        [TestMethod]
        public void MockBankSlowProcessingResponseGenerator_WithNullOptions_ThrowsArgumentNullException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<MockBankSlowProcessingResponseGenerator>>();
            IOptions<MockBankConfigOptions> options = null;

            // Act
            Action init = () => new MockBankSlowProcessingResponseGenerator(mockLogger.Object, options);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(options));
        }
    }
}