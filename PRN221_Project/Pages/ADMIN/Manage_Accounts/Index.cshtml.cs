using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;

namespace PRN221_Project.Pages.ADMIN.Manage_Accounts
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

        public IList<Account> Accounts { get;set; }
        public IList<Role> Roles { get;set; }
        public async Task OnGetAsync(int? roleFillter, int? id, bool? active)
        {
            if(id!=null)
            {
                var account = await _context.Accounts.Where(p => p.AccountId == id).FirstOrDefaultAsync();
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
            Roles = await _context.Roles.ToListAsync();
            if(roleFillter != null)
            {
                Accounts = await _context.Accounts.Include(p => p.Role).Where(p =>p.RoleId == roleFillter).ToListAsync();
            }
            else
            {
                Accounts = await _context.Accounts.Include(p => p.Role).ToListAsync();
            }
                      
            if (TempData["success"] != null)
            {
                _notyf.Success($"{TempData["success"]}");
            }
        }
    }
}
