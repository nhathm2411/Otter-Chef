namespace Mango.Services.CouponAPI.Models.Dto
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
        public bool IsActive { get; set; }
        public IFormFile? Image { get; set; }
    }
}
