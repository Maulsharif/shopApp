namespace eShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public decimal Price { get; set; } = 0;
        public string Image { get; set; }="images/no-image.png";
      
    }
}