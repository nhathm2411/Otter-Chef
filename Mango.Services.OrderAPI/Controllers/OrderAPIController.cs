﻿using AutoMapper;
using Mango.MessageBus;
using Mango.Services.OrderAPI.Data;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dto;
using Mango.Services.OrderAPI.Service.IService;
using Mango.Services.OrderAPI.Ultility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace Mango.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private IMapper _mapper;
        private readonly AppDbContext _db;
        private IProductService _productService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        public OrderAPIController(AppDbContext db,
            IProductService productService,
            IMapper mapper,
            IConfiguration configuration,
            IMessageBus messageBus)
        {
            _db = db;
            _response = new ResponseDto();
            _productService = productService;
            _mapper = mapper;
            _configuration = configuration;
            _messageBus = messageBus;
        }

        [Authorize]
        [HttpGet("GetOrders")]
        public ResponseDto? Get(string? userId = "")
        {
            try
            {
                IEnumerable<OrderHeader> objList;
                if (User.IsInRole(SD.RoleAdmin))
                {
                    objList = _db.OrderHeaders
                        .Include(u => u.OrderDetails)
                        .OrderByDescending(u => u.OrderHeaderId)
                        .ToList();
                }
                else
                {
                    objList = _db.OrderHeaders
                        .Include(u => u.OrderDetails)
                        .Where(u => u.UserId == userId)
                        .OrderByDescending(u => u.OrderHeaderId)
                        .ToList();
                }
                _response.Result = _mapper.Map<IEnumerable<OrderHeaderDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [Authorize]
        [HttpGet("GetOrder/{id:int}")]
        public async Task<ResponseDto?> Get(int id)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders
                    .Include(u => u.OrderDetails).First(u => u.OrderHeaderId == id);
                IEnumerable<ProductDto> productList = await _productService.GetProducts();
                foreach (var item in orderHeader.OrderDetails)
                {
                    item.Product = productList.FirstOrDefault(p => p.ProductId == item.ProductId);
                }
                _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        //[Authorize]
        [HttpGet("GetOrderDetails/{id:int}")]
        public async Task<ResponseDto?> GetOrderDetails(int id)
        {
            try
            {

                OrderDetails orderDetails = _db.OrderDetails
                    .First(u => u.OrderDetailsId == id);
                IEnumerable<ProductDto> productList = await _productService.GetProducts();
                var orderDetailsDto = _mapper.Map<OrderDetailsDto>(orderDetails);
                orderDetailsDto.Product = productList.First(p => p.ProductId == orderDetailsDto.ProductId);
                _response.Result = orderDetailsDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [Authorize]
        [HttpGet("GetOrderHeaderByOrderDetailsId/{orderDetailsId:int}")]
        public async Task<ResponseDto?> GetOrderHeaderByOrderDetailsId(int orderDetailsId)
        {
            try
            {
                OrderDetails orderDetails = await _db.OrderDetails
                    .FirstOrDefaultAsync(u => u.OrderDetailsId == orderDetailsId);

                OrderHeader orderHeader = await _db.OrderHeaders
                    .FirstOrDefaultAsync(oh => oh.OrderHeaderId == orderDetails.OrderHeaderId);

                var orderHeaderDto = _mapper.Map<OrderHeaderDto>(orderHeader);
                _response.Result = orderHeaderDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);

                OrderHeader orderCreated = _db.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
                await _db.SaveChangesAsync();

                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;
                _response.Result = orderHeaderDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<ResponseDto> CreateStripeSessions([FromBody] StripeRequestDto stripeRequestDto)
        {
            try
            {


                var options = new SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDto.ApprovedUrl,
                    CancelUrl = stripeRequestDto.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                var DiscountsObj = new List<SessionDiscountOptions>()
               {
                   new SessionDiscountOptions
                   {
                       Coupon = stripeRequestDto.OrderHeader.CouponCode
                   }
               };

                foreach (var item in stripeRequestDto.OrderHeader.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100), //$20.99 => 2099
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                if (stripeRequestDto.OrderHeader.Discount > 0)
                {
                    options.Discounts = DiscountsObj;
                }

                var service = new SessionService();
                Session session = service.Create(options);
                stripeRequestDto.StripeSessionUrl = session.Url;
                OrderHeader orderHeader = _db.OrderHeaders
                    .First(u => u.OrderHeaderId == stripeRequestDto.OrderHeader.OrderHeaderId);
                orderHeader.StripeSessionId = session.Id;
                _db.SaveChanges();
                _response.Result = stripeRequestDto;

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<ResponseDto> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders
                   .First(u => u.OrderHeaderId == orderHeaderId);

                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);

                var paymentIntentService = new PaymentIntentService();
                PaymentIntent paymentItent = paymentIntentService.Get(session.PaymentIntentId);


                if (paymentItent.Status == "succeeded")
                {
                    //then payment successful
                    orderHeader.PaymentIntentId = paymentItent.Id;
                    orderHeader.Status = SD.Status_Approved;
                    _db.SaveChanges();

                    RewardsDto rewardsDto = new()
                    {
                        OrderId = orderHeader.OrderHeaderId,
                        RewardsActivity = Convert.ToInt32(orderHeader.OrderTotal),
                        UserId = orderHeader.UserId
                    };
                    string topicName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");

                    /*await _messageBus.PublishMessage(rewardsDto, topicName);*/

                    _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
                }




            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        public async Task<ResponseDto> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders.First(u => u.OrderHeaderId == orderId);
                if (orderHeader != null)
                {
                    if (newStatus == SD.Status_Cancelled)
                    {
                        //we will give refund
                        var options = new RefundCreateOptions
                        {
                            Reason = RefundReasons.RequestedByCustomer,
                            PaymentIntent = orderHeader.PaymentIntentId,
                        };
                        var service = new RefundService();
                        Refund refund = service.Create(options);
                    }
                    orderHeader.Status = newStatus;
                    _db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
            }
            return _response;
        }



    }
}
