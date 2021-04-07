using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_NET_app
{
    class GetMACaddress
    {

       System.Diagnostics.Process pProcess;
       

        public event Action<object, string[], string> OnNewData;

        public GetMACaddress()
        {
            string macAddress = string.Empty;
            pProcess = new System.Diagnostics.Process();
        }

        static string[] MACGetter(System.Diagnostics.Process pProcess, string ipAddress)
        {
            string macAddress = string.Empty;
            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a " + ipAddress;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();
            string strOutput = pProcess.StandardOutput.ReadToEnd();
            string[] substrings = strOutput.Split('-');
            return substrings;
        }


        public async void Worker(string ipAddress)
            {
            
            var Reply = await Task.Run(() => MACGetter(pProcess, ipAddress));

            OnNewData?.Invoke(this, Reply, ipAddress);

        }

    }
}
