using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mango.Services.FeedbackAPI.Models.Dto
{
    public class FeedbackDto
    {
        public int FeedbackId { get; set; }
        public int OrderDetailId { get; set; }  // Liên kết phản hồi với sản phẩm trong OrderDetail
        public OrderDetailsDto? OrderDetail { get; set; }  // Navigation property đến OrderDetail
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime FeedbackDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
