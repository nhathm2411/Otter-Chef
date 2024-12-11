using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IOrderService
    {
        Task<ResponseDto?> CreateOrder(CartDto cartDto);
        Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto);
        Task<ResponseDto?> ValidateStripeSession(int orderHeaderId);
        Task<ResponseDto?> GetAllOrders(string? userId);
        Task<ResponseDto?> GetOrderDetails(int id);
        Task<ResponseDto?> GetOrderHeaderByOrderDetailsId(int orderDetailsId);
        Task<ResponseDto?> GetOrder(int orderId);
        Task<ResponseDto?> UpdateOrderStatus(int orderId, string newStatus);
    }
}
