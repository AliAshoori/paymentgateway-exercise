using System;
using System.Collections.Generic;
using PaymentGateway.ApplicationServices.Responses;

namespace PaymentGateway.ApplicationServices.Helpers
{
    public class NewPaymentCommandResponseLinkBuilder
        : BaseLinkBuilder, IApiDiscoveryLinkBuilder<NewPaymentCommandResponse>
    {
        public NewPaymentCommandResponseLinkBuilder(IHttpContextProvider contextProvider) : base(contextProvider)
        {
        }

        public IEnumerable<ApiDiscoveryLink> GenerateLinks(NewPaymentCommandResponse response)
        {
            var links = new List<ApiDiscoveryLink>
            {
                new()
                {
                    Action = "POST",
                    Href = new Uri($"{AppBaseUrl}/merchants/{response.MerchantId}/payments"),
                    Rel = "self"
                },
                new()
                {
                    Action = "GET",
                    Href = new Uri($"{AppBaseUrl}/merchants/{response.MerchantId}/payments/{response.PaymentId}"),
                    Rel = "get payment by id"
                }
            };

            return links;
        }
    }
}
