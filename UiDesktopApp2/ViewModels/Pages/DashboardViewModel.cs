using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class DashboardViewModel : ObservableObject
    {
        private string _imagePath = "pack://application:,,,/Assets/";

        [ObservableProperty]
        private int _counter = 5;
        [ObservableProperty]
        private ImageSource _currentImageSource;

        public DashboardViewModel()
        {
            this.CurrentImageSource = new BitmapImage(new Uri($"{this._imagePath}{this.Counter}.png"));
        }

        [RelayCommand]
        private void OnCounterIncrement()
        {
            Counter++;
        }

        [RelayCommand]
        private void OnSkip()
        {
            this.Counter--;
            this.CurrentImageSource = new BitmapImage(new Uri($"{this._imagePath}{this.Counter}.png"));
        }
    }
}
