using System;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentGateway.Domain.DTOs;

namespace PaymentGateway.Infrastructure.Bank
{
    public class MockBankSlowProcessingResponseGenerator : IMockBankRandomResponseGenerator
    {
        private readonly ILogger<MockBankSlowProcessingResponseGenerator> _logger;
        private readonly IOptions<MockBankConfigOptions> _options;

        public MockBankSlowProcessingResponseGenerator(
            ILogger<MockBankSlowProcessingResponseGenerator> logger,
            IOptions<MockBankConfigOptions> options)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _options = Guard.Against.Null(options, nameof(options));
        }

        public bool IsMatch(decimal amount)
        {
            return amount >= MockBankThresholds.ProcessableAmountThreshold &&
                   amount < MockBankThresholds.ServiceUnAvailableAmountThreshold;
        }

        public async Task<BankPaymentProcessResponse> GenerateAsync(BankPaymentProcessRequest request)
        {
            _logger.LogInformation("Mock Bank is now generating a random response for a slow-processing use case. " +
                                   $"Time to wait (ms): {_options.Value.SlowProcessingWaitTime}." +
                                   $" Threshold: {MockBankThresholds.ProcessableAmountThreshold}, Amount: {request.Amount}, Currency: {request.Currency}");

            await Task.Delay(_options.Value.SlowProcessingWaitTime);

            return BankPaymentProcessResponse.SuccessPayload();
        }
    }
}