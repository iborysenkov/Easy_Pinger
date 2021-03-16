using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Threading.Tasks;

namespace My_NET_app
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Get_Host_IP_address (1)

        public void Get_Host_IP_Address()
        {
            string hostName = Dns.GetHostName();

            textBox3.Text = String.Empty;

            IPHostEntry host = Dns.GetHostEntry(hostName);

            textBox3.Text += $"GetHostEntry({hostName}) returns:";

            foreach (IPAddress address in host.AddressList)
            {
                textBox3.Text += " #" + $"{address}" + "#" + "  ";
            }
        }

        #endregion

        #region ShowNetworkInterfaces (2)

        public void ShowNetworkInterfaces()
        {

            textBox4.Text = String.Empty;
            
            var List = new List<string>();
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            List.Add("Interface information for     " + computerProperties.HostName.ToString() + computerProperties.DomainName.ToString() + System.Environment.NewLine);
            if (nics == null || nics.Length < 1)
            {
                List.Add("  No network interfaces found.");
                return;
            }

            List.Add("  Number of interfaces .................... : " + nics.Length.ToString() + System.Environment.NewLine + System.Environment.NewLine);
            if (checkBox1.Checked)
            {
                foreach (NetworkInterface adapter in nics)
                {

                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    //Console.WriteLine();
                    List.Add(adapter.Description.ToString() + System.Environment.NewLine);
                    List.Add("=================================================" + System.Environment.NewLine);
                    //textBox4.Text += String.Empty.PadLeft(adapter.Description.Length, '=').ToString() + System.Environment.NewLine;
                    //textBox4.Text += "  Interface type .......................... : " + adapter.NetworkInterfaceType.ToString() + System.Environment.NewLine;
                    List.Add("  Physical Address ........................ : " +
                               adapter.GetPhysicalAddress().ToString() + System.Environment.NewLine);
                    List.Add("  Operational status ...................... : " +
                        adapter.OperationalStatus.ToString() + System.Environment.NewLine);
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
                    //textBox4.Text += "  IP version .............................. : "+ versions + System.Environment.NewLine;
                    //ShowIPAddresses(properties);

                    // The following information is not useful for loopback adapters.
                    if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    {
                        continue;
                    }
                    //textBox4.Text += "  DNS suffix .............................. : "+ properties.DnsSuffix.ToString() + System.Environment.NewLine + System.Environment.NewLine;

                    IPAddressCollection dnsAddresses = properties.DnsAddresses;

                    foreach (IPAddress dnsAdress in dnsAddresses)
                    {

                        List.Add("  DNS address .............................. : " + dnsAdress + System.Environment.NewLine);
                    }

                    List.Add(System.Environment.NewLine);


                    //string label;
                    if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                    {
                        IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
                        //textBox4.Text += ("  MTU...................................... : {0}", ipv4.Mtu);
                        if (ipv4.UsesWins)
                        {

                            IPAddressCollection winsServers = properties.WinsServersAddresses;
                            if (winsServers.Count > 0)
                            {
                                //label = "  WINS Servers ............................ :";
                                //ShowIPAddresses(label, winsServers);
                            }
                        }
                    }

                    //textBox4.Text += ("  DNS enabled ............................. : {0}",                    properties.IsDnsEnabled);
                    //textBox4.Text += ("  Dynamically configured DNS .............. : {0}",                    properties.IsDynamicDnsEnabled);
                    //textBox4.Text += ("  Receive Only ............................ : {0}",                    adapter.IsReceiveOnly);
                    //textBox4.Text += ("  Multicast ............................... : {0}",                    adapter.SupportsMulticast);
                    //ShowInterfaceStatistics(adapter);

                    //Console.WriteLine();
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
                        //textBox4.Text += String.Empty.PadLeft(adapter.Description.Length, '=').ToString() + System.Environment.NewLine;
                        //textBox4.Text += "  Interface type .......................... : " + adapter.NetworkInterfaceType.ToString() + System.Environment.NewLine;
                        List.Add("  Physical Address ........................ : " +
                                   adapter.GetPhysicalAddress().ToString() + System.Environment.NewLine);
                        List.Add("  Operational status ...................... : " +
                            adapter.OperationalStatus.ToString() + System.Environment.NewLine);
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
                        //textBox4.Text += "  IP version .............................. : "+ versions + System.Environment.NewLine;
                        //ShowIPAddresses(properties);

                        // The following information is not useful for loopback adapters.
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                        {
                            continue;
                        }
                        //textBox4.Text += "  DNS suffix .............................. : "+ properties.DnsSuffix.ToString() + System.Environment.NewLine + System.Environment.NewLine;

                        IPAddressCollection dnsAddresses = properties.DnsAddresses;

                        foreach (IPAddress dnsAdress in dnsAddresses)
                        {

                            List.Add("  DNS address .............................. : " + dnsAdress + System.Environment.NewLine);
                        }

                        List.Add(System.Environment.NewLine);


                        //string label;
                        if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                        {
                            IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
                            //textBox4.Text += ("  MTU...................................... : {0}", ipv4.Mtu);
                            if (ipv4.UsesWins)
                            {

                                IPAddressCollection winsServers = properties.WinsServersAddresses;
                                if (winsServers.Count > 0)
                                {
                                    //label = "  WINS Servers ............................ :";
                                    //ShowIPAddresses(label, winsServers);
                                }
                            }
                        }

                    }

                }
            }
        
            foreach (var item in List)
            {
                textBox4.Text += item;
            }
        }

        #endregion

        #region Show_Cumputer_Users (3)
        public static List<string> GetComputerUsers()
        {
            List<string> users = new List<string>();
            var path =
                string.Format("WinNT://{0},computer", Environment.MachineName);

            using (var computerEntry = new DirectoryEntry(path))
                foreach (DirectoryEntry childEntry in computerEntry.Children)
                    if (childEntry.SchemaClassName == "User")
                        users.Add(childEntry.Name);

            return users;
        }


        public void Show_Computer_Users()
        {
            textBox5.Text = null;
            List<string> Usernames = GetComputerUsers();
            foreach (string s in Usernames)
                textBox5.Text += "#" + s + "#" + "  ";
        }
        #endregion

        #region Ping_My_Address Ping_My_Address(string Ip_For_Check)

        public void Ping_My_Address(string Ip_For_Check)
            {
               
                    Ping myPing = new Ping();
                    PingReply reply = myPing.Send(Ip_For_Check, 1000);
                    bool result;
            if (reply != null)
            {
                textBox1.Text += "Ping " + Ip_For_Check.ToString() + System.Environment.NewLine;
                textBox1.Text += DateTime.Now.ToString() + "  //  Ping " + reply.Status + "  // Time : " + reply.RoundtripTime.ToString()+" //" + " Address : " + reply.Address + Environment.NewLine;
                //Console.WriteLine(reply.ToString());
                
                string Test_on_Success = reply.Status.ToString();
                result = Test_on_Success.Equals("Success");
            }
            else
            {
                textBox1.Text += "NULL Result" + Environment.NewLine + Environment.NewLine;
            }


        }
        #endregion

        #region Get_Ping_MAC

        public void Get_Pinged_MAC(string ipAddress)
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
                textBox1.Text += "MAC address: " + macAddress + Environment.NewLine + Environment.NewLine;
            }
            else
            {
                textBox1.Text += "not found" + Environment.NewLine + Environment.NewLine;
            }
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            Get_Host_IP_Address();
            ShowNetworkInterfaces();
            Show_Computer_Users();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Get_Host_IP_Address();
            ShowNetworkInterfaces();

            IPAddress ip_Checked; 
            string IP_address_for_check;
            IP_address_for_check = textBox2.Text;
            bool ValidateIP = IPAddress.TryParse(IP_address_for_check, out ip_Checked);

            if (ValidateIP)
            {
                try
                {
                    Ping_My_Address(ip_Checked.ToString());
                    Get_Pinged_MAC(ip_Checked.ToString());
                }
                catch
                {
                    MessageBox.Show("Fault of pinging IP address");
                }

            } 
            else MessageBox.Show("This is not a valide ip address, enter in format XXX.XXX.XXX.XXX");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = String.Empty;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/icoldiron/Easy_Pinger");
        }

        private void button3_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form2 f2 = new Form2();
            f2.ShowDialog();
            this.Close();

            //this.Hide();
            //var form2 = new Form2();
            //form2.Closed += (s, args) => this.Close();
            //form2.Show();
        }
    }
}
