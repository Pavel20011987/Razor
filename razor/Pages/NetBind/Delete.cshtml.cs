using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor.Data;
using razor.Models;
using Microsoft.AspNetCore.Authorization;

namespace razor.Pages.NetBind
{
    [Authorize(Roles = "Admin,User")]      
    public class DeleteModel : PageModel
    {
        private readonly razor.Data.ApplicationDbContext _context;

        public DeleteModel(razor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public NetworkAssignment NetworkAssignment { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NetworkAssignment = await _context.NetworkAssignments
                .Include(n => n.Network)
                .Include(n => n.Network.Mask)
                .Include(n => n.Vendor)
                .Include(n => n.Vlan).FirstOrDefaultAsync(m => m.NetworkAssignmentID == id);

            if (NetworkAssignment == null)
            {
                return NotFound();
            }

            /* Данные для логирования */
            ViewData["Netw"] = NetworkAssignment.Network.network;
            ViewData["Mask"] = NetworkAssignment.Network.Mask.mask;
            ViewData["Vlan"] = NetworkAssignment.Vlan.vlan;
            ViewData["Vendor"] = NetworkAssignment.Vendor.vendor;
            ViewData["mngIP"] = NetworkAssignment.networkMngmnt;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(   int? id,
                                                        string Netw,
                                                        string Mask,
                                                        int Vlan,
                                                        string Vendor,
                                                        string mngIP)
        {
            if (id == null)
            {
                return NotFound();
            }

            NetworkAssignment = await _context.NetworkAssignments.FindAsync(id);

            if (NetworkAssignment != null)
            {
                foreach(Network net in _context.Networks){
                    if(net.NetworkID==NetworkAssignment.NetworkID)
                        net.InUseNet = false;
                }                
                _context.NetworkAssignments.Remove(NetworkAssignment);
                await _context.SaveChangesAsync();
                
                
                Logger log = new Logger();
                log.LogDelNetBind(  User.Identity.Name,
                    Netw,                               
                    Mask,
                    Vlan,
                    Vendor,
                    mngIP);   
            }

            return RedirectToPage("./Index");
        }
    }
}
