using Mango.Web.Models;
using Mango.Web.Models.Statistics;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Mango.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;
        private readonly ITokenProvider _tokenProvider;
        public AdminController(IAuthService authService, IProductService productService, ICategoryService categoryService, IOrderService orderService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _productService = productService;
            _categoryService = categoryService;
            _orderService = orderService;
            _tokenProvider = tokenProvider;
        }

        [Authorize(Roles = SD.RoleAdmin)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = SD.RoleAdmin)]
        public async Task<IActionResult> AdminIndex()
        {
            List<ApplicationUser?> listCus = new();
            List<ProductDto?> listProduct = new();
            List<CategoryDto?> listCategory = new();
            List<OrderHeaderDto?> listOrder = new();
            List<OrderHeaderDto?> listOrderComplete = new();

            List<ApplicationUser?> listAdmin = new();
            ResponseDto? responseAdmin = await _authService.GetAllAdminUser();
            if (responseAdmin != null && responseAdmin.IsSuccess)
            {
                listAdmin = JsonConvert.DeserializeObject<List<ApplicationUser>>(Convert.ToString(responseAdmin.Result));
            }

            ResponseDto? responseCus = await _authService.GetAllCustomerUser();
            if (responseCus != null && responseCus.IsSuccess)
            {
                listCus = JsonConvert.DeserializeObject<List<ApplicationUser>>(Convert.ToString(responseCus.Result));
            }
            int customerCount = listCus.Count;

            ResponseDto? responseProduct = await _productService.GetAllProductsAsync();
            if (responseProduct != null && responseProduct.IsSuccess)
            {
                listProduct = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseProduct.Result));
            }
            int productCount = listProduct.Count;

            List<string> categoryName = new();
            List<int> productInCategory = new();
            ResponseDto? responseCategory = await _categoryService.GetAllCategoriesAsync();
            if (responseCategory != null && responseCategory.IsSuccess)
            {
                listCategory = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(responseCategory.Result));
            }
            int categoryCount = listCategory.Count;
            foreach(var category in listCategory)
            {
                int countProduct = 0;
                categoryName.Add(category.CategoryName);
                foreach(var product in listProduct)
                {
                    if(product.CategoryId == category.CategoryId) 
                    {
                        countProduct++;
                    }
                }
                productInCategory.Add(countProduct);
            }

            ResponseDto? responseOrder = await _orderService.GetAllOrders(listAdmin[0].Email);
            if (responseOrder != null && responseOrder.IsSuccess)
            {
                listOrder = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(responseOrder.Result));
            }
            int orderCount = 0;

            listOrderComplete = listOrder.Where(o => o.Status == SD.Status_Completed).ToList();
            orderCount = listOrderComplete.Count;

            List<OrderHeaderDto> listOrderToday = GetOrderToday(listOrderComplete);
            int countOrderToday = listOrderToday.Count();

            double todayEarning = listOrderToday.Sum(o => o.OrderTotal);
            int productSold = 0;
            foreach (var item in listOrderToday)
            {
                productSold += item.OrderDetails.Count();
            }

            List<List<OrderHeaderDto>> orderByMonth = GetOrdersByLast12Months(listOrderComplete);
            List<double> totalOrderByMonth = GetTotalIn12Months(orderByMonth);

            List<OrderHeaderDto> ordersLast7Days = GetOrdersByLast7Days(listOrderComplete);
            List<double> totalOrderByDay = GetTotalIn7Days(ordersLast7Days);
            List<List<OrderHeaderDto>> everyOrderIn7Days = GetOrdersEveryDayIn7Days(listOrderComplete);

            List<int> orderInOneDay = new List<int>();
            List<int> productInOneDay = new List<int>();
            foreach (var ordersInADay in everyOrderIn7Days)
            {
                orderInOneDay.Add(ordersInADay.Count);

                int totalProductsInDay = 0;
                foreach (var orderItem in ordersInADay)
                {
                    totalProductsInDay += orderItem.OrderDetails.Count();
                }
                productInOneDay.Add(totalProductsInDay);
            }

            var model = new AdminDashboardViewModel
            {
                CustomerCount = customerCount,
                ProductCount = productCount,
                CategoryCount = categoryCount,
                OrderCount = orderCount,
                TotalOrderToday = todayEarning,
                ProductSold = productSold,
                OrderToday = countOrderToday
            };

            ViewData["OrderChartLine"] = totalOrderByMonth;
            ViewData["OrderChartBar"] = totalOrderByDay;
            ViewData["OrderChart3Line"] = orderInOneDay;
            ViewData["ProductChart3Line"] = productInOneDay;
            ViewData["CategoryName"] = categoryName;
            ViewData["ProductInCategory"] = productInCategory;

            ViewBag.Top5CustomerByTotalPrice = await GetTop5CustomerByTotalPrice(listOrderComplete);
            return View(model);
        }

        public List<List<OrderHeaderDto>> GetOrdersByLast12Months(List<OrderHeaderDto> orderCompleted)
        {
            DateTime now = DateTime.Now;
            List<List<OrderHeaderDto>> ordersByMonth = new List<List<OrderHeaderDto>>();

            for (int i = 0; i < 12; i++)
            {
                DateTime monthStart = now.AddMonths(-11 + i);

                var ordersInMonth = orderCompleted
                    .Where(o => o.OrderTime.Month == monthStart.Month && o.OrderTime.Year == monthStart.Year)
                    .ToList();

                ordersByMonth.Add(ordersInMonth);
            }

            return ordersByMonth;
        }

        public List<List<OrderHeaderDto>> GetOrdersEveryDayIn7Days(List<OrderHeaderDto> orderCompleted)
        {
            DateTime lastWeek = DateTime.Now.AddDays(-7);

            List<List<OrderHeaderDto>> ordersBy7Days = new List<List<OrderHeaderDto>>();

            for (int i = 0; i < 7; i++)
            {
                DateTime dayStart = lastWeek.AddDays(i);

                var ordersInDay = orderCompleted
                    .Where(o => o.OrderTime.Day == dayStart.Day && o.OrderTime.Month == dayStart.Month && o.OrderTime.Year == dayStart.Year)
                    .ToList();

                ordersBy7Days.Add(ordersInDay);
            }

            return ordersBy7Days;
        }

        public List<double> GetTotalIn12Months(List<List<OrderHeaderDto>> ordersByMonth)
        {
            List<double> totalOrderInMonth = new List<double>();

            foreach (var orderList in ordersByMonth)
            {
                double monthlyTotal = orderList.Sum(order => order.OrderTotal);

                totalOrderInMonth.Add(monthlyTotal);
            }

            return totalOrderInMonth;
        }

        public List<OrderHeaderDto> GetOrdersByLast7Days(List<OrderHeaderDto> orderCompleted)
        {
            DateTime now = DateTime.Now;
            DateTime lastWeek = now.AddDays(-7);

            var ordersInLast7Days = orderCompleted
                .Where(o => o.OrderTime >= lastWeek && o.OrderTime <= now)
                .ToList();

            return ordersInLast7Days;
        }

        public List<double> GetTotalIn7Days(List<OrderHeaderDto> ordersInLast7Days)
        {
            List<double> totalOrderInDay = new List<double>();

            for (int i = 0; i < 7; i++)
            {
                DateTime day = DateTime.Now.AddDays(-6 + i);

                double dailyTotal = ordersInLast7Days
                    .Where(o => o.OrderTime.Date == day.Date)
                    .Sum(order => order.OrderTotal);

                totalOrderInDay.Add(dailyTotal);
            }

            return totalOrderInDay;
        }

        public List<OrderHeaderDto> GetOrderToday(List<OrderHeaderDto> orderCompleted)
        {
            DateTime today = DateTime.Today;

            var orderToday = orderCompleted
                .Where(o => o.OrderTime.Date == today)
                .ToList();

            return orderToday;
        }


        public async Task<List<TopPriceCustomerDto>> GetTop5CustomerByTotalPrice(List<OrderHeaderDto> listOrderComplete)
        {
            // Concurrent dictionaries for thread-safe operations
            var customerTotalOrder = new ConcurrentDictionary<string, double>();
            var customerInfo = new ConcurrentDictionary<string, UserDto>();

            // Populate customer totals and user information
            var tasks = listOrderComplete
                .Where(order => order.UserId != null)
                .Select(async order =>
                {
                    var userId = order.UserId!;
                    if (!customerInfo.ContainsKey(userId))
                    {
                        try
                        {
                            var userResponse = await _authService.GetUserById(userId);
                            if (userResponse?.IsSuccess == true)
                            {
                                var user = JsonConvert.DeserializeObject<UserDto>(Convert.ToString(userResponse.Result));
                                if (user != null)
                                {
                                    customerInfo.TryAdd(userId, user);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to retrieve user info for {userId}: {ex.Message}");
                        }
                    }

                    // Update the total order price for each customer
                    customerTotalOrder.AddOrUpdate(userId, order.OrderTotal, (key, total) => total + order.OrderTotal);
                });

            await Task.WhenAll(tasks);

            // Select top 5 customers by total order amount
            var top5Customers = customerTotalOrder
                .OrderByDescending(c => c.Value)
                .Take(5)
                .Select(c => new TopPriceCustomerDto
                {
                    ID = c.Key,
                    User = customerInfo[c.Key],
                    TotalPrice = c.Value,
                    OrderHeaders = listOrderComplete.Where(o => o.UserId == c.Key).ToList()
                })
                .ToList();

            return top5Customers;
        }



    }
}
