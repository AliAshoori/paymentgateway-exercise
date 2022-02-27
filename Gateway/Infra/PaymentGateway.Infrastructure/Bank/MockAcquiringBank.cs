using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.DTOs;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Infrastructure.Bank
{
    public class MockAcquiringBank : IMockAcquiringBank
    {
        private readonly ILogger<MockAcquiringBank> _logger;
        private readonly IEnumerable<IMockBankRandomResponseGenerator> _responseGenerators;

        public MockAcquiringBank(
            ILogger<MockAcquiringBank> logger,
            IEnumerable<IMockBankRandomResponseGenerator> responseGenerators)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _responseGenerators = Guard.Against.Null(responseGenerators, nameof(responseGenerators));
        }

        public async Task<BankPaymentProcessResponse> ProcessAsync(BankPaymentProcessRequest request)
        {
            _logger.LogInformation($"Mock Acquiring Bank is not processing a new request for merchant: {request.MerchantId}");

            return await _responseGenerators.Single(item => item.IsMatch(request.Amount)).GenerateAsync(request);
        }
    }
}
