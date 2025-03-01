using UiDesktopApp2.Helpers;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class SettingsViewModel(Settings settings) : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _appVersion = String.Empty;
        [ObservableProperty]
        private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;
        [ObservableProperty]
        private int _imageTime = 10;
        [ObservableProperty]
        private double _transitionImageDuration = 2;
        [ObservableProperty]
        private bool _isTimerVisible = false;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom() 
        {
            settings.ImageTime = ImageTime;
            settings.TransitionImageDuration = TransitionImageDuration;
            settings.IsTimerVisible = IsTimerVisible;
        }

        private void InitializeViewModel()
        {
            CurrentTheme = ApplicationThemeManager.GetAppTheme();
            AppVersion = $"UiDesktopApp1 - {GetAssemblyVersion()}";

            _isInitialized = true;
        }

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                ?? String.Empty;
        }

        [RelayCommand]
        private void OnChangeTheme(string parameter)
        {
            switch (parameter)
            {
                case "theme_light":
                    if (CurrentTheme == ApplicationTheme.Light)
                        break;

                    ApplicationThemeManager.Apply(ApplicationTheme.Light);
                    CurrentTheme = ApplicationTheme.Light;

                    break;

                default:
                    if (CurrentTheme == ApplicationTheme.Dark)
                        break;

                    ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                    CurrentTheme = ApplicationTheme.Dark;

                    break;
            }
        }
    }
}
