using ButtonsStats.Client.ViewModel;
using ReactiveUI;
using Splat;
using System;
using System.Reflection;
using System.Windows;

namespace ButtonsStats.Client
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            new AppBootstrapper();

            MainWindow = new MainWindow();
            MainWindow.DataContext = new MainViewModel();

            base.OnStartup(e);
        }
    }
}
