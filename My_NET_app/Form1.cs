using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.DirectoryServices;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;

namespace My_NET_app
{
    public partial class Form1 : Form
    {
        Ping_node ping_node;
        Network_Interfaces Netwrok_Interface;
        GetMACaddress GetMACaddress;
        public event EventHandler Updateinterface;
        private BindingList<string> PingListBox;
        static string Waiter = "Wait";

        //static bool starter = true;

        public Form1()
        {
            InitializeComponent();
            ping_node = new Ping_node();
            PingListBox = new BindingList<string>();
            Netwrok_Interface = new Network_Interfaces();
            GetMACaddress = new GetMACaddress();

            GetMACaddress.OnNewData += Get_Pinged_MAC;
            listBox1.DataSource = PingListBox;
            ping_node.OnNewData += DisplayReply;
            Netwrok_Interface.OnNewData += DisplayResult;
            Updateinterface += Netwrok_Interface.Worker;
            ping_node.OnCompleted += DisplayWait;
            
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
        
        public void DisplayResult(object sender, List<string> e)
        {
            textBox4.Text = String.Empty;
            foreach (var item in e)
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

        #region Display Ping Reply 

        private void DisplayReply(object sender, PingReply e)
        {
            
            PingReply reply = e;
            if (reply == null)
                return;
            IPAddress IPaddress = (IPAddress)reply.Address;
            PingListBox.Remove(Waiter);
            if (reply.Status == IPStatus.Success)
            {
                
                PingListBox.Add(" Reply From Address: " + reply.Address.ToString()
                    + " bytes=" + reply.Buffer.Length
                    + " time:" + reply.RoundtripTime.ToString()
                    + " TTL:" + reply.Options.Ttl.ToString());
            }
            else
            {
               PingListBox.Add("No reply from Pinged IP" + Environment.NewLine);

            }
            Waiter = "Wait";
            return;
        }

        private async void DisplayWait(object sender, CancellationToken token)
        {   
            await Task.Run(() => Mysmth(token));
        }

        void Mysmth(CancellationToken token)
        {
            Thread.Sleep(500);
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }
                PingListBox.Remove(Waiter);
                Waiter += ">";
                PingListBox.Add(Waiter);
                Thread.Sleep(200);

            }
            return;
        }

        #endregion

        #region Get_Ping_MAC

        public void Get_Pinged_MAC(object sender, string[] e, string ipAddress)
        {

            if (e.Length >= 8)
            {
                string macAddress = e[3].Substring(Math.Max(0, e[3].Length - 2))
                         + "-" + e[4] + "-" + e[5] + "-" + e[6]
                         + "-" + e[7] + "-"
                         + e[8].Substring(0, 2);
                PingListBox.Add(Environment.NewLine);
                PingListBox.Add(DateTime.Now.ToString() + " IP: " + ipAddress + " MAC address: " + macAddress + Environment.NewLine);
            }
            else
            {
                PingListBox.Add(Environment.NewLine);
                PingListBox.Add(Environment.NewLine + DateTime.Now.ToString() + " MAC of IP: " + ipAddress + " NOT FOUND" + Environment.NewLine);
            }
            PingListBox.Add("==============================" + Environment.NewLine);

        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            Get_Host_IP_Address();
            Show_Computer_Users();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Get_Host_IP_Address();
            IPAddress ip_Checked; 
            string IP_address_for_check;
            IP_address_for_check = textBox2.Text;
            bool ValidateIP = IPAddress.TryParse(IP_address_for_check, out ip_Checked);
            Updateinterface?.Invoke(this, EventArgs.Empty);

            if (ValidateIP)
            {
                try
                {
                    GetMACaddress.Worker(ip_Checked.ToString());
                    ping_node.Qty_of_tries = (int)numericPing.Value;
                    ping_node._IPaddress = ip_Checked;
                    ping_node.Start();
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
            //textBox1.Text = String.Empty;
            PingListBox.Clear();

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/iborysenkov/Easy_Pinger");
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Netwrok_Interface.Full_List_of_interface = checkBox1.Checked;
            Updateinterface?.Invoke(this, EventArgs.Empty);
        }


    }
}
