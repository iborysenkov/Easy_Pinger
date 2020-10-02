using System;
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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = String.Empty;
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
