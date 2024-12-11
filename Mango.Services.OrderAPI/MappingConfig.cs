using AutoMapper;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
               config.CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(des => des.CartTotal, u=> u.MapFrom(src => src.OrderTotal)).ReverseMap();

                config.CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(des => des.ProductName, u => u.MapFrom(src => src.Product.Name))
                .ForMember(des => des.Price, u => u.MapFrom(src => src.Product.Price));

                config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
                config.CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();


            });
            return mappingConfig;
        }
    }
}
