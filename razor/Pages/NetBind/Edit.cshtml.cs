using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using razor.Data;
using razor.Models;
using Microsoft.AspNetCore.Authorization;

namespace razor.Pages.NetBind
{
    [Authorize(Roles = "Admin,User")]      
    public class EditModel : NetworkAssingPageModel
    {
        private readonly razor.Data.ApplicationDbContext _context;
        public EditModel(razor.Data.ApplicationDbContext context)
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

            /* Вызов метода для генерации не привязанных сетей */            
            NotChooseNetworkForEditkDDL(_context,NetworkAssignment);
            
            /* Передача свободных сетей на HTML */
            ViewData["NetworkID"] = networkNameSL;
            
            /* Старый код для вывода списка всех сетей */            
            //ViewData["NetworkID"] = new SelectList(_context.Networks, "NetworkID", "network");

            ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "vendor");
            ViewData["VlanID"] = new SelectList(_context.Vlans, "VlanID", "vlan");



            /* Для InUse отдает в форму ID старой сети */
         
            ViewData["tempNetworkID"] = NetworkAssignment.NetworkID; 


            /* Данные для логирования */
            ViewData["NetwOld"] = NetworkAssignment.Network.network;
            ViewData["MaskOld"] = NetworkAssignment.Network.Mask.mask;
            ViewData["VlanOld"] = NetworkAssignment.Vlan.vlan;
            ViewData["VendorOld"] = NetworkAssignment.Vendor.vendor;
            ViewData["mngIPOld"] = NetworkAssignment.networkMngmnt;

            return Page();
        }        
        public async Task<IActionResult> OnPostAsync(int tempNetworkID,
                                                        string NetwOld,
                                                        string MaskOld,
                                                        int VlanOld,
                                                        string VendorOld,
                                                        string mngIPOld)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {       
                /*Изменение позиции InUse в таблице Network*/
                if(NetworkAssignment.NetworkID != tempNetworkID)
                {
                    foreach(Network net in _context.Networks)
                    {
                        if(net.NetworkID == NetworkAssignment.NetworkID)
                            net.InUseNet = NetworkAssignment.InUse;
                        if(net.NetworkID == tempNetworkID)
                            net.InUseNet = false;
                    } 
                }
                
                /*Привязка текущей даты при сознании записи*/
                NetworkAssignment.EnrollmentDate = DateTime.UtcNow;  
                            
                _context.Attach(NetworkAssignment).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NetworkAssignmentExists(NetworkAssignment.NetworkAssignmentID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
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
            log.LogEditNetBind(  User.Identity.Name,
                                NetwOld,                               
                                MaskOld,
                                VlanOld,
                                VendorOld,
                                mngIPOld,
                                network,                               
                                mask,
                                vlan,
                                vendor,
                                NetworkAssignment.networkMngmnt);               

            return RedirectToPage("./Index");
        }

        private bool NetworkAssignmentExists(int id)
        {
            return _context.NetworkAssignments.Any(e => e.NetworkAssignmentID == id);
        }
    }
}
