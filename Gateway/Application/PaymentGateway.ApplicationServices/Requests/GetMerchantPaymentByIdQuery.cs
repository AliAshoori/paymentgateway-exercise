using MediatR;
using Newtonsoft.Json;
using PaymentGateway.ApplicationServices.Responses;

namespace PaymentGateway.ApplicationServices.Requests
{
    public class GetMerchantPaymentByIdQuery : IRequest<GetMerchantPaymentByIdQueryResponse>
    {
        public GetMerchantPaymentByIdQuery(long merchantId, long paymentId)
        {
            MerchantId = merchantId;
            PaymentId = paymentId;
        }

        public long MerchantId { get; }

        public long PaymentId { get; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}