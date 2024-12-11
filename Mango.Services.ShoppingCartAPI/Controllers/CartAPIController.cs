using AutoMapper;
using Mango.MessageBus;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly AppDbContext _db;
        private IProductService _productService;
        private ICouponService _couponService;
        private IConfiguration _configuration;
        private IMessageBus _messageBus;

        public CartAPIController(AppDbContext db,
            IMapper mapper,
            IProductService productService,
            ICouponService couponService,
            IMessageBus messageBus,
            IConfiguration configuration)
        {
            _db = db;
            _messageBus = messageBus;
            this._response = new ResponseDto();
            _mapper = mapper;
            _productService = productService;
            _couponService = couponService;
            _configuration = configuration;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_db.CartHeaders.First(u => u.UserId == userId))
                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_db.CartDetails
                    .Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));

                IEnumerable<ProductDto> productDtos = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                //apply coupon if any
                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if (coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }

                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                if(cartDto.CartHeader.CouponCode != "")
                {
                    var couponExists = await _couponService.GetCoupon(cartDto.CartHeader.CouponCode);

                    if (couponExists.CouponCode == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Coupon code does not exist.";
                        return _response;
                    }
                    else if (couponExists != null && (couponExists.IsActive == false || couponExists.Quantity == 0))
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Coupon code is expired.";
                        return _response;
                    }
                }

                var cartFromDb = await _db.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId);
                cartFromDb.CouponCode = cartDto.CartHeader.CouponCode;
                _db.CartHeaders.Update(cartFromDb);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPost("EmailCartRequest")]
        public async Task<object> EmailCartRequest([FromBody] CartDto cartDto)
        {
            try
            {

                /*await _messageBus.PublishMessage(cartDto, _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"));*/

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }
            return _response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _db.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId);
                cartFromDb.CouponCode = "";
                _db.CartHeaders.Update(cartFromDb);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }
            return _response;
        }


        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();

                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First())); //maybe bug here
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var carDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDto.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (carDetailsFromDb == null)
                    {
                        //create cartdetails
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));  //maybe bug here
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartDto.CartDetails.First().Count += carDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = carDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = carDetailsFromDb.CartDetailsId;

                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First())); //maybe bug here
                        await _db.SaveChangesAsync();

                    }
                }
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int carDetailsId)
        {
            try
            {
                CartDetails cartDetails = _db.CartDetails
                    .First(u => u.CartDetailsId == carDetailsId);

                int totalCountOfCartItem = _db.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails);
                if (totalCountOfCartItem == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders
                        .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync();


                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("UpdateCartQuantity")]
        public async Task<ResponseDto> UpdateCartQuantity([FromBody] UpdateCartQuantityDto request)
        {
            try
            {
                CartDto cartDto = new();

                var cartDetails = await _db.CartDetails.FirstOrDefaultAsync(cd => cd.CartDetailsId == request.CartDetailsId);

                cartDetails.Count = request.Quantity;
                _db.CartDetails.Update(cartDetails);
                var cartHeaderDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(ch => ch.CartHeaderId == cartDetails.CartHeaderId);

                cartDto.CartHeader = _mapper.Map<CartHeaderDto>(cartHeaderDb);

                IEnumerable<ProductDto> productDtos = await _productService.GetProducts();

                cartDto.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_db.CartDetails
                        .Where(u => u.CartHeaderId == cartDto.CartHeader.CartHeaderId));
                foreach (var item in cartDto.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cartDto.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }
                //apply coupon if any
                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCoupon(cartDto.CartHeader.CouponCode);
                    if (coupon != null && cartDto.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cartDto.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cartDto.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }
                _db.CartHeaders.Update(_mapper.Map<CartHeader>(cartDto.CartHeader));
                await _db.SaveChangesAsync();
                _response.Result = cartDto;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }

            return _response;
        }

        [HttpDelete("ClearCart/{userId}")]
        public async Task<ResponseDto> ClearCart(string userId)
        {
            try
            {
                // Retrieve the cart header for the specified user
                var cartHeader = await _db.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId);
                if (cartHeader != null)
                {
                    // Remove all CartDetails associated with this CartHeader
                    var cartDetails = _db.CartDetails.Where(cd => cd.CartHeaderId == cartHeader.CartHeaderId);
                    _db.CartDetails.RemoveRange(cartDetails);

                    // Remove the CartHeader itself
                    _db.CartHeaders.Remove(cartHeader);
                    await _db.SaveChangesAsync();

                    _response.Result = true;
                    _response.Message = "Cart cleared successfully.";
                }
                else
                {
                    _response.Result = false;
                    _response.Message = "No cart found for the specified user.";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }
            return _response;
        }


    }
}
