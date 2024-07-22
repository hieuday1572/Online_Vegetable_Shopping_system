using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;
using PRN221_Project.ModelViews;

namespace PRN221_Project.Pages
{
    [BindProperties]
    public class RegisterModel : PageModel
    {
        private readonly PRN221_Project.Models.Project_PRN221Context _context;
        public RegisterVM register { get; set; }
        public RegisterModel(PRN221_Project.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var emailCheck = _context.Customers.AsNoTracking().SingleOrDefault(p => p.Email.ToLower() == register.Email.ToLower());
            var max = _context.Customers.Select(p => p.CustomerId).Max();
            if (emailCheck != null)
            {
                ModelState.AddModelError("register.Email", "The Email already exists !");
            }
            if (ModelState.IsValid)
            {
                Customer customer = new Customer
                {
                    CustomerId = ++max,
                    FullName = register.FullName,
                    Email = register.Email,
                    Phone = register.Phone,
                    Password = register.Password,
                    CreateDate = DateTime.Now,
                    Active = true
                };
                _context.Customers.Add(customer);
                _context.SaveChanges();
                HttpContext.Session.SetString("CustomerEmail", customer.Email);
                return RedirectToPage("/User/HomePage");
            }
            return Page();
        }
    }
}
