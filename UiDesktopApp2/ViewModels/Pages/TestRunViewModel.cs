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
        private int _variantTime = 0;
        private int _imageSetTime = 0;
        private ResultDTO _result;
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
            _currentTest = _globalState.TestToRun;
            _result = new ResultDTO()
            {
                ImageSetTimes = new List<ResultImageSetTimeDTO>(),
                VariantTimes = new List<ResultImageVariantTimeDTO>()
            };
            _globalState.SubjectToTest.Results.Add(_result);
            _timer = new Timer(10)
            {
                Tick = TimerTick,
                Completed = TimerComplete
            };
            _timer.Start();
            CurrentImageSource = GetDisplayImageSource();
        }
        #endregion

        #region Commands
        [RelayCommand]
        private void OnSkip()
        {
            RestartTimer();
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
                _result.ImageSetTimes.Add(new ResultImageSetTimeDTO()
                {
                    Seconds = _imageSetTime
                });
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
            RestartTimer();
            SetNextImage();
        }
        private void RestartTimer()
        {
            _timer.Restart();
        }
        #endregion
    }
}
