using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;

namespace PRN221_Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Project_PRN221Context _context;

        public IndexModel(Project_PRN221Context context)
        {
            _context = context;
        }
        public IList<Product> Product { get; set; }
        public IList<Category> Categories { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public IList<Product> bestSeller { get; set; }

        public async Task OnGetAsync(int? cate, int? id)
        {

            Categories = await _context.Categories.AsNoTracking().ToListAsync();
            bestSeller = await _context.Products.AsNoTracking().Where(p => p.BestSeller == true).Include(p => p.Ca).ToListAsync();
            int pageSize = 1; // số lượng mục trên mỗi trang
            CurrentPage = id ?? 1; // trang hiện tại
            CurrentPage = CurrentPage == 0 ? 1 : CurrentPage;
            var count = _context.Products.Count();
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            var skipAmount = (CurrentPage - 1) * pageSize;

            if (cate != null)
            {
                count = _context.Products.Where(p => p.CaId == cate).Count();
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                Product = await _context.Products.Where(p => p.CaId == cate).AsNoTracking().Include(p => p.Ca)
                .OrderBy(x => x.ProductId)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
                ViewData["cateTemp"] = cate;
            }
            else
            {
                Product = await _context.Products.AsNoTracking().Include(p => p.Ca)
                .OrderBy(x => x.ProductId)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
            }
        }
    }
}
