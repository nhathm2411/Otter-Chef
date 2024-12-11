using AutoMapper;
using Mango.Services.CategoryAPI.Models;
using Mango.Services.CategoryAPI.Models.Dto;

namespace Mango.Services.CategoryAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CategoryDto, Category>();
                config.CreateMap<Category, CategoryDto>();
            });
            return mappingConfig;
        }
    }
}
