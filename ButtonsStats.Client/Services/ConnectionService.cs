using DynamicData.Kernel;
using Splat;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ButtonsStats.Client.Services
{
    public class ConnectionService : IConnectionService, IEnableLogger
    {
        private TcpClient _tpcClient;

        public TcpClient TcpClient
        {
            get { return _tpcClient; }
        }
        public bool IsConnected => _tpcClient == null ? false : _tpcClient.Connected;

        public async Task<bool> ConnectAsync(string? socketAddress)
        {
            if (string.IsNullOrEmpty(socketAddress))
            {
                this.Log().Error("Socket address is empty.");
                return false;
            }

            string[] tokenizedAddress = socketAddress.Split(':');
            string hostname;
            int port;
            try
            {
                hostname = tokenizedAddress[0];
                port = Convert.ToInt32(tokenizedAddress[1]);

                _tpcClient = new TcpClient();

                await _tpcClient.ConnectAsync(hostname, port);
                this.Log().Info($"Success connection to {_tpcClient.Client.RemoteEndPoint}.");
                return true;
            }
            catch (Exception e)
            {
                this.Log().Error(e, "Host not availible or an incorrect address format.");
                return false;
            }
        }
    }
}
