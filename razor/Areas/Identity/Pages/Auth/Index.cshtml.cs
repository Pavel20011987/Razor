using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor.Data;
using razor.Models;
using razor.Pages;
using Microsoft.Extensions.Configuration; // для разбиения на страницы
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace razor.Pages.Auth
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
        public string UsernameSort { get; set; }   
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }  



        /* с разбиением на страницы  */
        public PaginatedList<IdentityUser> Usernames { get;set; }

        /* Без разбиения на страницы  */
        //public IList<Usernames> Usernames { get;set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {

        CurrentSort = sortOrder;            
        UsernameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
   
        if (searchString != null)
        {
            pageIndex = 1;
        }
        else
        {
            searchString = currentFilter;
        }

        CurrentFilter = searchString;

        IQueryable<IdentityUser> networkbindIQ =   from s in _context.Users
                                                        select s;




        if (!String.IsNullOrEmpty(searchString))
        {
            networkbindIQ = networkbindIQ.Where(s =>s.UserName.Contains(searchString));
        }


        switch (sortOrder)
        {
            case "name_desc":
                networkbindIQ = networkbindIQ.OrderByDescending(s => s.UserName);
                break;   
            default:
                networkbindIQ = networkbindIQ.OrderBy(s => s.UserName);
                break;
        }
            
        var pageSize = Configuration.GetValue("PageSize", 4);

            
        Usernames = await PaginatedList<IdentityUser>.CreateAsync(
            networkbindIQ
            .AsNoTracking(), pageIndex ?? 1, pageSize);            






        /* С сортировками  */
        /*             
        Usernames = await   networkbindIQ
                                    .Include(n => n.Network)
                                    .Include(m => m.Network.Mask)
                                    .Include(n => n.Vendor)
                                    .Include(n => n.Vlan).ToListAsync();    
        */        
        

        /* без сортировок */
        /*
        Usernames = await   _context.NetworkAssignments
                                    .Include(m => m.Network.Mask)
                                    .Include(n => n.Network)
                                    .Include(n => n.Vendor)
                                    .Include(n => n.Vlan).ToListAsync();
        */       
    
        }
    }
}
