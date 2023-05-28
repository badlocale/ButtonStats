using System.Net.Sockets;
using System.Threading.Tasks;

namespace ButtonStats.Client.Services
{
    public interface IConnectionService
    {
        public TcpClient TcpClient { get; }
        public bool IsConnected { get; }
        public Task<bool> ConnectAsync(string? socketAddress);
    }
}
