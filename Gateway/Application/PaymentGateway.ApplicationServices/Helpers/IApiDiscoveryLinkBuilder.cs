using System.Collections.Generic;
using PaymentGateway.ApplicationServices.Responses;

namespace PaymentGateway.ApplicationServices.Helpers
{
    public interface IApiDiscoveryLinkBuilder<in TResponse> 
        where TResponse : BaseApiResponse
    {
        IEnumerable<ApiDiscoveryLink> GenerateLinks(TResponse response);
    }
}