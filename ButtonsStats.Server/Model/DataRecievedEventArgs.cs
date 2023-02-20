using ButtonsStats.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButtonsStats.Server.Model
{
    public class DataRecievedEventArgs : EventArgs
    {
        public InputData InputData { get; set; }

        public DataRecievedEventArgs(InputData inputData) : base()
        {
            InputData = inputData;
        }
    }
}
