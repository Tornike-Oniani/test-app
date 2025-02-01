using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class TestRunViewModel : ObservableObject
    {
        private readonly GlobalState _globalState;
        private int setIndex = 0;
        private int imageIndex = 0;

        [ObservableProperty]
        private TestDTO _currentTest;
        [ObservableProperty]
        private ImageSource _currentImageSource;

        public TestRunViewModel(GlobalState globalState)
        {
            _globalState = globalState;
            _currentTest = _globalState.TestToRun!;
            CurrentImageSource = GetDisplayImageSource();
        }

        [RelayCommand]
        private void OnSkip()
        {
            imageIndex++;
            if (imageIndex > CurrentTest.ImageSets[setIndex].Images.Count - 1)
            {
                imageIndex = 0;
                setIndex++;
            }
            CurrentImageSource = GetDisplayImageSource();
        }

        private ImageSource GetDisplayImageSource()
        {
            return CurrentTest.ImageSets[setIndex].Images[imageIndex].Source;
        }
    }
}
