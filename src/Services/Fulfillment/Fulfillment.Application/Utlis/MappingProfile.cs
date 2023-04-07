using AutoMapper;
using Fulfillment.Application.Dtos;
using Fulfillment.Core.Buyer;

namespace Fulfillment.Application.Utlis;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Core.Order.Order, OrderResponseDto>();
        CreateMap<Core.Order.OrderItem, OrderItemResponseDto>();
        CreateMap<Buyer, BuyerResponseDto>();
    }
}