using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;

namespace PRN221_Project.Pages.ADMIN.Manage_Location
{
    public class IndexModel : PageModel
    {
        private readonly PRN221_Project.Models.Project_PRN221Context _context;
        private readonly INotyfService _notyf;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public IndexModel(PRN221_Project.Models.Project_PRN221Context context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IList<Location> Location { get;set; }

        public async Task OnGetAsync(int? id, string? search)
        {
            int pageSize = 10; // số lượng mục trên mỗi trang
            CurrentPage = id ?? 1; // trang hiện tại
            CurrentPage = (CurrentPage == 0) ? 1 : CurrentPage;
            var count = _context.Locations.Count();
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            var skipAmount = (CurrentPage - 1) * pageSize;
            if (!string.IsNullOrEmpty(search))
            {
                count = _context.Locations.Where(p => p.Name.Contains(search.Trim())).Count();
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                Location = await _context.Locations.Where(p => p.Name.Contains(search.Trim()))
                .OrderBy(x => x.LocationId)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();               
                ViewData["search"] = search;
            }
            else
            {
                Location = await _context.Locations.OrderBy(x => x.LocationId)
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
