using ButtonStats.Client.Api;
using ButtonStats.Client.Services;
using ButtonStats.Shared.Model;
using ReactiveUI;
using Splat;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace ButtonStats.Client.ViewModel
{
    public class MainViewModel : ReactiveObject
    {
        private IConnectionService _connectionService;
        private IApi _api;

        private InputData _lastInput;
        private string _text;
        private string _socketAddress;
        private bool _isConnected;

        public InputData LastInput
        {
            get { return _lastInput; }
            set { this.RaiseAndSetIfChanged(ref _lastInput, value); }
        }

        public string Text
        {
            get { return _text; }
            set { this.RaiseAndSetIfChanged(ref _text, value); }
        }

        public string SocketAddress
        {
            get { return _socketAddress; }
            set { this.RaiseAndSetIfChanged(ref _socketAddress, value); }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set { this.RaiseAndSetIfChanged(ref _isConnected, value); }
        }

        public ReactiveCommand<Unit, bool> ConnectCommand { get; }

        public MainViewModel()
        {
            _connectionService = Locator.Current.GetService<IConnectionService>();
            _api = Locator.Current.GetService<IApi>();

            ConnectCommand = ReactiveCommand.CreateFromTask(
                () => _connectionService.ConnectAsync(_socketAddress));

            IDisposable textAdd = this.WhenAnyValue(vm => vm.Text)
                .Do(_ => IsConnected = _connectionService.IsConnected)
                .SkipWhile(_ => IsConnected == false)
                .Buffer(2, 1)
                .Select(b => (Previous: b[0], Current: b[1]))
                .Where(b => b.Current.Length > b.Previous.Length)
                .Subscribe(b => UpdateLastInput(b.Current));

            IDisposable lastInputUpdate = this.WhenAnyValue(vm => vm.LastInput)
                .Subscribe(lastInput => _api.SendInputData(lastInput));
        }

        private void UpdateLastInput(string text)
        {
            this.Log().Debug("Input data updated.");
            if (!string.IsNullOrEmpty(text))
            {
                LastInput = new InputData(text.Last(), DateTime.Now);
            }
        }
    }
}