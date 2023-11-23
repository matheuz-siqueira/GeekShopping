using AutoMapper;
using GeekShopping.CartApi.Data.ValueObjects;
using GeekShopping.CartApi.Model;

namespace GeekShopping.CartApi.Config;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Product, ProductVO>().ReverseMap();
        CreateMap<CartHeader, CartHeaderVO>().ReverseMap(); 
        CreateMap<CartDetail, CartDetailVO>().ReverseMap(); 
        CreateMap<Cart, CartVO>().ReverseMap();
    }
}
