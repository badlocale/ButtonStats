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
            base.OnStartup(e);
        }
    }
}
