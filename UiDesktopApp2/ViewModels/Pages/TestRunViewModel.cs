using System.Diagnostics;
using System.Timers;
using System.Windows.Threading;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using UiDesktopApp2.Views.Pages;
using Wpf.Ui;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class TestRunViewModel : ObservableObject
    {
        #region Private memebers
        private readonly INavigationService _navigationService;
        private readonly GlobalState _globalState;
        private readonly UITimer _uiTimer;
        private readonly ResultTracker _resultTracker;
        private readonly ImageDisplay _imageDisplay;
        private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
        private bool _lastImageWasProcessed = false;
        #endregion

        #region Observable properties
        [ObservableProperty]
        private TestDTO _currentTest;
        [ObservableProperty]
        private string _currentImageSource;
        [ObservableProperty]
        private string _countdownText = "0";
        #endregion

        #region Constructors
        public TestRunViewModel(INavigationService navigationService, GlobalState globalState)
        {
            _navigationService = navigationService;
            _globalState = globalState;
            _currentTest = _globalState.TestToRun!;
            _resultTracker = new ResultTracker(CurrentTest.Id, globalState);
            _imageDisplay = new ImageDisplay();
            _imageDisplay.ImageSets = CurrentTest.ImageSets.ToList();
            _uiTimer = new UITimer(1)
            {
                SecondsToCountDown = 10,
                UpdateUI = UpdateUI,
                CompletedAction = OnTimerComplete
            };
            CurrentImageSource = _imageDisplay.GetNextImage();
            _uiTimer.Start();
        }
        #endregion

        #region Commands
        [RelayCommand]
        private void OnSkip()
        {
            // Track Time elapsed
            _resultTracker.TrackResult(_uiTimer.GetTimeElapsed(), _imageDisplay.IsNextImageAvailable());

            // If this was the last image finish the test
            if (_lastImageWasProcessed)
            {
                _navigationService.Navigate(typeof(TestResultPage));
                return;
            }

            // Display image
            CurrentImageSource = _imageDisplay.GetNextImage();
            _uiTimer.Restart();
            _lastImageWasProcessed = !_imageDisplay.IsNextImageAvailable() && !_imageDisplay.IsNextSetAvailable();
        }
        #endregion

        #region Private helpers
        private void UpdateUI(int seconds)
        {
            // Update timer on UI
            CountdownText = seconds.ToString();
        }
        private void OnTimerComplete(double timeElapsed)
        {            
            // Track Time elapsed
            _resultTracker.TrackResult(timeElapsed, _imageDisplay.IsNextImageAvailable());

            // If this was the last image finish the test
            if (_lastImageWasProcessed)
            {
                _dispatcher.Invoke(() =>
                {
                    _navigationService.Navigate(typeof(TestResultPage));
                });
                return;
            }

            // Display image
            CurrentImageSource = _imageDisplay.GetNextImage();
            _uiTimer.Start();
            _lastImageWasProcessed = !_imageDisplay.IsNextImageAvailable() && !_imageDisplay.IsNextSetAvailable();
        }
        #endregion
    }
}
