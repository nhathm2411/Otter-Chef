using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IBaseService _baseService;
        public CategoryService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> CreateCategoryAsync(CategoryDto categoryDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = categoryDto,
                Url = SD.CategoryAPIBase + "/api/category",
                ContentType = SD.ContentType.MultipartFormData,
            });
        }

        public async Task<ResponseDto?> DeleteCategoryAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CategoryAPIBase + "/api/category/" + id,
            });
        }

        public async Task<ResponseDto?> GetAllCategoriesAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CategoryAPIBase + "/api/category",
            });
        }

        public async Task<ResponseDto?> GetCategoryByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/category/" + id,
            });
        }

        public async Task<ResponseDto?> UpdateCategoryAsync(CategoryDto categoryDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = categoryDto,
                Url = SD.CategoryAPIBase + "/api/category",
                ContentType = SD.ContentType.MultipartFormData,
            });
        }
    }
}
