using AutoMapper;
using ShoppingCart.Application.Dtos;

namespace ShoppingCart.Application.Utils;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Core.ShoppingCart.ShoppingCart, ShoppingCartResponseDto>();
        // TODO we will need to customize the mapping for ShoppingCartItem
        CreateMap<Core.ShoppingCart.ShoppingCartItem, ShoppingCartItemResponseDto>();
    }
}