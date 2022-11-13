using System;
using System.IO;

namespace razor
{
    public class Logger 
    {
      //  string writePath = "log.txt";
        string text = "";

        public void WriterLog(string log)
        {
            using (StreamWriter sw = new StreamWriter("log.txt", true, System.Text.Encoding.Default))
            {
                sw.WriteLine(log);
            }   
        }
        public string ReaderLog()
        {
            try
            {
                using (StreamReader reader = File.OpenText("log.txt"))
                {
                    string fileContent = reader.ReadToEnd();
                    if (fileContent != null && fileContent != "")
                    {
                        return fileContent;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log
                return ex.ToString();
            }
            return null;
        }
        public void LogAddNet(string user, string net, string mask)
        {
            text = DateTime.UtcNow.ToString()+" Add network "+net+"/"+mask+" by "+user+";"; 
            WriterLog(text);
        }
        public void LogDelNet(string user, string net,string mask)
        {
            text = DateTime.UtcNow.ToString()+" Delete network "+net+"/"+mask+" by "+user+";"; 
            WriterLog(text);
        }
        public void LogEditNet(string user, string oldNet,string oldMask,
                                 string newNet,string newMask)
        {
            text = DateTime.UtcNow.ToString()+" Change network from "+oldNet
                                +"/"+oldMask+" to "+newNet+"/"+newMask+" by "+user+";"; 
            WriterLog(text);       
        }    
        public void LogAddNetBind(string user, string net, string mask,int vlan, 
                                    int allowIP, string vednor, string mngIp)
        {
            text = DateTime.UtcNow.ToString()+" Add NetBind "+net+"/"+mask
                +", vlan"+vlan+", vendor \""+vednor+"\", mngIP "+mngIp+" by "+user+";"; 
            WriterLog(text);
        }   
        public void LogDelNetBind(string user, string net, string mask,int vlan, 
                                    string vednor, string mngIp)
        {
            text = DateTime.UtcNow.ToString()+" Delete NetBind "+net+"/"+mask
                +", vlan"+vlan+", vendor \""+vednor+"\", mngIP "+mngIp+" by "+user+";"; 
            WriterLog(text);
        }   
        public void LogEditNetBind( string user, 
                                    string netOld,     
                                    string maskOld,
                                    int vlanOld, 
                                    string vednorOld, 
                                    string mngIpOld,
                                            string net,     
                                            string mask,
                                            int vlan, 
                                            string vednor, 
                                            string mngIp)
        {
            text = DateTime.UtcNow.ToString()+" Change NetBind by "+user
            +"\n\t OLD:"+netOld+"/"+maskOld+", vlan"+vlanOld+", vendor \""+vednorOld+"\", mngIP "+mngIpOld
            +"\n\t NEW:"+net+"/"+mask+", vlan"+vlan+", vendor \""+vednor+"\", mngIP "+mngIp+";"; 
            WriterLog(text);
        }   
        public void LogLogIn(string user)
        {
            text = DateTime.UtcNow.ToString()+" User:" +user+", Log In;"; 
            WriterLog(text);
        }      
        public void LogLogOut(string user)
        {
            text = DateTime.UtcNow.ToString()+" User:" +user+", Log Out;"; 
            WriterLog(text);
        }    
        public void LogStartTaskArp(string user)
        {
            text = DateTime.UtcNow.ToString()+" User:" +user+", Start Task Arp;"; 
            WriterLog(text);
        }         
    }
}
