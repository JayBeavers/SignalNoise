using Microsoft.Research.EyeGazeMouse;
using System;
using System.Windows;
using System.Windows.Threading;
using ToltTech.GazeInput;

namespace SignalNoise
{
    public partial class MainWindow
    {
        private GazeStats _gazeStats;
        private DispatcherTimer _timer;
        private IGazeDevice _gazeDevice;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _gazeStats = new GazeStats(90);

            _gazeDevice = GazeDeviceDetector.FindGazeDevice();
            _gazeDevice.GazePoint += GazeDevice_GazePoint;
            _gazeDevice.Error += GazeDevice_Error;

            _timer = new DispatcherTimer(TimeSpan.FromSeconds(3), DispatcherPriority.Normal, UpdateStats, Dispatcher);
        }

        private void GazeDevice_Error(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void GazeDevice_GazePoint(object sender, GazeEventArgs e)
        {
            _gazeStats.Update(e.Screen.X, e.Screen.Y);
        }

        void UpdateStats(object o, EventArgs ea)
        {
            Variance.Text = _gazeStats.Variance.ToString("F1");
        }
    }
}
