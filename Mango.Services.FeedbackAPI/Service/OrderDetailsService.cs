using Mango.Services.FeedbackAPI.Models.Dto;
using Mango.Services.FeedbackAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.FeedbackAPI.Service
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public OrderDetailsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<OrderDetailsDto> GetOrderDetails(int id)
        {
            var client = _httpClientFactory.CreateClient("Order");
            var response = await client.GetAsync($"/api/order/GetOrderDetails/"+id);
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<OrderDetailsDto>(Convert.ToString(resp.Result));
            }
            return null;
        }

        public async Task<OrderHeaderDto> GetOrderHeaderByOrderDetailsId(int orderDetailsId)
        {
            var client = _httpClientFactory.CreateClient("Order");
            var response = await client.GetAsync($"/api/order/GetOrderHeaderByOrderDetailsId/" + orderDetailsId);
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(resp.Result));
            }
            return null;
        }
    }
}
