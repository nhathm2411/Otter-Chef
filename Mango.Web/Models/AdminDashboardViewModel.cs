namespace Mango.Web.Models
{
    public class AdminDashboardViewModel
    {
        public int CustomerCount { get; set; }
        public int ProductCount { get; set; }
        public int CategoryCount { get; set; }
        public int OrderCount { get; set; }
        public double TotalOrderToday { get; set; }
        public int ProductSold { get; set; }
        public int OrderToday { get; set; }
    }
}
