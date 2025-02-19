
using System.Windows.Threading;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using UiDesktopApp2.Views.Pages;
using Wpf.Ui.Controls;
using Wpf.Ui;
using Wpf.Ui.Extensions;
using UiDesktopApp2.Views.Windows;

namespace UiDesktopApp2.ViewModels.Windows
{
    public partial class TestWindowViewModel : ObservableObject
    {
        #region Private memebers
        private readonly INavigationService _navigationService;
        private readonly GlobalState _globalState;
        private readonly IContentDialogService _contentDialogService;
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
        [ObservableProperty]
        private string _recognizeImageName = "";
        #endregion

        #region Constructors
        public TestWindowViewModel(INavigationService navigationService, GlobalState globalState, IContentDialogService contentDialogService, Settings settings)
        {
            _navigationService = navigationService;
            _globalState = globalState;
            _contentDialogService = contentDialogService;
            _currentTest = _globalState.TestToRun!;
            _resultTracker = new ResultTracker(CurrentTest.Id, globalState);
            _imageDisplay = new ImageDisplay();
            _imageDisplay.ImageSets = CurrentTest.ImageSets.ToList();
            _uiTimer = new UITimer(1)
            {
                SecondsToCountDown = settings.ImageTime,
                UpdateUI = UpdateUI,
                CompletedAction = OnTimerComplete
            };
            CurrentImageSource = _imageDisplay.GetCurrentImage();
            _uiTimer.Start();
        }
        #endregion

        #region Commands
        [RelayCommand]
        private void OnSkip()
        {
            // Track Time elapsed
            _resultTracker.TrackResult(_uiTimer.GetTimeElapsed(), _imageDisplay.IsNextImageAvailable(), skipped: true);

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

        [RelayCommand]
        private async Task OnRecognize(TestWindow window)
        {
            // Track Time elapsed
            _resultTracker.TrackResult(_uiTimer.GetTimeElapsed(), _imageDisplay.IsNextSetAvailable(), recognized: true);

            // If no more sets are available finish the test
            if (!_imageDisplay.IsNextSetAvailable())
            {
                window.Close();
                _navigationService.Navigate(typeof(TestResultPage));
                return;
            }

            // Jump to the next set
            CurrentImageSource = _imageDisplay.JumpToNextSet();
            _uiTimer.Restart();
            _lastImageWasProcessed = !_imageDisplay.IsNextImageAvailable() && !_imageDisplay.IsNextSetAvailable();
            RecognizeImageName = String.Empty;

        }
        #endregion

        #region Public methods
        public void StopTest()
        {
            _uiTimer.Pause();
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
        private async Task HandleRecoginzeDialog(object content)
        {
            ContentDialogResult result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Recoginze image",
                    Content = content,
                    PrimaryButtonText = "Recognize",
                    CloseButtonText = "Cancel"
                }
            );

            if (result == ContentDialogResult.Primary)
            {
                // Jump to the next set
                if (!_imageDisplay.IsNextSetAvailable())
                {
                    _navigationService.Navigate(typeof(TestResultPage));
                    return;
                }
                CurrentImageSource = _imageDisplay.JumpToNextSet();
                _uiTimer.Restart();
                _lastImageWasProcessed = !_imageDisplay.IsNextImageAvailable() && !_imageDisplay.IsNextSetAvailable();
                RecognizeImageName = String.Empty;
            }
            else
            {
                // Continue current image and timer
                _uiTimer.Resume();
                RecognizeImageName = String.Empty;
            }
        }
        #endregion
    }
}
