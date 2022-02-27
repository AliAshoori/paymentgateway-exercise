using AutoMapper;
using PaymentGateway.ApplicationServices.Responses;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.ApplicationServices.Mappers
{
    public class GetPaymentProfile : Profile
    {
        public GetPaymentProfile()
        {
            CreateMap<Payment, GetMerchantPaymentByIdQueryResponse>()
                .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.MerchantId))
                .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CardCvv, opt => opt.MapFrom(src => "****"))
                .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => "****"))
                .ForMember(dest => dest.Succeeded, opt => opt.MapFrom(src => src.Succeeded));
        }
    }
}