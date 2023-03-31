using AutoMapper;
using ShoppingCart.Application.Dtos;

namespace ShoppingCart.Application.Utils;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Core.ShoppingCart.ShoppingCart, ShoppingCartResponseDto>();
        CreateMap<Core.ShoppingCart.ShoppingCartItem, ShoppingCartItemResponseDto>().ConstructUsing(
            x => new ShoppingCartItemResponseDto(x.Product.Id, x.Product.Name, x.Product.Description, x.TotalPrice,
                x.Product.Price.CurrencyCode, x.Quantity));
    }
}