using System;

namespace PaymentGateway.Domain.DTOs
{
    public class BankPaymentProcessRequest
    {
        public string CardNumber { get; set; }

        public DateTime CardExpiry { get; set; }

        public string CardCvv { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public long MerchantId { get; set; }
    }
}