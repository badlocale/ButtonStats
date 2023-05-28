using ButtonStats.Server.ViewModel;
using ReactiveUI;
using System.Windows;

namespace ButtonStats.Server
{
    public partial class MainWindow : Window, IViewFor<MainViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(MainViewModel), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();

            this.WhenActivated(disposable =>
            {
                this.Bind(ViewModel, vm => vm.AverageSpeedPlotModel, v => v.AverageSpeedPlot.Model);
                this.Bind(ViewModel, vm => vm.InstantSpeedPlotModel, v => v.InstantSpeedPlot.Model);
            });
        }

        public MainViewModel? ViewModel
        {
            get => (MainViewModel?)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object? IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainViewModel)value;
        }
    }
}
