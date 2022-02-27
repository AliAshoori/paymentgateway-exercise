using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.ApplicationServices.Helpers;
using PaymentGateway.ApplicationServices.Requests;
using PaymentGateway.ApplicationServices.Responses;
using PaymentGateway.ApplicationServices.Validators;

namespace PaymentGateway.ApplicationServices
{
    public static class AppServiceRegistration
    {
        public static void RegisterAppServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(NewPaymentCommand));
            services.AddValidatorsFromAssembly(typeof(AppServiceRegistration).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddAutoMapper(typeof(AppServiceRegistration));
            services.AddScoped<IHttpContextProvider, HttpContextProvider>();
            services.AddScoped<IApiDiscoveryLinkBuilder<NewPaymentCommandResponse>, NewPaymentCommandResponseLinkBuilder>();
            services.AddScoped<IApiDiscoveryLinkBuilder<GetMerchantPaymentByIdQueryResponse>, GetMerchantPaymentResponseLinkBuilder>();
        }
    }
}
