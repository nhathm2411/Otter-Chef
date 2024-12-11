using Mango.Services.FeedbackAPI.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.FeedbackAPI.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }
        public int OrderDetailId { get; set; }  // Liên kết phản hồi với sản phẩm trong OrderDetail
        [NotMapped]
        public OrderDetailsDto? OrderDetail { get; set; }  // Navigation property đến OrderDetail
        public string UserId { get; set; }
        [NotMapped]
        public ApplicationUser? User { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime FeedbackDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
