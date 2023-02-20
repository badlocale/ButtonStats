﻿using System;
using System.Net.Sockets;
using System.Net;
using Splat;
using ReactiveUI;
using System.Runtime.Serialization.Formatters.Binary;
using ButtonsStats.Shared.Model;
using System.Windows.Documents;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace ButtonsStats.Server.Model
{
    public class InputDataListener : IInputDataListener, IEnableLogger
    {
        public event EventHandler<DataRecievedEventArgs> DataRecieved;

        public async void OpenTcpListener()
        {
            IPAddress localAddr = IPAddress.Loopback;
            TcpListener tcpListener = new TcpListener(localAddr, 8080);
            try
            {
                tcpListener.Start();
                this.Log().Info($"Listener started on address: {tcpListener.LocalEndpoint}. Wait for connection.");

                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                this.Log().Info($"Connected client with IP: {tcpClient.Client.LocalEndPoint}.");

                NetworkStream stream = tcpClient.GetStream();
                BinaryFormatter bf = new();
                new Thread(_ =>
                {
                    while (true)
                    {
                        ReadInputDataFromStream(stream, bf);
                    }
                }).Start();
            }
            catch (Exception e)
            {
                tcpListener.Stop();
                this.Log().Info($"Server stoped.", e);
            }
        }

        private void ReadInputDataFromStream(Stream stream, IFormatter formatter)
        {
            InputData inputData = (InputData)formatter.Deserialize(stream);
            OnDataRecieved(new DataRecievedEventArgs(inputData));
            this.Log().Info($"Data recieved: {inputData.ToString()}.");
        }

        private void OnDataRecieved(DataRecievedEventArgs args)
        {
            DataRecieved?.Invoke(this, args);
        }
    }
}
