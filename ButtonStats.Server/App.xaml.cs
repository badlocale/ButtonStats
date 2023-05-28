using System.Windows;

namespace ButtonStats.Server
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
