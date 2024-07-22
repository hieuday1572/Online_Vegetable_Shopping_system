using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Project.Extension;
using PRN221_Project.Models;
using PRN221_Project.ModelViews;

namespace PRN221_Project.Pages
{
    public class ShoppingCartModel : PageModel
    {
        private readonly Project_PRN221Context _context;
        public List<CartItem> cart
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("cart");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }


        }
        private readonly INotyfService _notyf;
        public ShoppingCartModel(Project_PRN221Context context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public async Task<IActionResult> OnGetAsync(int? productId, int? amount, int? productIDRemove)
        {
            List<CartItem> _cart = cart;
            if (productId != null)
            {
                CartItem item = _cart.SingleOrDefault(p => p.product.ProductId == productId);
                if (item != null)
                {
                    if (amount.HasValue)
                    {
                        item.amount += amount.Value;
                    }
                    else
                    {
                        item.amount++;
                    }
                }
                else
                {
                    Product hh = _context.Products.SingleOrDefault(p => p.ProductId == productId);
                    item = new CartItem
                    {
                        amount = amount.HasValue ? amount.Value : 1,
                        product = hh
                    };
                    _cart.Add(item);
                }
                //HttpContext.Session.Set<List<CartItem>>("cart", _cart);
            }
            if (productIDRemove != null)
            {
                CartItem item = _cart.SingleOrDefault(p => p.product.ProductId == productIDRemove);
                if (item != null)
                {
                    _cart.Remove(item);
                }
                //HttpContext.Session.Set<List<CartItem>>("cart", _cart);
            }
            HttpContext.Session.SetInt32("count", _cart.Count);
            HttpContext.Session.Set<List<CartItem>>("cart", _cart);
            return Page();
        }
    }
}
