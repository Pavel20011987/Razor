using razor.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using razor.Models;

namespace razor.Pages.NetBind
{
    public class NetworkAssingPageModel : PageModel
    {
        protected SelectList networkNameSL { get; set; }
        public void NotChooseNetworkDropDownList(ApplicationDbContext _context, 
            object selectedNetwork = null)
        {
            /* Создание запросов в таблицы для выборки не привязанных сетей*/            
            var assingNetworksQuery = from net in _context.Networks
                                from netAssing in _context.NetworkAssignments
                                where net.NetworkID == netAssing.NetworkID            
                                orderby net.network // Sort by name.
                                select net;  
            var fullNetworksQuery = from net in _context.Networks
                                orderby net.network // Sort by name.
                                select net; 
            var resultNetworksQuery = fullNetworksQuery.Except(assingNetworksQuery); 

            networkNameSL = new SelectList(resultNetworksQuery.AsNoTracking(),
                        "NetworkID", "network", selectedNetwork);                        
        } 

        public void NotChooseNetworkForEditkDDL(ApplicationDbContext _context, 
            NetworkAssignment chooseNA, object selectedNetwork = null)
        {
            /* Создание запросов в таблицы для выборки не привязанных сетей 
                с текущей сетью для EDIT*/            
            var assingNetworksQuery = from net in _context.Networks
                                from netAssing in _context.NetworkAssignments
                                where ( (net.NetworkID != chooseNA.NetworkID) && 
                                        (net.NetworkID == netAssing.NetworkID))
                                orderby net.network // Sort by name.
                                select net;  
            var fullNetworksQuery = from net in _context.Networks
                                orderby net.network // Sort by name.
                                select net; 
            var resultNetworksQuery = fullNetworksQuery.Except(assingNetworksQuery); 

            networkNameSL = new SelectList(resultNetworksQuery.AsNoTracking(),
                        "NetworkID", "network", selectedNetwork);
        }         
    }
}