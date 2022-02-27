using System.Collections.Generic;

namespace PaymentGateway.APIs
{
    public class BadRequestResponsePayload
    {
        public string Message { get; set; }

        public IEnumerable<BadRequestErrorDetail> Details { get; set; }
    }

    public class BadRequestErrorDetail
    {
        public string PropertyName { get; set; }

        public string AttemptedValue { get; set; }

        public string ErrorMessage { get; set; }
    }
}