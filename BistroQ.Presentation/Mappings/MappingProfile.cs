using AutoMapper;
using BistroQ.Domain.Dtos.Order;
using BistroQ.Domain.Dtos.Products;
using BistroQ.Domain.Dtos.Tables;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Domain.Models.Entities;
using BistroQ.Presentation.ViewModels.Models;

namespace BistroQ.Presentation.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        /**
         * 
         * Domain to Dto
         * 
         */
        CreateMap<Table, TableViewModel>();
        CreateMap<Table, TableViewModel>().ReverseMap();
        CreateMap<Zone, ZoneViewModel>();
        CreateMap<Zone, ZoneViewModel>().ReverseMap();
        CreateMap<Order, OrderViewModel>();
        CreateMap<Order, OrderViewModel>().ReverseMap();
        CreateMap<OrderItem, OrderItemViewModel>();
        CreateMap<OrderItem, OrderItemViewModel>().ReverseMap();
        CreateMap<OrderItem, KitchenOrderItemViewModel>()
            .ForMember(dest => dest.Table, opt => opt.MapFrom(src => src.Order.Table));
        CreateMap<Product, ProductViewModel>();
        CreateMap<Product, ProductViewModel>().ReverseMap();

        /**
         * 
         * Dto to Domain
         * 
         */
        CreateMap<TableResponse, Table>();

        CreateMap<ZoneResponse, Zone>();
        CreateMap<ZoneDetailResponse, Zone>()
            .ForMember(dest => dest.Tables, opt => opt.MapFrom(src => src.Tables));

        CreateMap<OrderDetailResponse, Order>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
        CreateMap<OrderItemDetailResponse, OrderItem>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

        CreateMap<ProductResponse, Product>();
    }
}