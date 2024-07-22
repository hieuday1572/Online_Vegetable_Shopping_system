using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Helpper;
using PRN221_Project.Models;

namespace PRN221_Project.Pages.ADMIN.Manage_Products
{
    public class EditModel : PageModel
    {
        private readonly PRN221_Project.Models.Project_PRN221Context _context;

        public EditModel(PRN221_Project.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products
                .Include(p => p.Ca).FirstOrDefaultAsync(m => m.ProductId == id);

            if (Product == null)
            {
                return NotFound();
            }
            ViewData["CaIdForEdit"] = new SelectList(_context.Categories, "CaId", "CaName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile? fThumb)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Product.ProductName = Utilities.ToTitleCase(Product.ProductName);
            if (fThumb != null)
            {
                string extension = Path.GetExtension(fThumb.FileName);
                string image = Utilities.SEOUrl(fThumb.FileName) + extension;
                Product.Thumb = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
            }
            Product.DateModified = DateTime.Now;
            _context.Update(Product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["success"] = "Edied successfully !";
            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
