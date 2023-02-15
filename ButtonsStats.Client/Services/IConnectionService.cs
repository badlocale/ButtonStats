using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ButtonsStats.Client.Services
{
    public interface IConnectionService
    {
        public TcpClient TcpClient { get; }
        public bool IsConnected { get; }
        public Task<bool> ConnectAsync(string? socketAddress);
    }
}
