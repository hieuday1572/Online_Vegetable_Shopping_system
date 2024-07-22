using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Extension;
using PRN221_Project.Models;
using PRN221_Project.ModelViews;

namespace PRN221_Project.Pages
{
    [BindProperties]
    public class CheckoutModel : PageModel
    {
        private readonly PRN221_Project.Models.Project_PRN221Context _context;
        public MuaHangVM model { get; set; }
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
        public CheckoutModel(PRN221_Project.Models.Project_PRN221Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            model = new MuaHangVM();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CustomerEmail")))
            {
                TempData["checkout"] = true;
                return RedirectToPage("Login");
            }
            var accEmail = HttpContext.Session.GetString("CustomerEmail");
            if (accEmail != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(p => p.Email == accEmail);
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.FullName;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                model.Address = khachhang.Address;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                Order order = new Order
                {
                    CustomerId = model.CustomerId,
                    Address = model.Address,
                    OrderDate = DateTime.Now,
                    ShipDate = DateTime.Now.AddDays(3),
                    TransactStatusId = 1,
                    Total = (int)(cart.Sum(p => p.TotalMoney)) + 20000,
                };
                _context.Add(order);
                _context.SaveChanges();

                foreach(var item in cart)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = item.product.ProductId,
                        Quantity = item.amount,
                        Total = (int)item.TotalMoney,
                        ShipDate = DateTime.Now.AddDays(3)
                    };
                    _context.Add(orderDetail);
                }
                _context.SaveChanges();
                HttpContext.Session.Remove("cart");
                TempData["order_success"] = "You have placed your order successfully";
                return RedirectToPage("My_Order");
            }
            return Page();
        }
    }
}
