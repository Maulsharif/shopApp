using System;

namespace eShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int ProductId { get; set; }
        public  virtual Product Product { get; set; }
        public int Quantity { get; set; } = 0;

    }
}