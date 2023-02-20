using ButtonsStats.Server.Model;
using ReactiveUI;
using Splat;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Reactive.Linq;
using DynamicData;
using System.Drawing;
using OxyPlot.Axes;
using ButtonsStats.Shared.Model;
using System.Collections.Generic;
using Serilog;
using System.Linq;

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

            IDisposable disposableInputDataUpdate = _inputs
                .Connect()
                .Subscribe(_ => OnInputDataUpdated());

            IDisposable inputRecievedSubscription =
                Observable.FromEventPattern<DataRecievedEventArgs>(_inputDataListener, "DataRecieved")
                .Subscribe(a => OnInputDataRecieved(a.EventArgs));
        }

        private void OnInputDataRecieved(DataRecievedEventArgs args)
        {
            _inputs.Add(args.InputData);
        }

        private void OnInputDataUpdated()
        {
            UpdateInstantSpeedPlot();
            UpdateAverageSpeedPlot();
            InstantSpeedPlotModel.InvalidatePlot(true);
            AverageSpeedPlotModel.InvalidatePlot(true);
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
        }
    }
}
