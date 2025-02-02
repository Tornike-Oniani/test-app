using Microsoft.EntityFrameworkCore;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UiDesktopApp2.DataAccess;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using UiDesktopApp2.Services;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class DashboardViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly TestRepository _testRepo;
        private bool _isInitialized = false;

        public GlobalState GlobalState { get; set; }

        [ObservableProperty]
        private TestDTO _selectedTest;

        public DashboardViewModel(GlobalState globalState, INavigationService navigationService, TestRepository testRepo)
        {
            _navigationService = navigationService;
            _testRepo = testRepo;
            GlobalState = globalState;
        }        

        [RelayCommand]
        private void OnRunTest(Type type)
        {
            GlobalState.TestToRun = this.SelectedTest;
            _ = this._navigationService.NavigateWithHierarchy(type);
        }

        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
            {
                await Initialize();
            }
        }

        public void OnNavigatedFrom()
        {

        }

        private async Task Initialize()
        {
            List<TestDTO> tests = await _testRepo.GetAllTestsAsync();

            GlobalState.Tests.Clear();
            foreach (var test in tests)
            {
                GlobalState.Tests.Add(test);
            }

            _isInitialized = true;
        }
    }
}
