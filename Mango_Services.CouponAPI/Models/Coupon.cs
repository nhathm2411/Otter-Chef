using System.ComponentModel.DataAnnotations;

namespace Mango.Services.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public double DiscountAmount { get; set; }
        [Required]
        public int MinAmount { get; set; }
        [Required]
        public int Quantity { get; set; }
        [StringLength(100)]
        public string? ImageUrl { get; set; }
        [StringLength(100)]
        public string? ImageLocalPath { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
