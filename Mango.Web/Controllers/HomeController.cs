using IdentityModel;
using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ICategoryService _categoryService;
        private readonly IFeedbackService _feedbackService;
        private readonly IAuthService _authService;
        private readonly ICouponService _couponService;
        public HomeController(IProductService productService, ICartService cartService, ICategoryService categoryService, IFeedbackService feedbackService, IAuthService authService, ICouponService couponService)
        {
            _productService = productService;
            _cartService = cartService;
            _categoryService = categoryService;
            _feedbackService = feedbackService;
            _authService = authService;
            _couponService = couponService;
        }

        public async Task<IActionResult> Index()
        {
            List<CouponDto> coupons = new();
            ResponseDto? responseCoupon = await _couponService.GetAllCouponsAsync();
            if (responseCoupon != null && responseCoupon.IsSuccess)
            {
                coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(responseCoupon.Result));
                ViewBag.Coupons = coupons;
            }


                List<ProductDto?> productList = new();
            ResponseDto? response = await _productService.GetAllProductsAsync();
            IEnumerable<CategoryDto?> categoryList = await GetCategoryList();
            if (response != null && response.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
                foreach (var product in productList)
                {
                    var category = categoryList.FirstOrDefault(c => c.CategoryId == product.CategoryId);

                    if (category != null)
                    {
                        product.Category = category;
                    }
                }
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            ViewBag.Categories = categoryList;
            return View(productList);
        }

        public async Task<IActionResult> ProductDetails(int productId)
        {
            // Retrieve product details
            var response = await _productService.GetProductByIdAsync(productId);
            if (response == null || !response.IsSuccess)
            {
                TempData["error"] = response?.Message;
                return RedirectToAction("Index"); // or some appropriate action
            }

            var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

            // Retrieve category list and assign category to product
            var categoryList = await GetCategoryList();
            var category = categoryList.FirstOrDefault(c => c.CategoryId == product.CategoryId);
            if (category != null)
            {
                product.Category = category;
            }

            // Retrieve feedback list
            var feedbackResponse = await _feedbackService.GetFeedbackByProductIdAsync(productId);
            List<FeedbackDto> feedbackList = null;

            if (feedbackResponse != null && feedbackResponse.IsSuccess)
            {
                feedbackList = JsonConvert.DeserializeObject<List<FeedbackDto>>(Convert.ToString(feedbackResponse.Result));
            }

            // Retrieve user details for each feedback concurrently
            if (feedbackList != null)
            {
                var userTasks = feedbackList.Select(async feedback =>
                {
                    var userResponse = await _authService.GetUserById(feedback.UserId);
                    if (userResponse.IsSuccess)
                    {
                        feedback.User = JsonConvert.DeserializeObject<ApplicationUser>(Convert.ToString(userResponse.Result));
                    }
                });

                await Task.WhenAll(userTasks);
            }

            ViewBag.FeedbackList = feedbackList ?? new List<FeedbackDto>();
            return View(product);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        //Add to cart
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            CartDto cartDto = new CartDto()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
                }
            };

            CartDetailsDto cartDetails = new CartDetailsDto()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId,
            };

            List<CartDetailsDto> cartDetailsDtos = new()
            {
                cartDetails
            };

            cartDto.CartDetails = cartDetailsDtos;

            ResponseDto? response = await _cartService.UpsertCartAsync(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item has been added to the Shopping Cart";
                //return RedirectToAction("ProductDetails", new { productId = productDto.ProductId });
                return RedirectToAction("Index", "Product");
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDto);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<IEnumerable<CategoryDto>> GetCategoryList()
        {
            List<CategoryDto?> categoryList = new();
            ResponseDto? categoryResponse = await _categoryService.GetAllCategoriesAsync();
            if (categoryResponse != null && categoryResponse.IsSuccess)
            {
                categoryList = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(categoryResponse.Result));
            }
            else
            {
                TempData["error"] = categoryResponse?.Message;
            }
            return categoryList;
        }
    }
}
