using System;
using EntityFrameworkCore.EncryptColumn.Attribute;

namespace PaymentGateway.Domain.Models
{
    public class Payment : BaseEntity
    {
        public long MerchantId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public bool Succeeded { get; set; }

        [EncryptColumn]
        public string CardNumber { get; set; }
        
        public string CardCvv { get; set; }

        public DateTime CardExpiry { get; set; }
    }
}