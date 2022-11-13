using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor.Models;
using Microsoft.Extensions.Configuration; // для разбиения на страницы
using Microsoft.AspNetCore.Authorization;

namespace razor.Pages.NetBind
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly razor.Data.ApplicationDbContext _context;
        private readonly IConfiguration Configuration;  // для разбиения на страницы          

        public IndexModel(razor.Data.ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;   // для разбиения на страницы             
        }
        public string NetworkSort { get; set; }
        public string AllocatedIPSort { get; set; }
        public string VendorSort { get; set; }
        public string DateSort { get; set; }  
        public string MngmntIPSort { get; set; }           
        public string VlanSort { get; set; }  
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; } 

        /* с разбиением на страницы  */
        public PaginatedList<NetworkAssignment> NetworkAssignment { get;set; }
        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
        CurrentSort = sortOrder;            
        NetworkSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        AllocatedIPSort = sortOrder == "allocIP" ? "allocIP_desc" : "allocIP";
        VendorSort = sortOrder == "vendor" ? "vendor_desc" : "vendor";
        DateSort = sortOrder == "date" ? "date_desc" : "date";   
        MngmntIPSort = sortOrder == "mngmnt" ? "mngmnt_desc" : "mngmnt";         
        VlanSort = sortOrder == "vlan" ? "vlan_desc" : "vlan";           

        if (searchString != null)
        {
            pageIndex = 1;
        }
        else
        {
            searchString = currentFilter;
        }
        CurrentFilter = searchString;

        IQueryable<NetworkAssignment> networkbindIQ =   from s in _context.NetworkAssignments
                                                        select s;
        /* Фильтр поиска */
        if (!String.IsNullOrEmpty(searchString))
        {
            networkbindIQ = networkbindIQ.Where(s =>s.Network.network.Contains(searchString)
                                                    || s.networkMngmnt.Contains(searchString));
        }
        /*Выбор сортировки*/
        switch (sortOrder)
        {
            case "name_desc":
                networkbindIQ = networkbindIQ.OrderByDescending(s => s.Network.network);
                break;
            case "allocIP":
                networkbindIQ = networkbindIQ.OrderBy(s => s.Network.allocatedIP);
                break;
            case "allocIP_desc":
                networkbindIQ = networkbindIQ.OrderByDescending(s => s.Network.allocatedIP);
                break;
            case "vendor":
                networkbindIQ = networkbindIQ.OrderBy(s => s.Vendor);
                break;
            case "vendor_desc":
                networkbindIQ = networkbindIQ.OrderByDescending(s => s.Vendor);
                break;
            case "date":
                networkbindIQ = networkbindIQ.OrderBy(s => s.EnrollmentDate);
                break;
            case "date_desc":
                networkbindIQ = networkbindIQ.OrderByDescending(s => s.EnrollmentDate);
                break;  
            case "mngmnt":
                networkbindIQ = networkbindIQ.OrderBy(s => s.networkMngmnt);
                break;
            case "mngmnt_desc":
                networkbindIQ = networkbindIQ.OrderByDescending(s => s.networkMngmnt);
                break; 
            case "vlan":
                networkbindIQ = networkbindIQ.OrderBy(s => s.Vlan.vlan);
                break;
            case "vlan_desc":
                networkbindIQ = networkbindIQ.OrderByDescending(s => s.Vlan.vlan);
                break;                 
            default:
                networkbindIQ = networkbindIQ.OrderBy(s => s.Network.network);
                break;
        }            
        var pageSize = Configuration.GetValue("PageSize", 4);
            
        NetworkAssignment = await PaginatedList<NetworkAssignment>.CreateAsync(
            networkbindIQ
            .Include(n => n.Network)
            .Include(m => m.Network.Mask)
            .Include(n => n.Vendor)
            .Include(n => n.Vlan).AsNoTracking(), pageIndex ?? 1, pageSize); 
        }
    }
}
