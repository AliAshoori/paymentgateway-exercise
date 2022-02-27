using AutoMapper;
using PaymentGateway.ApplicationServices.Requests;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.ApplicationServices.Mappers
{
    public class NewPaymentProfile : Profile
    {
        public NewPaymentProfile()
        {
            CreateMap<NewPaymentCommand, Payment>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CardCvv, opt => opt.MapFrom(src => src.CardCvv))
                .ForMember(dest => dest.CardExpiry, opt => opt.MapFrom(src => src.CardExpiry))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.MerchantId));
        }
    }
}
