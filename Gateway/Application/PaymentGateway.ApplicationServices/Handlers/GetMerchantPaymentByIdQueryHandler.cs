using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.ApplicationServices.Helpers;
using PaymentGateway.ApplicationServices.Requests;
using PaymentGateway.ApplicationServices.Responses;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.ApplicationServices.Handlers
{
    public class GetMerchantPaymentByIdQueryHandler : IRequestHandler<GetMerchantPaymentByIdQuery, GetMerchantPaymentByIdQueryResponse>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly IApiDiscoveryLinkBuilder<GetMerchantPaymentByIdQueryResponse> _discoveryLinkBuilder;
        private readonly ILogger<GetMerchantPaymentByIdQueryHandler> _logger;

        public GetMerchantPaymentByIdQueryHandler(
            IPaymentRepository paymentRepository,
            IMapper mapper, 
            IApiDiscoveryLinkBuilder<GetMerchantPaymentByIdQueryResponse> discoveryLinkBuilder, 
            ILogger<GetMerchantPaymentByIdQueryHandler> logger)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _paymentRepository = Guard.Against.Null(paymentRepository, nameof(paymentRepository));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _discoveryLinkBuilder = Guard.Against.Null(discoveryLinkBuilder, nameof(discoveryLinkBuilder));
        }

        public async Task<GetMerchantPaymentByIdQueryResponse> Handle(
            GetMerchantPaymentByIdQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"### Processing query: {query}");

            var payment = await _paymentRepository.GetMerchantPaymentById(query.MerchantId, query.PaymentId);

            if (payment == null)
            {
                return null;
            }

            var response = _mapper.Map<GetMerchantPaymentByIdQueryResponse>(payment);
            response.DiscoveryLink = _discoveryLinkBuilder.GenerateLinks(response);

            return response;
        }
    }
}