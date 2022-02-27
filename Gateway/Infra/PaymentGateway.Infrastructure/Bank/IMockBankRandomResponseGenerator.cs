using System.Threading.Tasks;
using PaymentGateway.Domain.DTOs;

namespace PaymentGateway.Infrastructure.Bank
{
    public interface IMockBankRandomResponseGenerator
    {
        bool IsMatch(decimal amount);

        Task<BankPaymentProcessResponse> GenerateAsync(BankPaymentProcessRequest request);
    }
}