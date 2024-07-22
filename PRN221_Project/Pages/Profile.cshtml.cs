using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;
using PRN221_Project.ModelViews;

namespace PRN221_Project.Pages
{
    [BindProperties]
    public class ProfileModel : PageModel
    {
        private readonly Project_PRN221Context _context;
        public Customer Customer { get; set; }
        private readonly INotyfService _notyf;
        public ProfileModel(Project_PRN221Context context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }   
        public void OnGet()
        {
            var cusEmail = HttpContext.Session.GetString("CustomerEmail");
            Customer = _context.Customers.Include(p => p.Location).Where(p => p.Email == cusEmail).SingleOrDefault();
            ViewData["LocationIdForEdit"] = new SelectList(_context.Locations, "LocationId", "Name");
        }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Update(Customer);
                _context.SaveChanges();
                //_notyf.Success("Edied successfully !");
                TempData["success"] = "Edited successfully !";
            }
        }
    }
}
