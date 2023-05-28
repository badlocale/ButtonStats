using ButtonStats.Server.Model;
using Serilog;
using Splat;
using Splat.Serilog;

namespace ButtonStats.Server
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
