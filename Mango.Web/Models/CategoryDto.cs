
using Mango.Web.Utility;

namespace Mango.Web.Models
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        [MaxFileSize(1)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile? Image { get; set; }
    }
}
