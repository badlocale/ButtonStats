using System.Windows;

namespace ButtonStats.Client
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
