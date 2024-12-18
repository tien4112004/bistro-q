using AutoMapper;
using BistroQ.Domain.Dtos.Account;
using BistroQ.Domain.Dtos.Category;
using BistroQ.Domain.Dtos.Order;
using BistroQ.Domain.Dtos.Orders;
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
        CreateMap<ZoneResponse, ZoneViewModel>();
        CreateMap<Order, OrderViewModel>();
        CreateMap<Order, OrderViewModel>().ReverseMap();
        CreateMap<OrderItem, OrderItemViewModel>();
        CreateMap<OrderItem, OrderItemViewModel>().ReverseMap();
        CreateMap<OrderItem, OrderItemViewModel>()
            .ForMember(dest => dest.Table, opt => opt.MapFrom(src => src.Order.Table));
        CreateMap<Product, ProductViewModel>();
        CreateMap<Product, ProductViewModel>().ReverseMap();
        CreateMap<Category, CategoryViewModel>();
        CreateMap<Category, CategoryViewModel>().ReverseMap();

        CreateMap<Account, AccountViewModel>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TableDisplay,
                opt => opt.MapFrom(src =>
                    src.TableId.HasValue
                        ? $"{src.Table.ZoneName} - Table {src.Table.Number}"
                        : "No Table Assigned"))
            .ForMember(dest => dest.ZoneName, opt => opt.MapFrom(src => src.Table != null ? src.Table.ZoneName : null))
            .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.Table != null ? src.Table.ZoneId : null));

        CreateMap<AccountViewModel, Account>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));

        /**
         * 
         * Dto to Domain
         * 
         */
        CreateMap<TableResponse, Table>();

        CreateMap<ZoneResponse, Zone>();
        CreateMap<ZoneDetailResponse, Zone>()
            .ForMember(dest => dest.Tables, opt => opt.MapFrom(src => src.Tables));

        CreateMap<OrderResponse, Order>();
        CreateMap<DetailOrderResponse, Order>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
        CreateMap<OrderItemWithProductResponse, OrderItem>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
        CreateMap<DetailOrderItemResponse, OrderItem>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
            .ForPath(dest => dest.Order.Table, opt => opt.MapFrom(src => src.Table));

        CreateMap<ProductResponse, Product>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(
                src => new Category
                {
                    CategoryId = src.CategoryId,
                    Name = src.CategoryName
                }
            ));

        CreateMap<CategoryResponse, Category>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        CreateMap<AccountResponse, Account>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Table, opt => opt.MapFrom(src => new Table
            {
                TableId = src.TableId,
                ZoneId = src.ZoneId,
                ZoneName = src.ZoneName,
                Number = src.TableNumber
            }));
    }
}