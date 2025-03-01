using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using Timer = System.Timers.Timer;

namespace UiDesktopApp2.Helpers
{
    public class UITimer
    {
        #region Private memebers
        private readonly Timer _timer;
        private readonly Stopwatch _stopwatch;
        private readonly Dispatcher _dispatcher;
        private double _initialSecondsToCountDown = 0;
        private double _secondsToCountDown;
        #endregion

        #region Public properties
        public Action<double> UpdateUI { get; set; }
        public Action<double> CompletedAction { get; set; }
        public int IntervalInSeconds { get; set; }
        public double SecondsToCountDown
        {
            get { return _secondsToCountDown; }
            set 
            {
                if (_initialSecondsToCountDown == 0)
                {
                    _initialSecondsToCountDown = value;
                }
                _secondsToCountDown = value;
            }
        }
        #endregion

        #region Constructors
        public UITimer(int intervalInSeconds)
        {
            IntervalInSeconds = intervalInSeconds;
            _dispatcher = Dispatcher.CurrentDispatcher;
            _timer = new Timer()
            {
                Interval = TimeSpan.FromSeconds(IntervalInSeconds).TotalMilliseconds
            };
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
            _stopwatch = new Stopwatch();
        }
        #endregion

        #region Public methods
        public void Start()
        {
            if (UpdateUI == null)
            {
                throw new MissingMemberException(nameof(UpdateUI));
            }

            SecondsToCountDown = _initialSecondsToCountDown;
            UpdateUI(SecondsToCountDown);
            _stopwatch.Restart();
            _timer.Start();
        }
        public void Pause()
        {
            _stopwatch.Stop();
            _timer.Stop();
        }
        public void Resume()
        {
            _stopwatch.Start();
            _timer.Start();
        }
        public void Restart()
        {
            _timer.Stop();
            SecondsToCountDown = _initialSecondsToCountDown;
            UpdateUI(SecondsToCountDown);
            _stopwatch.Restart();
            _timer.Start();
        }
        public double GetTimeElapsed()
        {
            return _stopwatch.Elapsed.TotalSeconds;
        }
        #endregion

        #region Private helpers
        private void _timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            SecondsToCountDown--;
            if (SecondsToCountDown == 0)
            {
                _timer.Stop();
                _stopwatch.Stop();
                _dispatcher.Invoke(() =>
                {
                    UpdateUI(SecondsToCountDown);
                });
                CompletedAction(_stopwatch.Elapsed.TotalSeconds);
                return;
            }

            _dispatcher.Invoke(() =>
            {
                UpdateUI(SecondsToCountDown);
            });
        }
        #endregion
    }
}
