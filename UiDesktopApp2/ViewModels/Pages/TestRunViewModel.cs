using System.Diagnostics;
using System.Timers;
using System.Windows.Threading;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using Timer = UiDesktopApp2.Helpers.Timer;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class TestRunViewModel : ObservableObject
    {
        #region Private memebers
        private readonly GlobalState _globalState;
        private int setIndex = 0;
        private int imageIndex = 0;
        private Timer _timer;
        #endregion

        #region Observable properties
        [ObservableProperty]
        private TestDTO _currentTest;
        [ObservableProperty]
        private string _currentImageSource;
        [ObservableProperty]
        private string _countdownText = "00:00";
        #endregion

        #region Constructors
        public TestRunViewModel(GlobalState globalState)
        {
            _globalState = globalState;
            _currentTest = _globalState.TestToRun!;
            CurrentImageSource = GetDisplayImageSource();
            _timer = new Timer(2)
            {
                Tick = TimerTick,
                Completed = TimerComplete
            };
            _timer.Start();
        }
        #endregion

        #region Commands
        [RelayCommand]
        private void OnSkip()
        {
            SetNextImage();
        }
        #endregion

        #region Private helpers
        private string GetDisplayImageSource()
        {
            return CurrentTest.ImageSets[setIndex].Images[imageIndex].Source;
        }
        private void SetNextImage()
        {
            if (IsNextImageAvailable())
            {
                imageIndex++;
            }
            else if (IsNextSetAvailable())
            {
                imageIndex = 0;
                setIndex++;
            }
            else
            {
                // TODO navigate to results page
                _timer.Reset();
                return;
            }
            CurrentImageSource = GetDisplayImageSource();
        }
        private bool IsNextSetAvailable()
        {
            return setIndex + 1 < CurrentTest.ImageSets.Count;
        }
        private bool IsNextImageAvailable()
        {
            return imageIndex + 1 < CurrentTest.ImageSets[setIndex].Images.Count;
        }
        private void TimerTick(int seconds)
        {
            CountdownText = TimeSpan.FromSeconds(seconds).ToString("mm':'ss");
        }
        private void TimerComplete()
        {
            SetNextImage();
            _timer.Restart();
        }
        #endregion
    }
}
