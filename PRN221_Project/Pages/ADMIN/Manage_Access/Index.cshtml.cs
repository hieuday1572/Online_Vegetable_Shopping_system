using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;

namespace PRN221_Project.Pages.ADMIN.Manage_Access
{
    public class IndexModel : PageModel
    {
        private readonly PRN221_Project.Models.Project_PRN221Context _context;
        private readonly INotyfService _notyf;

        public IndexModel(PRN221_Project.Models.Project_PRN221Context context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IList<Role> Role { get;set; }

        public async Task OnGetAsync()
        {
            Role = await _context.Roles.ToListAsync();
            if (TempData["success"] != null)
            {
                _notyf.Success($"{TempData["success"]}");
            }
        }
    }
}
