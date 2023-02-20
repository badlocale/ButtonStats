using ButtonsStats.Shared.Model;
using System;

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
