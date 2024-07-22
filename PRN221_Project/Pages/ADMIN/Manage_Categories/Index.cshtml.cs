﻿using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;

namespace PRN221_Project.Pages.ADMIN.Manage_Categories
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

        public IList<Category> Category { get;set; }

        public async Task OnGetAsync(string? search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                Category = await _context.Categories.Where(p => p.CaName.Contains(search))
                .OrderBy(x => x.CaId)
                .ToListAsync();
            }
            else
            {
                Category = await _context.Categories.ToListAsync();
            }

            if (TempData["success"] != null)
            {
                _notyf.Success($"{TempData["success"]}");
            }
        }
    }
}
