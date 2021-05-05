using System;
using System.Collections.Generic;
using System.Windows;

namespace Microsoft.Research.EyeGazeMouse
{
    class GazeStats
    {
        const int DefaultHistoryLength = 90;
        const double DefaultSaccadeDistance = .10; // Not sure this is the right value, this may be from the days where distance was measured 0...1

        readonly int _historyLength;
        readonly double _saccadeDistance;
        readonly bool _saccadeReset;

        List<Point> _history;
        Point _sum;
        Point _mean;

        public GazeStats(int historyLength = DefaultHistoryLength, double saccadeDistance = DefaultSaccadeDistance, bool saccadeReset = false)
        {
            _historyLength = historyLength;
            _saccadeDistance = saccadeDistance;
            _saccadeReset = saccadeReset;

            _history = new List<Point>();
            _sum = new Point();
            _mean = new Point();
        }

        public void Update(double x, double y)
        {
            Point current = new Point(x, y);

            if (_saccadeReset && _history.Count > 0)
            {
                Point last = _history[_history.Count - 1];

                if (Point.Subtract(current, last).Length > _saccadeDistance)
                {
                    // Saccade detected, reset stats
                    System.Diagnostics.Debug.WriteLine("Saccade!");

                    _history.Clear();
                    _sum = new Point();
                    _mean = new Point();
                }
            }

            if (_history.Count >= _historyLength)
            {
                Point first = _history[0];
                _sum.X -= first.X;
                _sum.Y -= first.Y;
                _history.RemoveAt(0);
            }

            _sum.X += x;
            _sum.Y += y;
            _mean.X = _sum.X / _history.Count;
            _mean.Y = _sum.Y / _history.Count;

            _history.Add(current);
        }

        public Point Mean
        {
            get { return _mean; }
        }

        public Point StandardDeviation
        {
            get
            {
                double x = 0.0, y = 0.0;
                for (int i = 0; i < _history.Count; i++)
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

        public double Variance
        {
            get
            {
                var standardDeviation = StandardDeviation; // Only calculate once
                return (standardDeviation.X + standardDeviation.Y) / 2;
            }
        }

    }
}
