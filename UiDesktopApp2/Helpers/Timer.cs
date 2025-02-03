using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace UiDesktopApp2.Helpers
{
    public class Timer
    {
        #region Private helpers
        private readonly DispatcherTimer _timer;
        private int _initialSeconds;
        #endregion

        #region Public properties
        public int Seconds { get; private set; }
        public Action<int>? Tick { get; set; }
        public Action? Completed { get; set; }
        #endregion

        #region Constructors
        public Timer(int seconds)
        {
            _initialSeconds = seconds;
            Seconds = seconds;
            _timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += _timer_Tick;
        }
        #endregion

        #region Public methods
        // Set the duration of the timer
        public void SetSeconds(int seconds)
        {
            Seconds = seconds;
            _initialSeconds = seconds;
        }
        // Start the timer
        public void Start()
        {
            _timer.Start();
        }
        // Pause the timer but keep current elapsed seconds
        public void Pause()
        {
            _timer.Stop();
        }
        // Stop the timer and reset the seconds, but don't start again
        public void Reset()
        {
            _timer.Stop();
            Seconds = _initialSeconds;
        }
        // Reset the seconds and restart the timer
        public void Restart()
        {
            //_timer.Stop();
            Seconds = _initialSeconds;
            //_timer.Start();
        }
        #endregion

        #region Private helpers
        private void _timer_Tick(object? sender, EventArgs e)
        {
            // If no action was passed throw an exception
            if (Tick == null)
            {
                throw new MissingMemberException(nameof(Tick));
            }

            // Invoke passed function until seconds are fully elapsed
            if (Seconds > 0)
            {
                Tick(Seconds);
                Seconds--;
            }
            else
            {
                // Stop the timer and invoke complete action if it was passeed
                //_timer.Stop();
                if (Completed != null)
                {
                    Completed();
                }
            }
        }
        #endregion
    }
}
