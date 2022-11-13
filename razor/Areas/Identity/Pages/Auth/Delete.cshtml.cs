using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor.Data;
using razor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace razor.Pages.Auth
{
    [Authorize(Roles = "Admin")]    
    public class DeleteModel : PageModel
    {
        private readonly razor.Data.ApplicationDbContext _context;

        public DeleteModel(razor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IdentityUser Username { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Username = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Username == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Username = await _context.Users.FindAsync(id);

            if (Username != null)
            {               
                _context.Users.Remove(Username);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
