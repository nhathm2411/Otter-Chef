using AutoMapper;
using Azure;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    /*[Authorize]*/
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;

        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()    
        {
            try
            {
                IEnumerable<Coupon> objList = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon obj = _db.Coupons.First(u => u.CouponId == id);
                _response.Result = _mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon obj = _db.Coupons.First(u => u.CouponCode.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpPost]
        /*[Authorize(Roles = "ADMIN")]*/
        public ResponseDto Post(CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                coupon.IsActive = true;
                Coupon couponIsExist = _db.Coupons.FirstOrDefault(u => u.CouponCode == couponDto.CouponCode);
                if(couponIsExist != null )
                {
                    if(couponIsExist.IsActive && couponIsExist.Quantity != 0)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Coupon Code was Existed!";
                        return _response;
                    }
                }
                _db.Coupons.Add(coupon);
                _db.SaveChanges();

                if (couponDto.Image != null)
                {
                    string fileName = coupon.CouponId + Path.GetExtension(couponDto.Image.FileName);
                    string filePath = @"wwwroot\CouponImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        couponDto.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";

                    coupon.ImageUrl = baseUrl + "/CouponImages/" + fileName;
                    coupon.ImageLocalPath = filePath;
                }
                else
                {
                    coupon.ImageUrl = "https://placehold.co/600x400";
                }

                _db.Coupons.Update(coupon);
                _db.SaveChanges();

                var options = new Stripe.CouponCreateOptions
                {
                    AmountOff = (long)(couponDto.DiscountAmount * 100),
                    Name = couponDto.CouponCode,
                    Currency = "usd",
                    Id = couponDto.CouponCode,
                };
                var service = new Stripe.CouponService();
                service.Create(options);

                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpPut]
        public ResponseDto Put(CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);

                if (couponDto.Image != null)
                {

                    if (!string.IsNullOrEmpty(coupon.ImageLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), coupon.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }


                    string fileName = coupon.CouponId + Path.GetExtension(couponDto.Image.FileName);
                    string filePath = @"wwwroot\CouponImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        couponDto.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";

                    coupon.ImageUrl = baseUrl + "/CouponImages/" + fileName;
                    coupon.ImageLocalPath = filePath;
                }

                _db.Coupons.Update(coupon);
                _db.SaveChanges();
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpDelete]
        [Route("{id:int}")]
        /*[Authorize(Roles = "ADMIN")]*/
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon obj = _db.Coupons.First(u => u.CouponId == id);
                _db.Coupons.Remove(obj);
                _db.SaveChanges();
               
                var service = new Stripe.CouponService();
                service.Delete(obj.CouponCode);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

    }
}
