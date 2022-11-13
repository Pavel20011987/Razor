using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using razor.Models;
using System.Text.RegularExpressions;

namespace razor
{
    public class ArpToSql
    {
        private readonly razor.Data.ApplicationDbContext _context;
        public ArpToSql(razor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public NetworkAssignment NetworkAssignment { get; set; }

        public async Task OnGetAsyncArp()
        {
            Regex regex = new Regex(@"^\d{1,5}$");
            string sshResult=""; 
            SshConnect3 ssh = new SshConnect3();
            foreach(var item in _context.NetworkAssignments)
            {               
                string arp = ssh.sshCheck3( item.networkMngmnt,
                                            item.Vlan.vlan,
                                            item.Network.network);
                MatchCollection matches = regex.Matches(arp);
                        if (matches.Count > 0)
                        {
                            sshResult = arp;
                        }
                        else
                        {
                            sshResult = "Bad";
                        } 
                        item.arp = sshResult;
                        item.arpUpdate = DateTime.Now;   
                        if(sshResult == "Bad")    
                        {
                            item.arpInInt = 99999;
                        }                  
                        else
                        {
                            item.arpInInt = Convert.ToInt32(sshResult);
                        }
            }
            await _context.SaveChangesAsync();
        }
    }
}
