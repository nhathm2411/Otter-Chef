namespace Mango.Web.Models.Statistics
{
    public class TopPriceCustomerDto
    {
        public string ID { get; set; }
        public UserDto User { get; set; }
        public List<OrderHeaderDto> OrderHeaders { get; set; }
        public int OrderCount => OrderHeaders?.Count ?? 0;  // Derived property to count orders
        public double TotalPrice { get; set; }
    }

}
