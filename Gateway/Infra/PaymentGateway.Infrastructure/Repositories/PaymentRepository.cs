using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Infrastructure.Context;

namespace PaymentGateway.Infrastructure.Repositories
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<long> PersistPaymentAsync(Payment payment)
        {
            await DbContext.Payments.AddAsync(payment);
            await DbContext.SaveChangesAsync();
            
            return payment.Id;
        }

        public async Task<Payment> GetMerchantPaymentById(long merchantId, long paymentId)
        {
            var payment = await DbContext.Payments.SingleOrDefaultAsync(
                x => x.MerchantId == merchantId &&
                     x.Id == paymentId);

            return payment;
        }
    }
}
