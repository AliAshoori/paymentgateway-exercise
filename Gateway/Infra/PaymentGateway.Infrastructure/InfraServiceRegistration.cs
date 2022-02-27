using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infrastructure.Bank;
using PaymentGateway.Infrastructure.Repositories;

namespace PaymentGateway.Infrastructure
{
    public static  class InfraServiceRegistration
    {
        public static void RegisterInfraServices(this IServiceCollection services)
        {
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            // mock bank
            services.AddScoped<IMockAcquiringBank, MockAcquiringBank>();
            services.AddScoped<IMockBankRandomResponseGenerator, MockBankSlowProcessingResponseGenerator>();
            services.AddScoped<IMockBankRandomResponseGenerator, MockBankServiceUnavailableResponseGenerator>();
            services.AddScoped<IMockBankRandomResponseGenerator, MockBankNormalResponseGenerator>();
        }
    }
}
