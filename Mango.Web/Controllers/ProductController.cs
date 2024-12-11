using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [Authorize(Roles = SD.RoleAdmin)]
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto?> productList = new();
            IEnumerable<CategoryDto?> categoryList = await GetCategoryList();

            ResponseDto? productResponse = await _productService.GetAllProductsAsync();
            if (productResponse != null && productResponse.IsSuccess && categoryList != null)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(productResponse.Result));
                foreach (var product in productList)
                {
                    var category = categoryList.FirstOrDefault(c => c.CategoryId == product.CategoryId);

                    if (category != null)
                    {
                        product.Category = category;
                    }
                }
            }
            else
            {
                TempData["error"] = productResponse?.Message;
            }

            return View(productList);
        }

        public async Task<IActionResult> Index(string? keyword)
        {
            List<ProductDto?> productList = new();
            IEnumerable<CategoryDto?> categoryList = await GetCategoryList();
            ResponseDto? productResponse = await _productService.GetAllProductsAsync();

            if (productResponse != null && productResponse.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(productResponse.Result));

                // Filter products by keyword if it exists
                if (!string.IsNullOrEmpty(keyword))
                {
                    productList = productList
                        .Where(p => p != null && p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                foreach (var product in productList)
                {
                    if (product != null)
                    {
                        var category = categoryList.FirstOrDefault(c => c.CategoryId == product.CategoryId);

                        if (category != null)
                        {
                            product.Category = category;
                        }
                    }
                }
            }
            else
            {
                TempData["error"] = productResponse?.Message;
            }

            ViewBag.Categories = categoryList;
            ViewBag.Keyword = keyword;  // To retain the keyword in the view (if needed)
            return View(productList);
        }


        [Authorize(Roles = SD.RoleAdmin)]
        public async Task<IActionResult> ProductCreate()
        {
            IEnumerable<CategoryDto?> categoryList = await GetCategoryList();
            ViewBag.CategoryList = new SelectList(categoryList, "CategoryId", "CategoryName");
            return View();
        }

        [Authorize(Roles = SD.RoleAdmin)]
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateProductAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View();
        }

        [Authorize(Roles = SD.RoleAdmin)]
        public async Task<IActionResult> ProductDelete(int productId)
        {
            IEnumerable<CategoryDto?> categoryList = await GetCategoryList();
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

                var category = categoryList.FirstOrDefault(c => c.CategoryId == model.CategoryId);

                if (category != null)
                {
                    model.Category = category;
                }
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [Authorize(Roles = SD.RoleAdmin)]
        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            ResponseDto? response = await _productService.DeleteProductAsync(productDto.ProductId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDto);
        }

        [Authorize(Roles = SD.RoleAdmin)]
        public async Task<IActionResult> ProductEdit(int productId)
        {
            IEnumerable<CategoryDto?> categoryList = await GetCategoryList();
            ViewBag.CategoryList = new SelectList(categoryList, "CategoryId", "CategoryName");

            ResponseDto? response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [Authorize(Roles = SD.RoleAdmin)]
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.UpdateProductAsync(productDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(productDto);
        }

        private async Task<IEnumerable<CategoryDto>> GetCategoryList()
        {
            List<CategoryDto?> categoryList = new();
            ResponseDto? categoryResponse = await _categoryService.GetAllCategoriesAsync();
            if (categoryResponse != null && categoryResponse.IsSuccess)
            {
                categoryList = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(categoryResponse.Result));
            }
            else
            {
                TempData["error"] = categoryResponse?.Message;
            }
            return categoryList;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ProductDto> list;
            IEnumerable<CategoryDto?> categoryList = await GetCategoryList();
            ResponseDto response = _productService.GetAllProductsAsync().GetAwaiter().GetResult();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
                foreach (var product in list)
                {
                    if (product != null)
                    {
                        var category = categoryList.FirstOrDefault(c => c.CategoryId == product.CategoryId);

                        if (category != null)
                        {
                            product.Category = category;
                        }
                    }
                }
            }
            else
            {
                list = new List<ProductDto>();
            }
            return Json(new { data = list });
        }
    }
}
