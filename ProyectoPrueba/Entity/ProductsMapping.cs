using AutoMapper;
using ProyectoPrueba.DTOs;

namespace ProyectoPrueba.Entity
{
    public class ProductsMapping : Profile
    {
        public ProductsMapping() {
            CreateMap<ProductDTO, Product>().ReverseMap();

            CreateMap<ProductDTO, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
