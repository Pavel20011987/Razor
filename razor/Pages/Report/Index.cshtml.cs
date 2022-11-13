using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;


namespace razor.Pages.Report
{
    [Authorize(Roles = "Admin,User")]  
    public class IndexModel : PageModel
    {
        private readonly razor.Data.ApplicationDbContext _context;       

        public IndexModel(razor.Data.ApplicationDbContext context)
        {
            _context = context;           
        }
        public IList<NetworkAssignment> NetworkAssignment { get;set; }

        public string NetworkSort { get; set; }
        public string VlanSort { get; set; }           
        public string AllocatedIPSort { get; set; }
        public string ArpSort { get; set; } 
        public string DateSort { get; set; }  
        public string UtilizationIPSort { get; set; }          
          

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }         

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString)
        {  
            CurrentSort = sortOrder;            
            NetworkSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            VlanSort = sortOrder == "vlan" ? "vlan_desc" : "vlan";         
            AllocatedIPSort = sortOrder == "allocIP" ? "allocIP_desc" : "allocIP";
            ArpSort = sortOrder == "arp" ? "arp_desc" : "arp";         
            UtilizationIPSort = sortOrder == "util" ? "util_desc" : "util";              
            DateSort = sortOrder == "date" ? "date_desc" : "date";   
        
            IQueryable<NetworkAssignment> reportIQ =   from s in _context.NetworkAssignments
                                                            select s;
            /* Фильтр поиска */
            if (!String.IsNullOrEmpty(searchString))
            {
                reportIQ = reportIQ.Where(s =>s.Network.network.Contains(searchString));
            }
            /*Выбор сортировки*/
            switch (sortOrder)
            {
                case "name_desc":
                    reportIQ = reportIQ.OrderByDescending(s => s.Network.network);
                    break;
                case "allocIP":
                    reportIQ = reportIQ.OrderBy(s => s.Network.allocatedIP);
                    break;
                case "allocIP_desc":
                    reportIQ = reportIQ.OrderByDescending(s => s.Network.allocatedIP);
                    break;
                case "date":
                    reportIQ = reportIQ.OrderBy(s => s.arpUpdate);
                    break;
                case "date_desc":
                    reportIQ = reportIQ.OrderByDescending(s => s.arpUpdate);
                    break;  
                case "arp":
                    reportIQ = reportIQ.OrderBy(s => s.arpInInt);
                    break;
                case "arp_desc":
                    reportIQ = reportIQ.OrderByDescending(s => s.arpInInt);
                    break; 

                case "util":
                    reportIQ = reportIQ.OrderBy(s => (Convert.ToDouble(s.arpInInt)/Convert.ToDouble(s.Network.allocatedIP))*100);
                    break;
                case "util_desc":
                    reportIQ = reportIQ.OrderByDescending(s => (Convert.ToDouble(s.arpInInt)/Convert.ToDouble(s.Network.allocatedIP))*100);
                    break; 

                case "vlan":
                    reportIQ = reportIQ.OrderBy(s => s.Vlan.vlan);
                    break;
                case "vlan_desc":
                    reportIQ = reportIQ.OrderByDescending(s => s.Vlan.vlan);
                    break;                 
                default:
                    reportIQ = reportIQ.OrderBy(s => s.Network.network);
                    break;
            }          


            NetworkAssignment = await   reportIQ
                                        .Include(m => m.Network.Mask)
                                        .Include(n => n.Network)
                                        .Include(n => n.Vendor)
                                        .Include(n => n.Vlan).ToListAsync();            


        }
    }
}
