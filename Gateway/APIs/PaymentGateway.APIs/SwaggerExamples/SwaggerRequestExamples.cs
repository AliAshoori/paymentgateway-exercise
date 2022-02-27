using System;
using PaymentGateway.ApplicationServices.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace PaymentGateway.APIs.SwaggerExamples
{
    public class SwaggerRequestExamples
    {
        public class NewPaymentCommandExample
            : IExamplesProvider<NewPaymentCommand>
        {
            public NewPaymentCommand GetExamples()
            {
                return new(
                    merchantId: 1001,
                    amount: 345.89m,
                    currency: "GBP",
                    cardNumber: "1234 4321 1234 4321",
                    cardCvv: "123",
                    cardExpiry: DateTime.Now.AddMonths(10));
            }
        }
    }
}
