using System;

namespace ButtonStats.Server.Model
{
    public interface IInputDataListener
    {
        public event EventHandler<DataRecievedEventArgs> DataRecieved;
        public void OpenTcpListener();
    }
}
