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
            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();
            CreateMap<RequestCartItemDTO, CartItem>();

            CreateMap<CartHeader, CartDTO>()
                .ForMember(dest => dest.CartHeader, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

            CreateMap<RequestCartItemDTO, CartItemDTO>()
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<CartDTO, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.CartItems))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CartHeader.UserId))
                .ForMember(dest => dest.Total, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDate, opt => opt.Ignore());

            CreateMap<CartItemDTO, OrderItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Manter este
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Qauntity))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));

        }
    }
}
