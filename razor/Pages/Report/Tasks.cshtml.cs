using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace razor.Pages.Report
{
    [Authorize(Roles = "Admin")]    
    public class TasksModel : PageModel
    {
        private readonly razor.Data.ApplicationDbContext _context;
        public TasksModel(razor.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<NetworkAssignment> NetworkAssignment { get;set; }        
        

        [BindProperty]
        public Network Network { get; set; }   
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        { 
            /*Логирование*/
            Logger log = new Logger();
            log.LogStartTaskArp(  User.Identity.Name);  

            NetworkAssignment = await   _context.NetworkAssignments
                                        .Include(m => m.Network.Mask)
                                        .Include(n => n.Network)
                                        .Include(n => n.Vendor)
                                        .Include(n => n.Vlan).ToListAsync();        
            ArpToSql D = new  ArpToSql(_context); 
            await D.OnGetAsyncArp();          
            return RedirectToPage("./Index");            
        }  
    }
}
