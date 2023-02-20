using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButtonsStats.Server.Model
{
    public interface IInputDataListener
    {
        public event EventHandler<DataRecievedEventArgs> DataRecieved;
        public void OpenTcpListener();
    }
}
