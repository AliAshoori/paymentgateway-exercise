using System;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.DTOs;

namespace PaymentGateway.Infrastructure.Bank
{
    public class MockBankNormalResponseGenerator : IMockBankRandomResponseGenerator
    {
        private readonly ILogger<MockBankNormalResponseGenerator> _logger;

        public MockBankNormalResponseGenerator(ILogger<MockBankNormalResponseGenerator> logger)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
        }

        public bool IsMatch(decimal amount) => amount < MockBankThresholds.ProcessableAmountThreshold;

        public Task<BankPaymentProcessResponse> GenerateAsync(BankPaymentProcessRequest request)
        {
            _logger.LogInformation("Mock Bank is now generating a random response for a normal use case. " +
                                   $" Threshold: {MockBankThresholds.ProcessableAmountThreshold}, Amount: {request.Amount}, Currency: {request.Currency}");

            return Task.FromResult(BankPaymentProcessResponse.SuccessPayload());
        }
    }
}