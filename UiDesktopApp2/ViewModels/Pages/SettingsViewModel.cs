using System.IO;
using System.Runtime.CompilerServices;
using UiDesktopApp2.Helpers;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class SettingsViewModel(Settings settings) : ObservableObject, INavigationAware
    {
        private string _settingsFilePath = Path.Combine(Environment.CurrentDirectory, "settings.json");
        private JsonWriterReader<SettingsData> _jsonReaderWriter;
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _appVersion = String.Empty;
        [ObservableProperty]
        private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;
        [ObservableProperty]
        private double _imageTime;
        [ObservableProperty]
        private double _transitionImageDuration;
        [ObservableProperty]
        private bool _isTimerVisible;

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
            _jsonReaderWriter.WriteData(new SettingsData()
            {
                ImageTime = settings.ImageTime,
                TransitionImageDuration = settings.TransitionImageDuration,
                IsTimerVisible = settings.IsTimerVisible
            });
        }

        private void InitializeViewModel()
        {
            CurrentTheme = ApplicationThemeManager.GetAppTheme();
            _jsonReaderWriter = new JsonWriterReader<SettingsData>(_settingsFilePath);
            ImageTime = settings.ImageTime;
            TransitionImageDuration = settings.TransitionImageDuration;
            IsTimerVisible = settings.IsTimerVisible;

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
