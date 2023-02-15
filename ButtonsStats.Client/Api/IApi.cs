using ButtonsStats.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButtonsStats.Client.Api
{
    public interface IApi
    {
        public bool SendInputData(InputData inputData);
    }
}
