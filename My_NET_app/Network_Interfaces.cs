using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Threading.Tasks;

namespace My_NET_app
{
    class Network_Interfaces
    {

        static IPGlobalProperties computerProperties;
        static NetworkInterface[] nics;
        public bool Full_List_of_interface { get; set; }
        private object sender=null;

        public event Action<object, List<string>> OnNewData;

        public Network_Interfaces()
        {
            computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            nics = NetworkInterface.GetAllNetworkInterfaces();
            
            Worker(sender, EventArgs.Empty);
        }

        static List<string> Get_NetworkInterface(bool Full_List_of_interface)
        {
            var List = new List<string>();
            List.Add("Interface information for     " + computerProperties.HostName.ToString() + computerProperties.DomainName.ToString() + System.Environment.NewLine);
            if (nics == null || nics.Length < 1)
            {
                List.Add("  No network interfaces found.");
                return List;
            }

            List.Add("  Number of interfaces .................... : " + nics.Length.ToString() + System.Environment.NewLine + System.Environment.NewLine);
            if (Full_List_of_interface)
            {
                foreach (NetworkInterface adapter in nics)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    //Console.WriteLine();
                    List.Add(adapter.Description.ToString() + System.Environment.NewLine);
                    List.Add("=================================================" + System.Environment.NewLine);
                    List.Add("  Physical Address ........................ : " + adapter.GetPhysicalAddress().ToString() + System.Environment.NewLine);
                    List.Add("  Operational status ...................... : " + adapter.OperationalStatus.ToString() + System.Environment.NewLine);
                    string versions = "";
                    foreach (IPAddressInformation unicast in properties.UnicastAddresses)
                    {
                        List.Add("  IP address ...................... : " + unicast.Address + System.Environment.NewLine);
                    }
                    // Create a display string for the supported IP versions.
                    if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                    {
                        versions = "IPv4";
                    }
                    if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                    {
                        if (versions.Length > 0)
                        {
                            versions += " ";
                        }
                        versions += "IPv6";
                    }

                    if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    {
                        continue;
                    }
                    IPAddressCollection dnsAddresses = properties.DnsAddresses;

                    foreach (IPAddress dnsAdress in dnsAddresses)
                    {
                        List.Add("  DNS address .............................. : " + dnsAdress + System.Environment.NewLine);
                    }
                    List.Add(System.Environment.NewLine);
                }
            }
            else
            {
                foreach (NetworkInterface adapter in nics)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();

                    if (adapter.OperationalStatus.ToString().Equals("Up"))
                    {
                        List.Add(adapter.Description.ToString() + System.Environment.NewLine);
                        List.Add("=================================================" + System.Environment.NewLine);
                        List.Add("  Physical Address ........................ : " + adapter.GetPhysicalAddress().ToString() + System.Environment.NewLine);
                        List.Add("  Operational status ...................... : " + adapter.OperationalStatus.ToString() + System.Environment.NewLine);
                        string versions = "";
                        foreach (IPAddressInformation unicast in properties.UnicastAddresses)
                        {
                            List.Add("  IP address ...................... : " + unicast.Address + System.Environment.NewLine);
                        }
                        // Create a display string for the supported IP versions.
                        if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                        {
                            versions = "IPv4";
                        }
                        if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                        {
                            if (versions.Length > 0)
                            {
                                versions += " ";
                            }
                            versions += "IPv6";
                        }
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                        {
                            continue;
                        }
                        IPAddressCollection dnsAddresses = properties.DnsAddresses;

                        foreach (IPAddress dnsAdress in dnsAddresses)
                        {

                            List.Add("  DNS address .............................. : " + dnsAdress + System.Environment.NewLine);
                        }

                        List.Add(System.Environment.NewLine);


                        
                    }
                    
                }
            }
            return List;
        }
        public async void Worker(object sender, EventArgs e)
        {
            if (Full_List_of_interface)
            {
                var Result = await Task.Run(() => Get_NetworkInterface(Full_List_of_interface));
                OnNewData?.Invoke(this, Result);
                //await Task.Delay(4000);   
            }
            else
            {
                var Result = await Task.Run(() => Get_NetworkInterface(Full_List_of_interface));
                OnNewData?.Invoke(this, Result);
                //await Task.Delay(4000);
            }
        }
    }





    
    
    
}
