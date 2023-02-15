using ButtonsStats.Client.Api;
using ButtonsStats.Client.Services;
using ReactiveUI;
using Serilog;
using Splat;
using Splat.Serilog;

namespace ButtonsStats.Client
{
    public class AppBootstrapper
    {
        public AppBootstrapper()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            Locator.CurrentMutable.RegisterLazySingleton<IApi>(() => new SocketApi());
            Locator.CurrentMutable.RegisterLazySingleton<IConnectionService>(() => new ConnectionService());
            Locator.CurrentMutable.UseSerilogFullLogger();
        }
    }
}
