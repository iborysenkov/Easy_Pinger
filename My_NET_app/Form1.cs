using System;
using System.Management;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace My_NET_app
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Finds the MAC address of the first operation NIC found.
        /// </summary>
        /// <returns>The MAC address.</returns>
        private string GetMyMacAddress()
        {
            string My_macAddresses = String.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
              My_macAddresses += nic.GetPhysicalAddress().ToString() + System.Environment.NewLine;
                    
            }

            return My_macAddresses;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        public void ShowNetworkInterfaces()
        {

            textBox4.Text = String.Empty;
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            textBox4.Text += "Interface information for     " +   computerProperties.HostName.ToString() + computerProperties.DomainName.ToString() + System.Environment.NewLine;
            if (nics == null || nics.Length < 1)
            {
                textBox4.Text += "  No network interfaces found.";
                return;
            }

            textBox4.Text += "  Number of interfaces .................... : "+ nics.Length.ToString()+ System.Environment.NewLine;
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                //Console.WriteLine();
                textBox4.Text += adapter.Description.ToString()+System.Environment.NewLine;
                textBox4.Text += String.Empty.PadLeft(adapter.Description.Length, '=').ToString() + System.Environment.NewLine;
                //textBox4.Text += "  Interface type .......................... : " + adapter.NetworkInterfaceType.ToString() + System.Environment.NewLine;
                textBox4.Text += "  Physical Address ........................ : " +
                           adapter.GetPhysicalAddress().ToString() + System.Environment.NewLine;
                textBox4.Text += "  Operational status ...................... : " +
                    adapter.OperationalStatus.ToString() + System.Environment.NewLine;
                string versions = "";
                foreach (IPAddressInformation unicast in properties.UnicastAddresses)
                {
                    textBox4.Text += "  IP address ...................... : " + unicast.Address + System.Environment.NewLine;
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
                //textBox4.Text += "  IP version .............................. : "+ versions + System.Environment.NewLine;
                //ShowIPAddresses(properties);

                // The following information is not useful for loopback adapters.
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                {
                    continue;
                }
                //textBox4.Text += ("  DNS suffix .............................. : {0}",                properties.DnsSuffix);

                string label;
                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
                    //textBox4.Text += ("  MTU...................................... : {0}", ipv4.Mtu);
                    if (ipv4.UsesWins)
                    {

                        IPAddressCollection winsServers = properties.WinsServersAddresses;
                        if (winsServers.Count > 0)
                        {
                            label = "  WINS Servers ............................ :";
                            //ShowIPAddresses(label, winsServers); 
                        }
                    }
                }

                //textBox4.Text += ("  DNS enabled ............................. : {0}",                    properties.IsDnsEnabled);
                //textBox4.Text += ("  Dynamically configured DNS .............. : {0}",                    properties.IsDynamicDnsEnabled);
                //textBox4.Text += ("  Receive Only ............................ : {0}",                    adapter.IsReceiveOnly);
                //textBox4.Text += ("  Multicast ............................... : {0}",                    adapter.SupportsMulticast);
                //ShowInterfaceStatistics(adapter);

                Console.WriteLine();
            }
        }

        public string GetMacAddress(string ipAddress)
        {
            string macAddress = string.Empty;
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a " + ipAddress;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();
            string strOutput = pProcess.StandardOutput.ReadToEnd();
            string[] substrings = strOutput.Split('-');
            if (substrings.Length >= 8)
            {
                macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                         + "-" + substrings[4] + "-" + substrings[5] + "-" + substrings[6]
                         + "-" + substrings[7] + "-"
                         + substrings[8].Substring(0, 2);
                return macAddress;
            }

            else
            {
                return "not found";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
 {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress ip_Checked;
            string IP_address_CHK;
            IP_address_CHK = textBox2.Text;
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                                                 //Console.WriteLine(hostName);
                                                 // Get the IP 
           
            
            
            bool ValidateIP = IPAddress.TryParse(IP_address_CHK, out ip_Checked);
            if (ValidateIP)
            {
                try
                {
                    Ping myPing = new Ping();
                    PingReply reply = myPing.Send(ip_Checked, 1000);
                    if (reply != null)
                    {
                        textBox1.Text += "Ping " + ip_Checked.ToString() + System.Environment.NewLine;
                        textBox1.Text += DateTime.Now.ToString() + " " + reply.Status + " Time : " + reply.RoundtripTime.ToString() + " Address : " + reply.Address + Environment.NewLine;
                        //Console.WriteLine(reply.ToString());

                        string Test_on_Success = reply.Status.ToString();

                        bool result = Test_on_Success.Equals("Success");
                        if (result)
                        {
                            string ipAddress = ip_Checked.ToString();
                            string macAddress = string.Empty;
                            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
                            pProcess.StartInfo.FileName = "arp";
                            pProcess.StartInfo.Arguments = "-a " + ipAddress;
                            pProcess.StartInfo.UseShellExecute = false;
                            pProcess.StartInfo.RedirectStandardOutput = true;
                            pProcess.StartInfo.CreateNoWindow = true;
                            pProcess.Start();
                            string strOutput = pProcess.StandardOutput.ReadToEnd();
                            string[] substrings = strOutput.Split('-');
                            if (substrings.Length >= 8)
                            {
                                macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                                         + "-" + substrings[4] + "-" + substrings[5] + "-" + substrings[6]
                                         + "-" + substrings[7] + "-"
                                         + substrings[8].Substring(0, 2);
                                textBox1.Text += "MAC address: " + macAddress + Environment.NewLine;
                            }

                            else
                            {
                                textBox1.Text += "not found"+ Environment.NewLine;
                            }
                        }

                    }

                    

                    textBox3.Text = String.Empty;

                    IPHostEntry host = Dns.GetHostEntry(hostName);

                    textBox3.Text += $"GetHostEntry({hostName}) returns:";

                    foreach (IPAddress address in host.AddressList)
                    {
                        textBox3.Text += $"    {address}" + System.Environment.NewLine;
                    }

                    //textBox3.Text += GetMyMacAddress();

                    ShowNetworkInterfaces();

                }
                catch
                {
                    Console.WriteLine("ERROR: You have Some TIMEOUT issue");
                }

            }
            else
                MessageBox.Show("This is not a valide ip address, enter in format XXX.XXX.XXX.XXX");

            //Console.WriteLine("My IP Address is :" + myIP);

            


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }



        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = String.Empty;
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/icoldiron/Easy_Pinger");
        }
    }
}
