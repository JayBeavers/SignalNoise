using EyeXFramework;
using Microsoft.Research.EyeGazeMouse;
using System;
using System.Windows;
using System.Windows.Threading;

namespace SignalNoise
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private EyeXHost _eyexHost;
        private GazeStats _gazeStats;
        private DispatcherTimer _timer;

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _gazeStats = new GazeStats(90);

            _eyexHost = new EyeXHost();
            var eyePositionDataStream = _eyexHost.CreateEyePositionDataStream();
            eyePositionDataStream.Next += eyePositionDataStream_Next;
            _eyexHost.Start();

            _timer = new DispatcherTimer(TimeSpan.FromSeconds(3), DispatcherPriority.Normal, UpdateStats, Dispatcher);
        }
 
        void eyePositionDataStream_Next(object sender, EyePositionEventArgs e)
        {
            _gazeStats.Update(e.LeftEye.X, e.LeftEye.Y);
        }

        void UpdateStats(object o, EventArgs ea)
        {
            Variance.Text = ((_gazeStats.StandardDeviation().X + _gazeStats.StandardDeviation().Y) / 2).ToString("F1");
        }
    }
}
