using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;

namespace PRN221_Project.Pages
{
    public class ShopModel : PageModel
    {
        private readonly Project_PRN221Context _context;

        public ShopModel(Project_PRN221Context context)
        {
            _context = context;
        }

        public IList<Product> Product { get; set; }
        public IList<Category> Categories { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGetAsync(string? search, int? id, int? cateFillter, int? PriceOrder)
        {
            Categories = await _context.Categories.AsNoTracking().ToListAsync();
            int pageSize = 10; // số lượng mục trên mỗi trang
            CurrentPage = id ?? 1; // trang hiện tại
            CurrentPage = CurrentPage == 0 ? 1 : CurrentPage;
            var count = _context.Products.Count();
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            var skipAmount = (CurrentPage - 1) * pageSize;

            //Category
            if (cateFillter != null)
            {
                count = _context.Products.Where(p => p.CaId == cateFillter).Count();
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                if (PriceOrder != null)
                {
                    if (PriceOrder == 0)
                    {
                        Product = await _context.Products.Where(p => p.CaId == cateFillter).AsNoTracking().Include(p => p.Ca)
                .OrderBy(p => p.Price)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
                        ViewData["PriceOrder"] = PriceOrder;
                        ViewData["cateTemp"] = cateFillter;
                    }
                    else
                    {
                        Product = await _context.Products.Where(p => p.CaId == cateFillter).AsNoTracking().Include(p => p.Ca)
                .OrderByDescending(p => p.Price)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
                        ViewData["PriceOrder"] = PriceOrder;
                        ViewData["cateTemp"] = cateFillter;
                    }
                }
                else
                {
                    Product = await _context.Products.Where(p => p.CaId == cateFillter).AsNoTracking().Include(p => p.Ca)
                .OrderBy(x => x.ProductId)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
                    ViewData["cateTemp"] = cateFillter;
                }

            }

            //Search
            if (!string.IsNullOrEmpty(search))
            {
                count = _context.Products.Where(p => p.ProductName.Contains(search.Trim())).Count();
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                if (PriceOrder != null)
                {
                    if (PriceOrder == 0)
                    {
                        Product = await _context.Products.Where(p => p.ProductName.Contains(search)).AsNoTracking().Include(p => p.Ca)
                .OrderBy(p => p.Price)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
                        ViewData["PriceOrder"] = PriceOrder;
                        ViewData["search"] = search;
                    }
                    else
                    {
                        Product = await _context.Products.Where(p => p.ProductName.Contains(search)).AsNoTracking().Include(p => p.Ca)
                .OrderByDescending(p => p.Price)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
                        ViewData["PriceOrder"] = PriceOrder;
                        ViewData["search"] = search;
                    }
                }
                else
                {
                    Product = await _context.Products.Where(p => p.ProductName.Contains(search.Trim())).AsNoTracking().Include(p => p.Ca)
                .OrderBy(x => x.ProductId)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
                    ViewData["search"] = search;
                }
            }

            if (cateFillter == null && string.IsNullOrEmpty(search))
            {
                if (PriceOrder != null)
                {
                    if (PriceOrder == 0)
                    {
                        Product = await _context.Products.AsNoTracking().Include(p => p.Ca)
                .OrderBy(x => x.Price)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
                        ViewData["PriceOrder"] = PriceOrder;
                    }
                    else
                    {
                        Product = await _context.Products.AsNoTracking().Include(p => p.Ca)
                .OrderByDescending(x => x.Price)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
                        ViewData["PriceOrder"] = PriceOrder;
                    }
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
}
