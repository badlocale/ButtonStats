using ButtonsStats.Client.ViewModel;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace ButtonsStats.Client
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
                this.Bind(ViewModel, vm => vm.Text, v => v.TextField.Text)
                    .DisposeWith(disposable);
                this.Bind(ViewModel, vm => vm.SocketAddress, v => v.AddressField.Text)
                    .DisposeWith(disposable);
                this.BindCommand(ViewModel, vm => vm.ConnectCommand, v => v.ConnectButton)
                    .DisposeWith(disposable);
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
