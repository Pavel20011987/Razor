using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using razor.Data;
using razor.Models;
using Microsoft.AspNetCore.Authorization;

namespace razor.Pages.NetBind
{
    [Authorize(Roles = "Admin,User")]    
    public class CreateModel : NetworkAssingPageModel
    {
        
        private readonly razor.Data.ApplicationDbContext _context;

        public CreateModel(razor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            /* Вызов метода для генерации не привязанных сетей */
            NotChooseNetworkDropDownList(_context);
            /* Передача свободных сетей на HTML */
            ViewData["NetworkID"] = networkNameSL;     

            /* Старый код для вывода списка всех сетей */
            //ViewData["NetworkID"] = new SelectList(resultNetworksQuery, "NetworkID", "network");           

  
            ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "vendor");
            ViewData["VlanID"] = new SelectList(_context.Vlans, "VlanID", "vlan");

            return Page();
        }

        [BindProperty]
        public NetworkAssignment NetworkAssignment { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            /*Цикл для передачи флага InUse в таблицу Network*/
            foreach(Network net in _context.Networks){
                if(net.NetworkID==NetworkAssignment.NetworkID)
                    net.InUseNet = NetworkAssignment.InUse;
            }
            /*Привязка текущей даты при сознании записи*/
            NetworkAssignment.EnrollmentDate = DateTime.UtcNow;
            
            _context.NetworkAssignments.Add(NetworkAssignment);
            await _context.SaveChangesAsync();
            
            /* Для логирования связка таблиц */
            Logger log = new Logger();
            string network="", mask="",vendor="";
            int vlan=0,allowIp=0, maskId=0;

            foreach(Network net in _context.Networks)
            {
                if(net.NetworkID == NetworkAssignment.NetworkID)
                    {
                        network = net.network;
                        allowIp = net.allocatedIP;
                        maskId = net.MaskID;
                    }
            }
            foreach(Mask m in _context.Masks)
            {
                if(m.MaskID == maskId)
                        mask = m.mask;
            }    
            foreach(Vlan v in _context.Vlans)
            {
                if(v.VlanID == NetworkAssignment.VlanID)
                        vlan = v.vlan;
            }  
            foreach(Vendor v in _context.Vendors)
            {
                if(v.VendorID == NetworkAssignment.VendorID)
                        vendor = v.vendor;
            }                                   

            log.LogAddNetBind(  User.Identity.Name,
                                network,                               
                                mask,
                                vlan,
                                allowIp,
                                vendor,
                                NetworkAssignment.networkMngmnt);                        

            return RedirectToPage("./Index");
        }
    }
}
