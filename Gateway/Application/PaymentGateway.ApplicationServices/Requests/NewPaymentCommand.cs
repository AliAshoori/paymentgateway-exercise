using System;
using System.Text.Json.Serialization;
using MediatR;
using Newtonsoft.Json;
using PaymentGateway.ApplicationServices.Responses;

namespace PaymentGateway.ApplicationServices.Requests
{
    public class NewPaymentCommand : IRequest<NewPaymentCommandResponse>
    {
        public NewPaymentCommand(
            long merchantId, 
            decimal amount, 
            string currency, 
            string cardNumber, 
            string cardCvv, 
            DateTime cardExpiry)
        {
            MerchantId = merchantId;
            Amount = amount;
            Currency = currency;
            CardNumber = cardNumber;
            CardCvv = cardCvv;
            CardExpiry = cardExpiry;
        }

        public long MerchantId { get; }

        public decimal Amount { get; }

        public string Currency { get; }

        public string CardNumber { get; }

        public string CardCvv { get; }

        public DateTime CardExpiry { get; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(new
            {
                MerchantId,
                Currency,
                Amount,
                CardExpiry
            });
        }
    }
}
