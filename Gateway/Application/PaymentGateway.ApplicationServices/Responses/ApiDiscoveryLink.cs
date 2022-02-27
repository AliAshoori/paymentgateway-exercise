using System;

namespace PaymentGateway.ApplicationServices.Responses
{
    public class ApiDiscoveryLink
    {
        public Uri Href { get; set; }

        public string Action { get; set; }

        public string Rel { get; set; }
    }
}