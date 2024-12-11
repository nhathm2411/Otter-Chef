using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IFeedbackService _feedbackService;

        public OrderController(IOrderService orderService, IFeedbackService feedbackService)
        {
            _orderService = orderService;
            _feedbackService = feedbackService;
        }
        [Authorize]
        public IActionResult OrderIndex()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> OrderDetail(int orderId)
        {
            string userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var orderHeaderDto = await GetOrderHeaderAsync(orderId);
            if (orderHeaderDto == null)
            {
                return NotFound();
            }

            if (!User.IsInRole(SD.RoleAdmin) && userId != orderHeaderDto.UserId)
            {
                return Forbid();
            }

            var feedbackList = await GetUserFeedbackAsync(userId);
            ViewBag.FeedbackList = feedbackList;

            return View(orderHeaderDto);
        }
        private async Task<OrderHeaderDto?> GetOrderHeaderAsync(int orderId)
        {
            var response = await _orderService.GetOrder(orderId);
            if (response != null && response.IsSuccess)
            {
                return JsonConvert.DeserializeObject<OrderHeaderDto>(response.Result?.ToString() ?? string.Empty);
            }
            return null;
        }
        private async Task<List<FeedbackDto>> GetUserFeedbackAsync(string userId)
        {
            var feedbackList = new List<FeedbackDto>();

            var feedbackResponse = await _feedbackService.GetFeedbackByUserIdAsync(userId);
            if (feedbackResponse != null && feedbackResponse.IsSuccess)
            {
                feedbackList = JsonConvert.DeserializeObject<List<FeedbackDto>>(feedbackResponse.Result?.ToString() ?? "[]") ?? new();
            }

            return feedbackList;
        }

        [HttpPost("OrderReadyForPickup")]
        public async Task<IActionResult> OrderReadyForPickup(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, SD.Status_ReadyForPickup);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }

        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, SD.Status_Completed);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, SD.Status_Cancelled);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeaderDto> list;
            string userId = "";
            if (!User.IsInRole(SD.RoleAdmin))
            {
                userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            }
            ResponseDto response = _orderService.GetAllOrders(userId).GetAwaiter().GetResult();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.Result));
                switch (status)
                {
                    case "approved":
                        list = list.Where(u => u.Status == SD.Status_Approved);
                        break;

                    case "readyforpickup":
                        list = list.Where(u => u.Status == SD.Status_ReadyForPickup);
                        break;
                    case "completed":
                        list = list.Where(u => u.Status == SD.Status_Completed);
                        break;
                    case "cancelled":
                        list = list.Where(u => u.Status == SD.Status_Cancelled);
                        break;
                    
                    default:
                        break;
                }
            }
            else
            {
                list = new List<OrderHeaderDto>();
            }
            return Json(new { data = list });
        }
    }
}
