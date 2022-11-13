using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor.Models;

namespace razor.Pages.Networki
{
    public class DetailsModel : PageModel
    {
        private readonly razor.Data.ApplicationDbContext _context;

        public DetailsModel(razor.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public Network Network { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Network = await _context.Networks
                .Include(c =>c.Mask)                
                .FirstOrDefaultAsync(m => m.NetworkID == id);
            if (Network == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
