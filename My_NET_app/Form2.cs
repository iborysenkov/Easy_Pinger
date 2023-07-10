using System;
using System.Linq;
using System.Windows.Forms;
using Modbus;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_NET_app
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        #region Test_Modbus_TCPMaster

        public void Test_Modbus_TCPMuster(string Ip_Address, int Port)
        {
            string Address_Unit_ID = numericAddress.Value.ToString();
            byte unit_id = Convert.ToByte(Address_Unit_ID);

            try
            {
                ModbusMasterTCP TCPmm = new ModbusMasterTCP(Ip_Address, Port);
                TCPmm.Connect();
                try
                {
                    ushort[] arc = TCPmm.ReadHoldingRegisters(unit_id, 3059, 2);
                    string data_from_register = null;


                    for (ushort i=0; i<arc.Length; i++) {

                        data_from_register += arc[i].ToString("X4");
                        
                    }
                    uint num = uint.Parse(data_from_register, System.Globalization.NumberStyles.AllowHexSpecifier);

                    byte[] floatVals = BitConverter.GetBytes(num);
                    float f = BitConverter.ToSingle(floatVals, 0);
                    textBox3.Text += "IP: "+ Ip_Address + " Address: " + unit_id+  System.Environment.NewLine + "Consumed power in kWts = "+ f + System.Environment.NewLine;
                    textBox3.Text += "HEX Format:   " + data_from_register + System.Environment.NewLine + System.Environment.NewLine;

                    //byte[] bytes = BitConverter.GetBytes(data_from_register);
                    //if (BitConverter.IsLittleEndian)
                    //{
                    //    bytes = bytes.Reverse().ToArray();
                    //}
                    //float myFloat = BitConverter.ToSingle(bytes, 0);

                    //textBox3.Text += arc + System.Environment.NewLine;
                }
                catch
                {
                    textBox3.Text += "Error 1: Can not Poll this register range" + System.Environment.NewLine;
                }
            }
            catch
            {
                textBox3.Text += "Error 2: Can not find slave" + System.Environment.NewLine;
            }


        }
        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.ShowDialog();
            this.Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            IPAddress ip_Checked;
            string IP_address_CHK;
            IP_address_CHK = textBox2.Text;
            bool ValidateIP = IPAddress.TryParse(IP_address_CHK, out ip_Checked);
                        
            string Port_number_string;
            Port_number_string = textBox1.Text;

            try
            {
                int Port_number = Int32.Parse(Port_number_string);
                if (Enumerable.Range(1, 65535).Contains(Port_number) & ValidateIP)
                {
                    await Task.Run(() => Test_Modbus_TCPMuster(ip_Checked.ToString(), Port_number));
                }
                else
                {
                    MessageBox.Show("This is not a valide ip address, enter in format XXX.XXX.XXX.XXX or Port not in range 1-65535");
                }
            }
            catch
            {
                MessageBox.Show("You entered litteres into address and port textbox, please corect to digits and try once again");
            }

            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = String.Empty;
        }

    }
}
