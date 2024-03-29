﻿using ButtonStats.Client.Services;
using ButtonStats.Shared.Model;
using Splat;
using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace ButtonStats.Client.Api
{
    public class SocketApi : IApi, IEnableLogger
    {
        private IConnectionService _connectionService;

        public SocketApi()
        {
            _connectionService = Locator.Current.GetService<IConnectionService>();
        }

        public bool SendInputData(InputData inputData)
        {
            if (_connectionService.TcpClient != null &&
                _connectionService.TcpClient.Connected)
            {
                NetworkStream stream = _connectionService.TcpClient.GetStream();
                BinaryFormatter bf = new();

                try
                {
                    bf.Serialize(stream, inputData);
                    return true;
                }
                catch (Exception e)
                {
                    this.Log().Error(e);
                    return false;
                }
            }
            else
            {
                this.Log().Error("Tcp client not connected. No input data has been sent.");
                return false;
            }
        }
    }
}
