using System.ComponentModel.DataAnnotations;

namespace Mango.Services.CategoryAPI.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }
        [StringLength(100)]
        public string? ImageUrl { get; set; }
        [StringLength(100)]
        public string? ImageLocalPath { get; set; }
    }
}
