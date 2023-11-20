using AutoMapper;
using GeekShopping.ProductApi.Data.ValueObjects;
using GeekShopping.ProductApi.Model;

namespace GeekShopping.ProductApi.Config;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<ProductVO, Product>();
        CreateMap<Product, ProductVO>();              
    }
}
