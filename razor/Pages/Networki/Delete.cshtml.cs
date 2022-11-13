using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor.Models;
using Microsoft.AspNetCore.Authorization;

namespace razor.Pages.Networki
{
    [Authorize(Roles = "Admin")]    
    public class DeleteModel : PageModel
    {
        private readonly razor.Data.ApplicationDbContext _context;
        private string oldNet="",oldMask="";
        private int oldMaskID = 0; 

        public DeleteModel(razor.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public Network NetworkOld { get; set; }           

        [BindProperty]
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
            NetworkOld = Network;

            if (Network == null)
            {
                return NotFound();
            }

            /*Для логов*/
            foreach(Network n in _context.Networks)
            {
                if(n.NetworkID == id){
                    oldNet = n.network;
                    oldMaskID = n.MaskID;
                }
            }             
            foreach(Mask m in _context.Masks)
            {
                if(m.MaskID == oldMaskID){
                    oldMask = m.mask;
                }
            }  
            ViewData["oldNet"]=oldNet;
            ViewData["oldMask"]=oldMask;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string oldNet, string oldMask)
        {
            if (id == null)
            {
                return NotFound();
            }
            //NetworkOld
            Network = await _context.Networks.FindAsync(id);

            if (Network != null)
            {
                _context.Networks.Remove(Network);
                await _context.SaveChangesAsync();
                Logger log = new Logger();
                log.LogDelNet(User.Identity.Name,oldNet,oldMask);
            }

            return RedirectToPage("./Index");
        }
    }
}
