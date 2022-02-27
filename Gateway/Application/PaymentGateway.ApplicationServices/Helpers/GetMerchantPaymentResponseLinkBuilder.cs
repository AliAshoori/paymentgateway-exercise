using System;
using System.Collections.Generic;
using PaymentGateway.ApplicationServices.Responses;

namespace PaymentGateway.ApplicationServices.Helpers
{
    public class GetMerchantPaymentResponseLinkBuilder
        : BaseLinkBuilder, IApiDiscoveryLinkBuilder<GetMerchantPaymentByIdQueryResponse>
    {
        public GetMerchantPaymentResponseLinkBuilder(IHttpContextProvider contextProvider) : base(contextProvider)
        {
        }

        public IEnumerable<ApiDiscoveryLink> GenerateLinks(GetMerchantPaymentByIdQueryResponse response)
        {
            var links = new List<ApiDiscoveryLink>
            {
                new()
                {
                    Action = "POST",
                    Href = new Uri($"{AppBaseUrl}/merchants/{response.MerchantId}/payments"),
                    Rel = "Process a new payment"
                },
                new()
                {
                    Action = "GET",
                    Href = new Uri($"{AppBaseUrl}/merchants/{response.MerchantId}/payments/{response.PaymentId}"),
                    Rel = "self"
                }
            };

            return links;
        }
    }
}
