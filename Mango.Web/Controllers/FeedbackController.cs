using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IFeedbackService _feedbackService;
        private readonly IAuthService _authService;
        private readonly IProductService _productService;

        public FeedbackController(IOrderService orderService, IFeedbackService feedbackService, IAuthService authService, IProductService productService)
        {
            _orderService = orderService;
            _feedbackService = feedbackService;
            _authService = authService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> FeedbackCreate(int orderDetailId)
        {
            string userId = "";
            OrderDetailsDto? orderDetailDto = null;
            ProductDto productData = null;
            userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); // Redirect if user is not authenticated
            }
            ResponseDto? userResponse = await _authService.GetUserById(userId);
            ResponseDto? orderDetailsResponse = await _orderService.GetOrderDetails(orderDetailId);

            if (orderDetailsResponse != null && orderDetailsResponse.IsSuccess)
            {
                orderDetailDto = JsonConvert.DeserializeObject<OrderDetailsDto>(Convert.ToString(orderDetailsResponse.Result));
                productData = orderDetailDto.Product;
            }

            UserDto userData = null;

            if (userResponse != null && userResponse.IsSuccess)
            {
                userData = JsonConvert.DeserializeObject<UserDto>(Convert.ToString(userResponse.Result));
            }
            ViewBag.UserData = userData;
            ViewBag.ProductData = productData;
            ViewBag.OrderHeaderId = orderDetailDto.OrderHeaderId;
            // Create a FeedbackDto instance
            FeedbackDto feedbackModel = new FeedbackDto
            {
                OrderDetailId = orderDetailId,
                UserId = userId
            };
            return View(feedbackModel);
        }

        [HttpPost]
        public async Task<IActionResult> FeedbackCreate(FeedbackDto feedbackDto)
        {
            feedbackDto.IsActive = true;
            feedbackDto.FeedbackDate = DateTime.Now;
            OrderHeaderDto? orderHeaderDto = null;
            ResponseDto? createFeedbackResponse = await _feedbackService.CreateFeedbackAsync(feedbackDto);
            if (createFeedbackResponse != null && createFeedbackResponse.IsSuccess)
            {
                ResponseDto? orderHeaderResponse = await _orderService.GetOrderHeaderByOrderDetailsId(feedbackDto.OrderDetailId);
                if (orderHeaderResponse != null && orderHeaderResponse.IsSuccess)
                {
                    orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(orderHeaderResponse.Result));
                }
                TempData["success"] = "Create feedback successfully!";
                if (orderHeaderDto != null)
                    return RedirectToAction("OrderDetail", "Order", new { orderId = orderHeaderDto.OrderHeaderId });
            }
            TempData["error"] = createFeedbackResponse.Message;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FeedbackDetail(int feedbackId)
        {
            FeedbackDto? feedbackDto = null;
            ResponseDto? feedbackResponse = await _feedbackService.GetFeedbackByIdAsync(feedbackId);
            if (feedbackResponse != null && feedbackResponse.IsSuccess)
            {
                feedbackDto = JsonConvert.DeserializeObject<FeedbackDto>(Convert.ToString(feedbackResponse.Result));
            }

            string userId = "";
            OrderDetailsDto? orderDetailDto = null;
            ProductDto productData = null;
            userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
            ResponseDto? userResponse = await _authService.GetUserById(userId);
            ResponseDto? orderDetailsResponse = await _orderService.GetOrderDetails(feedbackDto.OrderDetailId);

            if (orderDetailsResponse != null && orderDetailsResponse.IsSuccess)
            {
                orderDetailDto = JsonConvert.DeserializeObject<OrderDetailsDto>(Convert.ToString(orderDetailsResponse.Result));
                productData = orderDetailDto.Product;
            }

            UserDto userData = null;

            if (userResponse != null && userResponse.IsSuccess)
            {
                userData = JsonConvert.DeserializeObject<UserDto>(Convert.ToString(userResponse.Result));
            }
            ViewBag.UserData = userData;
            ViewBag.ProductData = productData;
            ViewBag.OrderHeaderId = orderDetailDto.OrderHeaderId;

            return View(feedbackDto);
        }

        [HttpGet]
        public async Task<IActionResult> FeedbackIndex()
        {
            List<ProductDto?> productList = new();

            ResponseDto? productResponse = await _productService.GetAllProductsAsync();
            if (productResponse != null && productResponse.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(productResponse.Result));
            }
            else
            {
                TempData["error"] = productResponse?.Message;
            }

            return View(productList);
        }

        [HttpGet]
        public async Task<IActionResult> FeedbackList(int productId)
        {
            List<FeedbackDto> feedbackList = new();
            ProductDto product = new();
            ResponseDto? feedbackResponse = await _feedbackService.GetFeedbackByProductIdAsync(productId);
            ResponseDto? productResponse = await _productService.GetProductByIdAsync(productId);
            if (productResponse != null && productResponse.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(productResponse.Result));
            }
            else
            {
                TempData["error"] = productResponse?.Message;
            }
            if (feedbackResponse != null && feedbackResponse.IsSuccess)
            {
                feedbackList = JsonConvert.DeserializeObject<List<FeedbackDto>>(Convert.ToString(feedbackResponse.Result));
                if(feedbackList != null)
                {
                    foreach(var feedback in feedbackList)
                    {
                        string userId = feedback.UserId;
                        ApplicationUser userData = null;
                        ResponseDto? userResponse = await _authService.GetUserById(userId);
                        if (userResponse != null && userResponse.IsSuccess)
                        {
                            userData = JsonConvert.DeserializeObject<ApplicationUser>(Convert.ToString(userResponse.Result));
                            feedback.User = userData;
                        }
                    }
                }
            }
            else
            {
                TempData["error"] = feedbackResponse?.Message;
            }
            ViewData["ProductOfFeedback"] = product.Name;
            ViewData["ProductImageOfFeedback"] = product.ImageUrl;
            return View(feedbackList);
        }

        public async Task<IActionResult> BanFeedback(int feedbackId)
        {
            FeedbackDto feedback = new();
            ResponseDto? feedbackResponse = await _feedbackService.GetFeedbackByIdAsync(feedbackId);
            if (feedbackResponse != null && feedbackResponse.IsSuccess)
            {
                feedback = JsonConvert.DeserializeObject<FeedbackDto>(Convert.ToString(feedbackResponse.Result));
            }
            else
            {
                TempData["error"] = feedbackResponse?.Message;
            }
            feedback.IsActive = false;
            feedbackResponse = await _feedbackService.UpdateFeedbackAsync(feedback);
            if (feedbackResponse != null && feedbackResponse.IsSuccess)
            {
                feedback = JsonConvert.DeserializeObject<FeedbackDto>(Convert.ToString(feedbackResponse.Result));
            }
            var productId = feedback.OrderDetail.ProductId;
            TempData["success"] = $"Feedback with id {feedbackId} was banned!";
            return RedirectToAction(nameof(FeedbackList), new { productId = productId });

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ProductDto> list;
            List<FeedbackDto> feedbacks = new();
            ResponseDto productResponse = _productService.GetAllProductsAsync().GetAwaiter().GetResult();
            
            if (productResponse != null && productResponse.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(productResponse.Result));
                if(list != null)
                {
                    foreach (var item in list)
                    {
                        ResponseDto feedbackResponse = await _feedbackService.GetFeedbackByProductIdAsync(item.ProductId);
                        if (feedbackResponse != null && productResponse.IsSuccess)
                        {
                            feedbacks = JsonConvert.DeserializeObject<List<FeedbackDto>>(Convert.ToString(feedbackResponse.Result));
                            item.feedbackCount = feedbacks.Count();
                        }
                    }
                }
            }
            else
            {
                list = new List<ProductDto>();
            }
            return Json(new { data = list });
        }
    }
}
