using System;
using Renci.SshNet;
using System.Text.RegularExpressions;

namespace razor
{
    public class SshConnect3
    {
        public string sshCheck3(string ipMngmnt, int vlan, string net)
        {
            string[] ipNetParse = net.Split('.');
            string[] ipMngmntParse = ipMngmnt.Split('.');
            string ipNetGateway = ipNetParse[0]+"."+ipNetParse[1]+"."+ipNetParse[2]+".1";
            /* Norm NE */
            string ipNE = "172.21."+ipMngmntParse[2]+".100"; 
            int port_ssh_server=22;
            string ssh_user="viewerarp";
            string ssh_password="Al@d1N!YS";

            PasswordConnectionInfo connectionInfo = 
                new PasswordConnectionInfo( ipNE,
                                            port_ssh_server, 
                                            ssh_user,
                                            ssh_password);
            connectionInfo.Timeout = TimeSpan.FromSeconds(5);
            string rezultat = "";
            try
            {           
                using (var client = new SshClient(connectionInfo))
                {
                    client.Connect();
                    var outputNet = client.RunCommand("display ip interface "
                        +"description Vlanif "+vlan);
                    client.Disconnect();
                    string checkVlanAndNet = outputNet.Result.ToString();
                    Regex regexVlan = new Regex($@"{vlan}");
                    Regex regexNet = new Regex($@"{ipNetGateway}");
                    MatchCollection matchesVlan = regexVlan.Matches(checkVlanAndNet);
                    MatchCollection matchesNet = regexNet.Matches(checkVlanAndNet);
                    if (matchesVlan.Count > 0)
                    {
                        if (matchesNet.Count > 0)
                        { 
                            client.Connect();    
                            var outputArp = client.RunCommand("display arp "
                                +"interface Vlanif "+vlan+" | include Total");
                            client.Disconnect();
                            string checkArp = outputArp.Result.ToString();
                            Regex regex = new Regex(@"Total:\d{1,5}");
                            MatchCollection matches = regex.Matches(checkArp);
                            if (matches.Count > 0)
                            {
                                string[] arp;
                                foreach (Match match in matches)
                                {
                                    arp = match.Value.ToString().Split(":");
                                    rezultat = arp[1];      
                                }
                            }
                            else { rezultat = "Arp not Match"; }  
                        }    
                        else { rezultat = "Network not Match"; }
                    }    
                    else { rezultat = "Vlan not Match"; }
                }
                return rezultat;
            }
            catch(Exception e)
            {
                rezultat = e.Message;
                return rezultat;
            }
        }
    }
}