using System.Collections.Generic;
using System.Linq;

namespace PaymentGateway.ApplicationServices.Responses
{
    public abstract class BaseApiResponse
    {
        public IEnumerable<ApiDiscoveryLink> DiscoveryLink { get; set; } 
            = Enumerable.Empty<ApiDiscoveryLink>();
    }
}