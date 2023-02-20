using System.Windows;

namespace ButtonsStats.Server
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            new AppBootstrapper();
            base.OnStartup(e);
        }
    }
}
