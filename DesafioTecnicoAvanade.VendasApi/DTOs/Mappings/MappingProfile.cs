using AutoMapper;
using DesafioTecnicoAvanade.VendasApi.Models;

namespace DesafioTecnicoAvanade.VendasApi.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            // Mapeamento do CartDTO para Order
            CreateMap<CartDTO, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.CartItems))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CartHeader.UserId))
                .ForMember(dest => dest.Total, opt => opt.Ignore()) // vai calcular depois
                .ForMember(dest => dest.OrderDate, opt => opt.Ignore());

            // Mapeamento de CartItem para OrderItem
            CreateMap<CartItem, OrderItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Qauntity));

        }
    }
}
