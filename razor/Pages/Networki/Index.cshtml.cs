using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor.Models;
using Microsoft.Extensions.Configuration; // для разбиения на страницы
using Microsoft.AspNetCore.Authorization;

namespace razor.Pages.Networki
{
    [Authorize(Roles = "Admin")]
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
        public string NamodizmSort { get; set; }
        public string InUseNetSort { get; set; }        
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<Network> Networks { get; set; }
        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            
            CurrentSort = sortOrder;            
            NetworkSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            AllocatedIPSort = sortOrder == "allocIP" ? "allocIP_desc" : "allocIP";
            NamodizmSort = sortOrder == "nomad" ? "nomad_desc" : "nomad";
            InUseNetSort = sortOrder == "inuse" ? "inuse_desc" : "inuse";   

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Network> networksIQ =    from s in _context.Networks
                                                select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                networksIQ = networksIQ.Where(s => s.network.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    networksIQ = networksIQ.OrderByDescending(s => s.network);
                    break;
                case "allocIP":
                    networksIQ = networksIQ.OrderBy(s => s.allocatedIP);
                    break;
                case "allocIP_desc":
                    networksIQ = networksIQ.OrderByDescending(s => s.allocatedIP);
                    break;
                case "nomad":
                    networksIQ = networksIQ.OrderBy(s => s.Nomadizm);
                    break;
                case "nomad_desc":
                    networksIQ = networksIQ.OrderByDescending(s => s.Nomadizm);
                    break;
                case "inuse":
                    networksIQ = networksIQ.OrderBy(s => s.InUseNet);
                    break;
                case "inuse_desc":
                    networksIQ = networksIQ.OrderByDescending(s => s.InUseNet);
                    break;  
                default:
                    networksIQ = networksIQ.OrderBy(s => s.network);
                    break;
            }   

            var pageSize = Configuration.GetValue("PageSize", 4); 
            
            Networks = await PaginatedList<Network>.CreateAsync(
                networksIQ.Include(c => c.Mask).AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}