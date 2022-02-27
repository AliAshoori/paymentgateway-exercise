using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Infrastructure.Context
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Amount).HasPrecision(18, 3);

            builder.ToTable("Payment", "Gateway");

            builder.Property(x => x.CreatedAt).HasDefaultValue(DateTime.Now);
        }
    }
}