using PRN221_Project.Models;

namespace PRN221_Project.ModelViews
{
    public class CartItem
    {
        public Product product { get; set; }
        public int amount { get; set; }
        public double TotalMoney => amount * product.Price;
    }
}
