using AutoMapper;
using DesafioTecnicoAvanade.EstoqueApi.Models;

namespace DesafioTecnicoAvanade.EstoqueApi.DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Product, ProductDTO>().ReverseMap();
    }

}
