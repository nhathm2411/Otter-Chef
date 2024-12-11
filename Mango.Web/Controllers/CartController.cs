using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly ICouponService _couponService;
        private readonly IAuthService _authService;

        public CartController(ICartService cartService, IOrderService orderService, ICouponService couponService, IAuthService authService)
        {
            _cartService = cartService;
            _orderService = orderService;
            _couponService = couponService;
            _authService = authService;
        }

        [Authorize(Roles = SD.RoleCustomer)]
        public async Task<IActionResult> CartIndex()
        {
            var userId = User.Claims
                .Where(u => u.Type == JwtRegisteredClaimNames.Sub)
                ?.FirstOrDefault()?.Value;

            ResponseDto? response = await _cartService.GetCartByUserIdAsync(userId);
            if (response != null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert
                    .DeserializeObject<CartDto>(Convert.ToString(response.Result));
                if (cartDto.CartHeader.CouponCode != "")
                {
                    var couponExists = await _couponService.GetCouponAsync(cartDto.CartHeader.CouponCode);
                }
                return View(cartDto);
            }
            return View(new CartDto());
        }

        public async Task<IActionResult> RemoveCouponBeforeCart()
        {
            CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
            if(cart != null)
            {
                await RemoveCoupon(cart);
                return RedirectToAction("CartIndex");
            }
            return RedirectToAction("CartIndex");
        }

        [Authorize(Roles = SD.RoleCustomer)]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ApplicationUser? user = null;
            ResponseDto? response = await _authService.GetUserById(userId);
            if (response != null && response.IsSuccess)
            {
                user = JsonConvert.DeserializeObject<ApplicationUser>(Convert.ToString(response.Result));
            }
            ViewBag.User = user;
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [HttpPost]
        [ActionName(nameof(Checkout))]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Email = cartDto.CartHeader.Email;
            cart.CartHeader.Name = cartDto.CartHeader.Name;

            var response = await _orderService.CreateOrder(cart);
            OrderHeaderDto orderHeaderDto =
                JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

            if (response != null && response.IsSuccess)
            {
                var couponResponse = await _couponService.GetCouponAsync(orderHeaderDto.CouponCode);
                if (couponResponse != null && couponResponse.IsSuccess)
                {
                    CouponDto couponTmp = JsonConvert
                        .DeserializeObject<CouponDto>(Convert.ToString(couponResponse.Result));

                    couponTmp.Quantity -= 1;
                    _couponService.UpdateCouponAsync(couponTmp);
                }

                    //get stripe session and redirect to stripe to place order

                    var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                StripeRequestDto stripeRequestDto = new()
                {
                    ApprovedUrl = domain + "cart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
                    CancelUrl = domain + "cart/checkout",
                    OrderHeader = orderHeaderDto,
                };

                var stripeResponse = await _orderService.CreateStripeSession(stripeRequestDto);
                StripeRequestDto stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDto>
                    (Convert.ToString(stripeResponse.Result));
                ResponseDto? clearCartResponse = await _cartService.ClearCartByUserIdAsync(userId);
                if (clearCartResponse != null && clearCartResponse.IsSuccess)
                {
                    Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
                    return new StatusCodeResult(303);
                }
            }
            return View();
        }

        [Authorize(Roles = SD.RoleCustomer)]
        public async Task<IActionResult> RetryPayment(int orderId)
        {
            // Retrieve the order by its ID
            ResponseDto? response = await _orderService.GetOrder(orderId);
            if (response != null && response.IsSuccess)
            {
                OrderHeaderDto orderHeaderDto = JsonConvert
                    .DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

                // Check if the order status is Pending
                if (orderHeaderDto.Status == SD.Status_Pending)
                {
                    // Prepare the Stripe session
                    var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                    StripeRequestDto stripeRequestDto = new()
                    {
                        ApprovedUrl = domain + "cart/Confirmation?orderId=" + orderId,
                        CancelUrl = domain + "cart/checkout",
                        OrderHeader = orderHeaderDto,
                    };

                    // Create a new Stripe session
                    var stripeResponse = await _orderService.CreateStripeSession(stripeRequestDto);
                    StripeRequestDto stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDto>
                        (Convert.ToString(stripeResponse.Result));

                    // Check if the session was created successfully
                    if (!string.IsNullOrEmpty(stripeResponseResult.StripeSessionUrl))
                    {
                        // Clear the cart for the user after retrying payment
                        var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                        ResponseDto? clearCartResponse = await _cartService.ClearCartByUserIdAsync(userId);
                        if (clearCartResponse != null && clearCartResponse.IsSuccess)
                        {
                            // Redirect to the Stripe session URL for payment
                            Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
                            return new StatusCodeResult(303);
                        }
                    }
                }
            }

            TempData["error"] = "Unable to retry payment for this order.";
            return RedirectToAction(nameof(CartIndex));
        }
        [Authorize(Roles = SD.RoleCustomer)]
        public async Task<IActionResult> Confirmation(int orderId)
        {
            ResponseDto? response = await _orderService.ValidateStripeSession(orderId);
            if (response != null && response.IsSuccess)
            {
                OrderHeaderDto orderHeader = JsonConvert
                    .DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

                if (orderHeader.Status == SD.Status_Approved)
                {
                    return View(orderId);

                }
            }
            return View(orderId);
        }


        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            ResponseDto? response = await _cartService.RemoveFromCartAsync(cartDetailsId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {

            ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Apply Coupon Successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                TempData["error"] = response.Message;
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
            cart.CartHeader.Email = User.Claims
                .Where(u => u.Type == JwtRegisteredClaimNames.Email)
                ?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.EmailCart(cart);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Email will be processed and sent shortly.";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            if(cartDto.CartHeader == null || cartDto.CartDetails == null) return RedirectToAction(nameof(CartIndex));
            cartDto.CartHeader.CouponCode = "";
            ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Remove Coupon Successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims
                .Where(u => u.Type == JwtRegisteredClaimNames.Sub)
                ?.FirstOrDefault()?.Value;

            ResponseDto? response = await _cartService.GetCartByUserIdAsync(userId);
            if (response != null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert
                    .DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
            return new CartDto();
        }
    }
}
