using ButtonStats.Shared.Model;
using ReactiveUI;
using Splat;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace ButtonStats.Server.Model
{
    public class InputDataListener : IInputDataListener, IEnableLogger
    {
        public event EventHandler<DataRecievedEventArgs>? DataRecieved;

        public async void OpenTcpListener()
        {
            IPAddress localAddr = IPAddress.Loopback;
            TcpListener tcpListener = new TcpListener(localAddr, 8080);
            try
            {
                new Thread(async _ =>
                {
                    tcpListener.Start();
                    this.Log().Info($"Listener started on address: {tcpListener.LocalEndpoint}. Wait for connection.");

                    while (true)
                    {
                        TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                        this.Log().Info($"Connected client with IP: {tcpClient.Client.LocalEndPoint}.");

                        NetworkStream stream = tcpClient.GetStream();
                        BinaryFormatter bf = new();
                        while (true)
                        {
                            ReadInputDataFromStream(stream, bf);
                        }
                    }
                }).Start();
            }
            catch (Exception e)
            {
                tcpListener.Stop();
                this.Log().Info(e, $"Server stoped.");
            }
        }

        private void ReadInputDataFromStream(Stream stream, IFormatter formatter)
        {
            try
            {
                InputData inputData = (InputData)formatter.Deserialize(stream);
                OnDataRecieved(new DataRecievedEventArgs(inputData));
                this.Log().Info($"Data recieved: {inputData}.");
            }
            catch
            {
                throw;
            }
        }

        private void OnDataRecieved(DataRecievedEventArgs args)
        {
            DataRecieved?.Invoke(this, args);
        }
    }
}
