using Mango.Web.Models;
using Mango.Web.Models.Dto;

namespace Mango.Web.Service.IService
{
    public interface IFeedbackService
    {
        Task<ResponseDto?> CreateFeedbackAsync(FeedbackDto feedbackDto);
        Task<ResponseDto?> GetFeedbackByProductIdAsync(int productId);
        Task<ResponseDto?> GetFeedbackByUserIdAsync(string userId);
        Task<ResponseDto?> GetFeedbackByIdAsync(int feedbackId);
        Task<ResponseDto?> UpdateFeedbackAsync(FeedbackDto feedbackDto);

    }
}
