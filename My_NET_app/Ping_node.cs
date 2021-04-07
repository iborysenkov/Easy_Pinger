using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Threading;
using System.Text;

namespace My_NET_app
{
    class Ping_node
    {

        public static Ping pingSender { get; set; }
        public static PingReply Reply { get; set; }
        static int timeout;
        bool isActive;
        static PingOptions options;
        static byte[] buffer;
        static AutoResetEvent waiter = new AutoResetEvent(false);
        public IPAddress _IPaddress { get; set; }
        public int Qty_of_tries { get; set; }

        public event Action<object, PingReply> OnNewData;
        public event Action<object, CancellationToken> OnCompleted;

        public Ping_node()
        {
            pingSender = new Ping();
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            buffer = Encoding.ASCII.GetBytes(data);
            timeout = 10000;
            options = new PingOptions(64, false);
            waiter = new AutoResetEvent(false);
        }

        public Ping_node(IPAddress _IPaddress, int Qty_of_tries)
        {
            this._IPaddress = _IPaddress;
            this.Qty_of_tries = Qty_of_tries;
        }

        public void Start()
        {
            isActive = true;
            Worker();
        }

        public void Stop()
        {
            isActive = false;
        }

        static PingReply _pingerr(IPAddress _IPaddress)
        {
            try
            {
                //Reply = pingSender.SendAsync(_IPaddress, timeout, buffer, options, waiter);
                Reply = pingSender.Send(_IPaddress, timeout, buffer, options);
                return Reply;
            }
            catch
            {
                
                return null;
            }
        }

        private async void Worker()
        {
            
            
            for (int i = 1; i <= Qty_of_tries; i++)
            {
                /*
                if (!isActive)
                {
                    //OnCompleted?.Invoke(this);
                    return;
                }
                */
                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken token = cancelTokenSource.Token;
                OnCompleted?.Invoke(this, token);
                var Reply = await Task.Run(()=>_pingerr(_IPaddress));
                
                await Task.Delay(1000);
                cancelTokenSource.Cancel();
                OnNewData?.Invoke(this, Reply);

                cancelTokenSource = null;
                

            }

        }
    }
}
