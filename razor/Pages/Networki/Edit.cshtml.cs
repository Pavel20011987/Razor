using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using razor.Models;
using Microsoft.AspNetCore.Authorization;

namespace razor.Pages.Networki
{
    [Authorize(Roles = "Admin")]    
    public class EditModel : NetworkNamePageModel
    {
        private readonly razor.Data.ApplicationDbContext _context; 
        public EditModel(razor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Network Network { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Network = await _context.Networks
                .Include(c => c.Mask)
                .FirstOrDefaultAsync(m => m.NetworkID == id);

            if (Network == null)
            {
                return NotFound();
            }
            PopulateMasksDropDownList(_context);
          
            /*Для логов*/         
            ViewData["oldNet"]=Network.network;
            ViewData["oldMask"]=Network.Mask.mask;

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string oldMask,string oldNet)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Network).State = EntityState.Modified;
            string newMask="";
            try
            {
                await _context.SaveChangesAsync();
                Logger log = new Logger();
                
                foreach(Mask m in _context.Masks)
                {
                    if(m.MaskID==Network.MaskID)
                        newMask = m.mask;
                }
                log.LogEditNet(User.Identity.Name,oldNet,oldMask ,Network.network,newMask);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NetworkExists(Network.NetworkID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./Index");
        }
        private bool NetworkExists(int id)
        {
            return _context.Networks.Any(e => e.NetworkID == id);
        }
    }
}
