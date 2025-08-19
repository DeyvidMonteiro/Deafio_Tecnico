using AutoMapper;
using DesafioTecnicoAvanade.VendasApi.Models;

namespace DesafioTecnicoAvanade.VendasApi.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CartDTO, Cart>().ReverseMap();
            CreateMap<CartHeaderDTO, CartHeader>().ReverseMap();
            CreateMap<CartItemDTO, CartItem>().ReverseMap();
            CreateMap<ProductDTO, Product>().ReverseMap();
        }
    }
}
