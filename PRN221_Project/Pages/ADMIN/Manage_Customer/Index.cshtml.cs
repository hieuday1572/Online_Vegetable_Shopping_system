using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;

namespace PRN221_Project.Pages.ADMIN.Manage_Customer
{
    public class IndexModel : PageModel
    {
        private readonly PRN221_Project.Models.Project_PRN221Context _context;
        private readonly INotyfService _notyf;
        public IList<Customer> customers { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public IndexModel(PRN221_Project.Models.Project_PRN221Context context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }


        public async Task OnGetAsync(int? id, string? search, bool? active, int?id2)
        {
            if (id2 != null)
            {
                var account = await _context.Customers.Where(p => p.CustomerId == id2).FirstOrDefaultAsync();
                if (active == true)
                {
                    account.Active = false;
                }
                else
                {
                    account.Active = true;
                }
                _context.Update(account);
                await _context.SaveChangesAsync();
                TempData["success"] = "Edited successfully !";
            }
            int pageSize = 10; // số lượng mục trên mỗi trang
            CurrentPage = id ?? 1; // trang hiện tại
            CurrentPage = (CurrentPage == 0) ? 1 : CurrentPage;
            var count = _context.Customers.Count();
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            var skipAmount = (CurrentPage - 1) * pageSize;

            if (!string.IsNullOrEmpty(search))
            {
                count = _context.Customers.Where(p => p.Email.Contains(search.Trim())).Count();
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                customers = await _context.Customers.Include(p => p.Location).Where(p => p.Email.Contains(search.Trim()))
                .OrderBy(x => x.CustomerId)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();                
                ViewData["search"] = search;
            }
            else
            {
                customers = await _context.Customers.Include(p => p.Location)
                .OrderBy(x => x.CustomerId)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
            }
            if (TempData["success"] != null)
            {
                _notyf.Success($"{TempData["success"]}");
            }
        }
    }
}
