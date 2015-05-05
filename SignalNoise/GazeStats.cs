using System;
using System.Collections.Generic;
using System.Windows;

namespace Microsoft.Research.EyeGazeMouse
{
    class GazeStats
    {
        int _historyLength;
        List<Point> _history;
        Point _sum;
        Point _mean;

        public GazeStats(int historyLength)
        {
            _historyLength = historyLength;
            _history = new List<Point>();
            _sum = new Point();
            _mean = new Point();
        }

        public void Update(double x, double y)
        {
            if (_history.Count >= _historyLength)
            {
                Point first = _history[0];
                _sum.X -= first.X;
                _sum.Y -= first.Y;
                _history.RemoveAt(0);
            }

            Point newPoint = new Point(x, y);
            _sum.X += x;
            _sum.Y += y;
            _mean.X = _sum.X / _history.Count;
            _mean.Y = _sum.Y / _history.Count;

            _history.Add(newPoint);
        }

        public Point Mean
        {
            get { return _mean; }
        }

        public Point StandardDeviation()
        {
            double x = 0.0, y = 0.0;
            for (int i = 0; i <_history.Count; i++)
            {
                x += (_history[i].X - _mean.X) * (_history[i].X - _mean.X);
                y += (_history[i].Y - _mean.Y) * (_history[i].Y - _mean.Y);
            }
            // TODO: Should we use population mean or sample mean to compute variance?
            x /= _history.Count;
            y /= _history.Count;

            return new Point(Math.Sqrt(x), Math.Sqrt(y));
        }
    }
}
