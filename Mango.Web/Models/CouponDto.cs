using Mango.Web.Utility;

namespace Mango.Web.Models
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public bool IsActive { get; set; } = true;
        [MaxFileSize(1)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile? Image { get; set; }
    }
}
