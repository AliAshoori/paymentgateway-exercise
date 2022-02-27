using System.Threading.Tasks;
using PaymentGateway.Domain.DTOs;

namespace PaymentGateway.Domain.Interfaces
{
    public interface IMockAcquiringBank
    {
        Task<BankPaymentProcessResponse> ProcessAsync(BankPaymentProcessRequest request);
    }
}