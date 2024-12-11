using AutoMapper;
using Mango.Services.FeedbackAPI.Models;
using Mango.Services.FeedbackAPI.Models.Dto;

namespace Mango.Services.FeedbackAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<FeedbackDto, Feedback>();
                config.CreateMap<Feedback, FeedbackDto>();
            });
            return mappingConfig;
        }
    }
}
