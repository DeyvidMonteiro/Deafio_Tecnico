using AutoMapper;
using DesafioTecnicoAvanade.VendasApi.DTOs.Request;
using DesafioTecnicoAvanade.VendasApi.Models;

namespace DesafioTecnicoAvanade.VendasApi.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<CartItem, CartItemDTO>();
            CreateMap<Cart, CartDTO>().ReverseMap();

            CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();


            CreateMap<RequestCartItemDTO, CartItemDTO>()
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<RequestCartDTO, CartDTO>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems))
                .ForMember(dest => dest.CartHeader, opt => opt.MapFrom(src => new CartHeaderDTO { UserId = src.UserId }));



            CreateMap<CartDTO, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.CartItems))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CartHeader.UserId))
                .ForMember(dest => dest.Total, opt => opt.Ignore()) 
                .ForMember(dest => dest.OrderDate, opt => opt.Ignore());


            CreateMap<CartItem, OrderItem>()
               .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
               .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Qauntity))
               .ForMember(dest => dest.ProductName, opt => opt.Ignore())
               .ForMember(dest => dest.Price, opt => opt.Ignore());

        }
    }
}
