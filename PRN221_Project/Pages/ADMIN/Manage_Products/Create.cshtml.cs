using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN221_Project.Helpper;
using PRN221_Project.Models;

namespace PRN221_Project.Pages.ADMIN.Manage_Products
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
            ViewData["CaId"] = new SelectList(_context.Categories, "CaId", "CaName");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile fThumb)
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

            if (string.IsNullOrEmpty(Product.Thumb))
            {
                Product.Thumb = "default.jpg";
            }
            Product.DateCreated = DateTime.Now;
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();
            TempData["success"] = "Created successfully !";
            return RedirectToPage("./Index");
        }
    }
}

