using Microsoft.EntityFrameworkCore;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UiDesktopApp2.DataAccess;
using UiDesktopApp2.DataAccess.Repositories;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class DashboardViewModel(INavigationService navigationService, TestRepository testRepo, PersonRepository personRepo, GlobalState globalState) : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        public GlobalState GlobalState 
        {
            get
            {
                return globalState;
            }
        }

        [ObservableProperty]
        private TestDTO _selectedTest;   

        [RelayCommand]
        private void OnRunTest(Type type)
        {
            GlobalState.TestToRun = this.SelectedTest;
            _ = navigationService.NavigateWithHierarchy(type);
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
            List<PersonDTO> subjects = await personRepo.GetAllPeopleAsync();

            GlobalState.Subjects.Clear();
            foreach (var subject in subjects)
            {
                GlobalState.Subjects.Add(subject);
            }

            List<TestDTO> tests = await testRepo.GetAllTestsAsync();

            GlobalState.Tests.Clear();
            foreach (var test in tests)
            {
                GlobalState.Tests.Add(test);
            }

            _isInitialized = true;
        }
    }
}
