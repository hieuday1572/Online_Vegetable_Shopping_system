using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN221_Project.Models;

namespace PRN221_Project.Pages.ADMIN.Manage_Access
{
    public class CreateModel : PageModel
    {
        private readonly PRN221_Project.Models.Project_PRN221Context _context;

        public CreateModel(PRN221_Project.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Role Role { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                _context.Roles.Add(Role);
                await _context.SaveChangesAsync();
                TempData["success"] = "Created successfully !";
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
