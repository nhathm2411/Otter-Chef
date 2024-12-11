using AutoMapper;
using Mango.Services.FeedbackAPI.Data;
using Mango.Services.FeedbackAPI.Models;
using Mango.Services.FeedbackAPI.Models.Dto;
using Mango.Services.FeedbackAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.FeedbackAPI.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;
        private IOrderDetailsService _orderDetailsService;

        public FeedbackAPIController(AppDbContext db, IOrderDetailsService orderDetailsService, IMapper mapper)
        {
            _db = db;
            _orderDetailsService = orderDetailsService;
            _mapper = mapper;
            _response = new ResponseDto();
        }
        [HttpPost]
        public ResponseDto Post(FeedbackDto feedBackDto)
        {
            try
            {
                Feedback feedback = _mapper.Map<Feedback>(feedBackDto);
                if (feedback.IsActive == null)
                {
                    feedback.IsActive = true;
                }
                _db.Feedbacks.Add(feedback);
                _db.SaveChanges();

                _response.Result = _mapper.Map<FeedbackDto>(feedback);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        public ResponseDto Put(FeedbackDto feedBackDto)
        {
            try
            {
                Feedback feedback = _mapper.Map<Feedback>(feedBackDto);
                _db.Feedbacks.Update(feedback);
                _db.SaveChanges();

                _response.Result = _mapper.Map<FeedbackDto>(feedback);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("GetFeedbackByProductId/{id}")]
        public async Task<ResponseDto> GetFeedbackByProductId(int id)
        {
            try
            {
                var feedbackListDb = await _db.Feedbacks.ToListAsync();
                var feedbackListDto = _mapper.Map<IEnumerable<FeedbackDto>>(feedbackListDb);

                var orderDetailTasks = feedbackListDto
                    .Select(feedback => _orderDetailsService.GetOrderDetails(feedback.OrderDetailId))
                    .ToArray();
                var orderDetails = await Task.WhenAll(orderDetailTasks);
                foreach (var feedback in feedbackListDto)
                {
                    feedback.OrderDetail = await _orderDetailsService.GetOrderDetails(feedback.OrderDetailId);
                }
                int i = 0;
                foreach (var feedback in feedbackListDto)
                {
                    feedback.OrderDetail = orderDetails[i++];
                }
                _response.Result = feedbackListDto.Where(fb => fb.OrderDetail.ProductId == id);
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("GetFeedbackByUserId/{userId}")]
        public async Task<ResponseDto> GetFeedbackByUserId(string userId)
        {
            try
            {
                // Directly filter feedbacks in the database by UserId
                var feedbackListDb = await _db.Feedbacks
                    .Where(fb => fb.UserId == userId)
                    .ToListAsync();

                var feedbackListDto = _mapper.Map<IEnumerable<FeedbackDto>>(feedbackListDb);

                // Fetch OrderDetails concurrently
                var orderDetailTasks = feedbackListDto
                    .Select(feedback => _orderDetailsService.GetOrderDetails(feedback.OrderDetailId))
                    .ToArray();
                var orderDetails = await Task.WhenAll(orderDetailTasks);

                // Map the order details back to the feedback items
                int i = 0;
                foreach (var feedback in feedbackListDto)
                {
                    feedback.OrderDetail = orderDetails[i++];
                }

                _response.Result = feedbackListDto;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet("GetFeedbackById/{feedbackId}")]
        public async Task<ResponseDto> GetFeedbackById(int feedbackId)
        {
            try
            {
                // Directly filter feedbacks in the database by UserId
                var feedbackDb = await _db.Feedbacks
                    .FirstOrDefaultAsync(fb => fb.FeedbackId == feedbackId);
                var feedbackDto = _mapper.Map<FeedbackDto>(feedbackDb);
                feedbackDto.OrderDetail = await _orderDetailsService.GetOrderDetails(feedbackDto.OrderDetailId);

                _response.Result = feedbackDto;
                _response.IsSuccess = true;
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
