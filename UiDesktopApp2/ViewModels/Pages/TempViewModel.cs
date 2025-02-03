using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class TempViewModel : ObservableObject
    {
        private DispatcherTimer _timer;
        private int SecondsToCountDown = 5;
        private int timerCycle = 2;
        private int currentCycle = 1;
        private DateTime startTime;
        private List<double> timeDifferences = new List<double>();
        private Stopwatch _stopwatch = new Stopwatch();

        [ObservableProperty]
        private string _textOnUI = "Cirkachia levani";

        public TempViewModel()
        {
            _timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += _timer_Tick;
        }

        [RelayCommand]
        private void OnTimerStart()
        {
            StartTimer();
        }

        private void StartTimer()
        {
            _stopwatch.Restart();
            SecondsToCountDown = 5;
            _timer_Tick(this, EventArgs.Empty);
            _timer.Start();
            startTime = DateTime.Now;
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            TextOnUI = SecondsToCountDown.ToString();
            SecondsToCountDown--;

            if (SecondsToCountDown == -1)
            {
                _timer.Stop();
                _stopwatch.Stop();
                timeDifferences.Add(_stopwatch.Elapsed.TotalSeconds);
                currentCycle++;
                if (currentCycle <= timerCycle)
                {
                    StartTimer();
                }
                else
                {
                    MessageBox.Show(
                        $"Difference in one: {timeDifferences[0]}\nDifference in two: {timeDifferences[1]}"
                        );
                }
            }
        }
    }
}
