using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.DTOs;

namespace PaymentGateway.Infrastructure.Bank
{
    public class MockBankServiceUnavailableResponseGenerator : IMockBankRandomResponseGenerator
    {
        private readonly ILogger<MockBankServiceUnavailableResponseGenerator> _logger;

        public MockBankServiceUnavailableResponseGenerator(ILogger<MockBankServiceUnavailableResponseGenerator> logger)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
        }

        public bool IsMatch(decimal amount) => amount >= MockBankThresholds.ServiceUnAvailableAmountThreshold;

        public Task<BankPaymentProcessResponse> GenerateAsync(BankPaymentProcessRequest request)
        {
            _logger.LogInformation("Mock Bank is now generating a random response for when it is not available. " +
                                   $"The request amount is: {MockBankThresholds.ServiceUnAvailableAmountThreshold}");

            return Task.FromResult(BankPaymentProcessResponse.FailurePayload());
        }
    }
}