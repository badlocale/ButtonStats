using ButtonsStats.Server.Model;
using Serilog;
using Splat;
using Splat.Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButtonsStats.Server
{
    public class AppBootstrapper
    {
        public AppBootstrapper()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            Locator.CurrentMutable.RegisterLazySingleton<IInputDataListener>(() => new InputDataListener());
            Locator.CurrentMutable.UseSerilogFullLogger();
        }
    }
}
