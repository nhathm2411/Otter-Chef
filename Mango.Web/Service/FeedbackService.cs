using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using static Mango.Web.Utility.SD;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace Mango.Web.Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IBaseService _baseService;
        public FeedbackService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateFeedbackAsync(FeedbackDto feedbackDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = feedbackDto,
                Url = SD.FeedbackAPIBase + "/api/feedback",
            });
        }

        public async Task<ResponseDto?> GetFeedbackByIdAsync(int feedbackId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.FeedbackAPIBase + "/api/feedback/GetFeedbackById/" + feedbackId,
            });
        }

        public async Task<ResponseDto?> GetFeedbackByProductIdAsync(int productId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.FeedbackAPIBase + "/api/feedback/GetFeedbackByProductId/"+productId,
            });
        }

        public async Task<ResponseDto?> GetFeedbackByUserIdAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.FeedbackAPIBase + "/api/feedback/GetFeedbackByUserId/" + userId,
            });
        }

        public async Task<ResponseDto?> UpdateFeedbackAsync(FeedbackDto feedbackDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = feedbackDto,
                Url = SD.FeedbackAPIBase + "/api/feedback",
            });
        }
    }
}
