using AutoMapper;
using MiniWarehouse.Domain.Entities;
using MiniWarehouse.Application.DTOs;

namespace MiniWarehouse.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse !=null ? src.Warehouse.Name : "Невідомий склад"));
            CreateMap<OrderCreateDto, Order>();
        }
    }
}
