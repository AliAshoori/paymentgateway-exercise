using System;
using System.Collections.Generic;
using PaymentGateway.ApplicationServices.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace PaymentGateway.APIs.SwaggerExamples
{
    public class SwaggerResponseExamples
    {
        public class GetMerchantPaymentByIdQueryBadRequestResponseExample
            : IExamplesProvider<object>
        {
            public object GetExamples()
            {
                return new
                {
                    Message = "Validation Failed. The payload request is not valid.",
                    Errors = "{ PropertyName = MerchantId, ErrorMessage = Invalid merchant id, AttemptedValue = 0 }"
                };
            }
        }

        public class GetMerchantPaymentByIdQueryNotFoundResponseExample
            : IExamplesProvider<object>
        {
            public object GetExamples()
            {
                return new
                {
                    Message = "No payment with id: 5 that belongs to merchant with id: 1001 found",
                };
            }
        }

        public class GetMerchantPaymentByIdQueryResponseExample
            : IExamplesProvider<GetMerchantPaymentByIdQueryResponse>
        {
            public GetMerchantPaymentByIdQueryResponse GetExamples()
            {
                return new GetMerchantPaymentByIdQueryResponse
                {
                    MerchantId = 1001,
                    PaymentId = 300,
                    CardNumber = "****",
                    CardCvv = "****",
                    Currency = "GBP",
                    Amount = 4556.123m,
                    Succeeded = true,
                    CreatedAt = DateTime.Now,
                    DiscoveryLink = new List<ApiDiscoveryLink>
                    {
                        new()
                        {
                            Action = "POST",
                            Href = new Uri("https://localhost:5001/{merchantId:long}/payment"),
                            Rel = "process new payment"
                        },
                        new()
                        {
                            Action = "GET",
                            Href = new Uri("https://localhost:5001/{merchantId:long}/payment/{paymentId:long}"),
                            Rel = "self"
                        }
                    }
                };
            }
        }

        public class NewPaymentCommandBadRequestResponseExample
            : IExamplesProvider<object>
        {
            public object GetExamples()
            {
                return new
                {
                    Message = "Validation Failed. The payload request is not valid.",
                    Errors = "{ PropertyName = MerchantId, ErrorMessage = Invalid merchant id, AttemptedValue = 0 }"
                };
            }
        }

        public class NewPaymentCommandResponseExample
            : IExamplesProvider<NewPaymentCommandResponse>
        {
            public NewPaymentCommandResponse GetExamples()
            {
                return new()
                {
                    PaymentId = 2003,
                    DiscoveryLink = new List<ApiDiscoveryLink>
                    {
                        new()
                        {
                            Action = "POST",
                            Href = new Uri("https://localhost:5001/{merchantId:long}/payment"),
                            Rel = "self"
                        },
                        new()
                        {
                            Action = "GET",
                            Href = new Uri("https://localhost:5001/{merchantId:long}/payment/{paymentId:long}"),
                            Rel = "get payment by id"
                        }
                    }
                };
            }
        }
    }
}
