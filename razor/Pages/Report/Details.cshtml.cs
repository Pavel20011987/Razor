using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor.Models;
using Microsoft.AspNetCore.Authorization;


namespace razor.Pages.Report
{
    [Authorize(Roles = "Admin,User")]     
    public class DetailsModel : PageModel
    {
        private readonly razor.Data.ApplicationDbContext _context;

        public DetailsModel(razor.Data.ApplicationDbContext context)
        {
            _context = context;
        }
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
            SshConnect3 ssh = new SshConnect3();
            ViewData["SSH"] = ssh.sshCheck3(    NetworkAssignment.networkMngmnt,
                                                NetworkAssignment.Vlan.vlan,
                                                NetworkAssignment.Network.network);                  
            return Page();
        }
    }
}
