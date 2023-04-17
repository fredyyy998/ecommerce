using Inventory.Application.Dtos;
using Inventory.Core.Product;
namespace Inventory.Application.Utils;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductResponseDto>();
        CreateMap<Product, AdminProductResponseDto>();
        CreateMap<Price, PriceDto>();
    }
}
