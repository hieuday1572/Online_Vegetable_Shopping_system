using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;

namespace PRN221_Project.Pages.ADMIN.Manage_Categories
{
    public class DetailsModel : PageModel
    {
        private readonly PRN221_Project.Models.Project_PRN221Context _context;

        public DetailsModel(PRN221_Project.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Categories.FirstOrDefaultAsync(m => m.CaId == id);

            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
