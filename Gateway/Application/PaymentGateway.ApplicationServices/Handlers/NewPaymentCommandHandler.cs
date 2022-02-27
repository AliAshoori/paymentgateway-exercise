using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.ApplicationServices.Helpers;
using PaymentGateway.ApplicationServices.Requests;
using PaymentGateway.ApplicationServices.Responses;
using PaymentGateway.Domain.DTOs;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.ApplicationServices.Handlers
{
    public class NewPaymentCommandHandler : IRequestHandler<NewPaymentCommand, NewPaymentCommandResponse>
    {
        private readonly IMockAcquiringBank _acquireBankService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly IApiDiscoveryLinkBuilder<NewPaymentCommandResponse> _discoveryLinkBuilder;
        private readonly ILogger<NewPaymentCommandHandler> _logger;

        public NewPaymentCommandHandler(
            IMockAcquiringBank acquireBankService,
            IPaymentRepository paymentRepository,
            IMapper mapper, 
            IApiDiscoveryLinkBuilder<NewPaymentCommandResponse> discoveryLinkBuilder,
            ILogger<NewPaymentCommandHandler> logger)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _acquireBankService = Guard.Against.Null(acquireBankService, nameof(acquireBankService));
            _paymentRepository = Guard.Against.Null(paymentRepository, nameof(paymentRepository));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _discoveryLinkBuilder = Guard.Against.Null(discoveryLinkBuilder, nameof(discoveryLinkBuilder));
        }

        public async Task<NewPaymentCommandResponse> Handle(NewPaymentCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Processing payment: {command}");

            var bankPaymentProcessRequest = _mapper.Map<BankPaymentProcessRequest>(command);
            var bankResponse = await _acquireBankService.ProcessAsync(bankPaymentProcessRequest);

            var payment = _mapper.Map<Payment>(command);
            payment.Succeeded = bankResponse.Code == BankPaymentProcessResponse.SuccessPayload().Code;

            var paymentId = await _paymentRepository.PersistPaymentAsync(payment);

            var response = new NewPaymentCommandResponse
            {
                MerchantId = payment.MerchantId,
                PaymentId = paymentId,
                Succeeded = payment.Succeeded
            };

            response.DiscoveryLink = _discoveryLinkBuilder.GenerateLinks(response);

            return response;
        }
    }
}
