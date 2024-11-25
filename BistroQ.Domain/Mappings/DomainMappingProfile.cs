using AutoMapper;
using BistroQ.Domain.Dtos.Order;
using BistroQ.Domain.Dtos.Products;
using BistroQ.Domain.Dtos.Tables;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Mappings;

public class DomainMappingProfile : Profile
{
    public DomainMappingProfile()
    {
        // Table mappings
        CreateMap<TableResponse, Table>();

        // Zone mappings
        CreateMap<ZoneResponse, Zone>();
        CreateMap<ZoneDetailResponse, Zone>()
            .ForMember(dest => dest.Tables, opt => opt.MapFrom(src => src.Tables));

        // Order mappings
        CreateMap<OrderDetailResponse, Order>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
        CreateMap<OrderItemDetailResponse, OrderItem>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
        
        // Product mappings
        CreateMap<ProductResponse, Product>();
    }
}