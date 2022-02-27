using AutoMapper;
using PaymentGateway.ApplicationServices.Requests;
using PaymentGateway.Domain.DTOs;

namespace PaymentGateway.ApplicationServices.Mappers
{
    public class AcquireBankPaymentProfile : Profile
    {
        public AcquireBankPaymentProfile()
        {
            CreateMap<NewPaymentCommand, BankPaymentProcessRequest>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CardCvv, opt => opt.MapFrom(src => src.CardCvv))
                .ForMember(dest => dest.CardExpiry, opt => opt.MapFrom(src => src.CardExpiry))
                .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.CardNumber))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.MerchantId));
        }
    }
}