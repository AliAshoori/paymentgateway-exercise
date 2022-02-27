using System.Threading.Tasks;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task<long> PersistPaymentAsync(Payment payment);

        Task<Payment> GetMerchantPaymentById(long merchantId, long paymentId);
    }
}