using AutoMapper;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Models;
using BistroQ.Domain.Models.Entities;
using BistroQ.Presentation.ViewModels.Models;

namespace BistroQ.Presentation.Mappings;

public class PresentationMappingProfile : Profile
{
    public PresentationMappingProfile()
    {
        CreateMap<Table, TableViewModel>();
        CreateMap<Table, TableViewModel>().ReverseMap();
        
        CreateMap<Zone, ZoneViewModel>();
        CreateMap<Zone, ZoneViewModel>().ReverseMap();
    }
}