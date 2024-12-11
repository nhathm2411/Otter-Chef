using Mango.Services.FeedbackAPI.Models.Dto;

namespace Mango.Services.FeedbackAPI.Service.IService
{
    public interface IOrderDetailsService
    {
        Task<OrderDetailsDto> GetOrderDetails(int id);
        Task<OrderHeaderDto> GetOrderHeaderByOrderDetailsId(int orderDetailsId);

    }
}
