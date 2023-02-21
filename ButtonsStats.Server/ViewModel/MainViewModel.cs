using ButtonsStats.Server.Model;
using ButtonsStats.Shared.Model;
using DynamicData;
using OxyPlot;
using OxyPlot.Series;
using ReactiveUI;
using Splat;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace ButtonsStats.Server.ViewModel
{
    public class MainViewModel : ReactiveObject, IEnableLogger
    {
        private IInputDataListener _inputDataListener;

        private SourceList<InputData> _inputs = new();
        private int _inputCount = 0;
        private LineSeries _instantSpeedLine;
        private LineSeries _averageSpeedLine;

        public PlotModel InstantSpeedPlotModel { get; }
        public PlotModel AverageSpeedPlotModel { get; }

        public MainViewModel()
        {
            _inputDataListener = Locator.Current.GetService<IInputDataListener>();

            InstantSpeedPlotModel = new PlotModel { Title = "Instant speed (symbols/sec)." };
            _instantSpeedLine = new LineSeries()
            {
                Color = OxyColors.Blue,
                MarkerType = MarkerType.Circle,
                StrokeThickness = 2
            };
            InstantSpeedPlotModel.Series.Add(_instantSpeedLine);

            AverageSpeedPlotModel = new PlotModel { Title = "Average speed (symbols/sec)." };
            _averageSpeedLine = new LineSeries()
            {
                Color = OxyColors.Red,
                MarkerType = MarkerType.Circle,
                StrokeThickness = 2,
            };
            AverageSpeedPlotModel.Series.Add(_averageSpeedLine);

            _inputDataListener.OpenTcpListener();

            IDisposable inputDataUpdate = _inputs
                .Connect()
                .Do(_ => UpdateInstantSpeedPlot())
                .Subscribe(_ => UpdateAverageSpeedPlot());

            IDisposable inputRecievedSubscription =
                Observable.FromEventPattern<DataRecievedEventArgs>(_inputDataListener, "DataRecieved")
                .Subscribe(a => _inputs.Add(a.EventArgs.InputData));
        }

        //Instant speed = 1 / interval between two last inputs
        private void UpdateInstantSpeedPlot()
        {
            double instantSpeed;
            if (_inputs.Count > 1)
            {
                TimeSpan dt = _inputs.Items.Last().InputTime - 
                    _inputs.Items.ElementAt(_inputs.Items.Count() - 2).InputTime;
                instantSpeed = (1 / dt.TotalMilliseconds) * 1000;
            }
            else
            {
                instantSpeed = 0;
            }

            if (_averageSpeedLine.Points.Count > 50)
            {
                _averageSpeedLine.Points.RemoveAt(0);
            }

            DataPoint newInstantSpeedPoint = new DataPoint(_inputCount++, instantSpeed);
            _instantSpeedLine.Points.Add(newInstantSpeedPoint);

            InstantSpeedPlotModel.InvalidatePlot(true);
        }

        //Average speed = inputs count / entire time
        private void UpdateAverageSpeedPlot()
        {
            double averageSpeed;
            if (_inputs.Count > 1)
            {
                TimeSpan entireTime = _inputs.Items.Last().InputTime - 
                    _inputs.Items.First().InputTime;
                averageSpeed = (_inputs.Count / entireTime.TotalMilliseconds) * 1000;
            }
            else
            {
                averageSpeed = 0;
            }

            if (_instantSpeedLine.Points.Count > 50)
            {
                _instantSpeedLine.Points.RemoveAt(0);
            }

            DataPoint newAverageSpeedPoint = new DataPoint(_inputCount++, averageSpeed);
            _averageSpeedLine.Points.Add(newAverageSpeedPoint);

            AverageSpeedPlotModel.InvalidatePlot(true);
        }
    }
}
