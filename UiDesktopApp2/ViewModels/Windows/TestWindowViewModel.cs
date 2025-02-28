
using System.Windows.Threading;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using UiDesktopApp2.Views.Pages;
using Wpf.Ui.Controls;
using Wpf.Ui;
using Wpf.Ui.Extensions;
using UiDesktopApp2.Views.Windows;
using UiDesktopApp2.DataAccess.Repositories;
using System.IO;

namespace UiDesktopApp2.ViewModels.Windows
{
    public partial class TestWindowViewModel : ObservableObject
    {
        #region Private memebers
        private readonly GlobalState _globalState;
        private readonly UITimer _uiTimer;
        private readonly UITimer _middleUITimer;
        private readonly ResultTracker _resultTracker;
        private readonly ImageDisplay _imageDisplay;
        private readonly ResultRepository _resultRepo;
        private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
        private TestWindow _window;
        private bool _lastImageWasProcessed = false;
        private bool _imageWasRecognized = false;
        #endregion

        #region Observable properties
        [ObservableProperty]
        private TestDTO _currentTest;
        [ObservableProperty]
        private string _currentImageSource;
        [ObservableProperty]
        private string _countdownText = "0";
        [ObservableProperty]
        private bool _isMiddleImageBeingShown = false;
        #endregion

        #region Constructors
        public TestWindowViewModel(
            GlobalState globalState, 
            Settings settings,
            ResultRepository resultRepo,
            TestWindow window
            )
        {
            _globalState = globalState;
            _currentTest = _globalState.TestToRun!;
            _resultTracker = new ResultTracker(CurrentTest.Id, _globalState.SubjectToTest.Id, globalState, settings);
            _imageDisplay = new ImageDisplay();
            _imageDisplay.ImageSets = CurrentTest.ImageSets.ToList();
            _resultRepo = resultRepo;
            _window = window;
            _uiTimer = new UITimer(1)
            {
                SecondsToCountDown = settings.ImageTime,
                UpdateUI = UpdateUI,
                CompletedAction = OnTimerComplete
            };
            _middleUITimer = new UITimer(1)
            {
                SecondsToCountDown = settings.MiddleImageTime,
                UpdateUI = UpdateUI,
                CompletedAction = OnMiddleTimerComplete
            };
            CurrentImageSource = _imageDisplay.GetCurrentImage();
            _uiTimer.Start();
        }
        #endregion

        #region Commands
        [RelayCommand]
        private async Task OnRecognize()
        {
            // If middle image is being shown there is nothing to recognize
            if (IsMiddleImageBeingShown)
            {
                return;
            }

            // Track Time elapsed
            _resultTracker.TrackResult(
                _imageDisplay.GetCurrentSetId(),
                _uiTimer.GetTimeElapsed(), 
                _imageDisplay.IsNextSetAvailable(), 
                recognized: true
                );

            // If no more sets are available finish the test
            if (!_imageDisplay.IsNextSetAvailable())
            {
                await FinishTest();
                return;
            }

            _uiTimer.Pause();
            // Show middle image
            ShowMiddleImage();
            _imageWasRecognized = true;            
            _middleUITimer.Start();
            // Jump to the next set
            //HandleNextImage(_imageDisplay.JumpToNextSet());
            //CurrentImageSource = _imageDisplay.JumpToNextSet();
            //_uiTimer.Restart();
            //_lastImageWasProcessed = !_imageDisplay.IsNextImageAvailable() && !_imageDisplay.IsNextSetAvailable();
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
        private async void OnTimerComplete(double timeElapsed)
        {
            // Track Time elapsed
            _resultTracker.TrackResult(
                _imageDisplay.GetCurrentSetId(),
                timeElapsed, 
                _imageDisplay.IsNextImageAvailable()
                );

            // If this was the last image finish the test
            if (_lastImageWasProcessed)
            {
                await FinishTest();
                return;
            }
          
            _uiTimer.Pause();
            // If next image comes from the next set show middle image first
            if (!_imageDisplay.IsThereNextImageInSet())
            {
                ShowMiddleImage();
                _middleUITimer.Start();
            }
            // Display next image
            else
            {
               HandleNextImage(_imageDisplay.GetNextImage());
            }
            
            //CurrentImageSource = _imageDisplay.GetNextImage();
            //_uiTimer.Restart();
            //_lastImageWasProcessed = !_imageDisplay.IsNextImageAvailable() && !_imageDisplay.IsNextSetAvailable();
        }
        private void OnMiddleTimerComplete(double timeElapsed)
        {
            string nextImageSource = _imageWasRecognized ? _imageDisplay.JumpToNextSet() : _imageDisplay.GetNextImage();
            HandleNextImage(nextImageSource);
            _imageWasRecognized = false;
            IsMiddleImageBeingShown = false;
        }
        private async Task FinishTest()
        {
            _uiTimer.Pause();
            await _dispatcher.InvokeAsync(() =>
            {
                _window.Close();
            });
            await _resultRepo.CreateResult(_resultTracker.GetResult());
        }
        private void HandleNextImage(string imageSource)
        {
            CurrentImageSource = imageSource;
            _uiTimer.Restart();
            _lastImageWasProcessed = !_imageDisplay.IsNextImageAvailable() && !_imageDisplay.IsNextSetAvailable();
        }
        private void ShowMiddleImage()
        {
            IsMiddleImageBeingShown = true;
            CurrentImageSource = Path.Combine(Environment.CurrentDirectory, "შუა.png");
        }
        #endregion
    }
}
