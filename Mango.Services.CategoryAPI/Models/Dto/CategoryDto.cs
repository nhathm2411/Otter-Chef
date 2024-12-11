
namespace Mango.Services.CategoryAPI.Models.Dto
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public IFormFile? Image { get; set; }
    }
}
