using System;

namespace PaymentGateway.Domain.Models
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}