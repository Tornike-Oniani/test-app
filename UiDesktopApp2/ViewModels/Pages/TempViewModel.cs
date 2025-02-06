using System.Timers;
using System.Windows.Threading;
using System.Diagnostics;
using System.Collections.ObjectModel;

public partial class TempViewModel : ObservableObject
{
    private System.Timers.Timer _timer;
    private int SecondsToCountDown;
    private Stopwatch _stopwatch;
    private readonly Dispatcher _dispatcher;

    [ObservableProperty]
    private string _textOnUI = "Cirkachia levani";
    public ObservableCollection<double> TimeDiffernceForEachCycle { get; set; } = new ObservableCollection<double>();

    public TempViewModel()
    {
        _dispatcher = Dispatcher.CurrentDispatcher; // Get the UI dispatcher
        _timer = new System.Timers.Timer(1000); // Set interval to 1 second
        _timer.Elapsed += _timer_Elapsed;
        _timer.AutoReset = true;
        _stopwatch = new Stopwatch();
    }

    [RelayCommand]
    private void OnTimerStart()
    {
        StartTimer();
    }

    private void StartTimer()
    {
        SecondsToCountDown = 30;
        _stopwatch.Restart(); // Start high-precision timing
        _dispatcher.Invoke(() => TextOnUI = SecondsToCountDown.ToString()); // Update UI immediately
        _timer.Start();
    }

    private void _timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        SecondsToCountDown--;

        if (SecondsToCountDown == 0)
        {
            _timer.Stop();
            _stopwatch.Stop();
            _dispatcher.Invoke(() =>
                TimeDiffernceForEachCycle.Add(_stopwatch.Elapsed.TotalSeconds)
            );
            StartTimer();
            return;
        }

        _dispatcher.Invoke(() => TextOnUI = SecondsToCountDown.ToString()); // Update UI
    }
}
