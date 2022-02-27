using System;

namespace PaymentGateway.ApplicationServices.Responses
{
    public class GetMerchantPaymentByIdQueryResponse : BaseApiResponse
    {
        public long PaymentId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CardNumber { get; set; }

        public string CardCvv { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public long MerchantId { get; set; }

        public bool Succeeded { get; set; }

        public object NotFoundResponse(long merchantId, long paymentId)
        {
            return new
            {
                Message = $"No payment with id: {paymentId} that belongs to merchant with id: {merchantId} found"
            };
        }
    }
}