using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UiDesktopApp2.DataAccess;
using UiDesktopApp2.DataAccess.Repositories;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using UiDesktopApp2.Views.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class DashboardViewModel(INavigationService navigationService, TestRepository testRepo, PersonRepository personRepo, GlobalState globalState, IServiceProvider serviceProvider) : ObservableObject, INavigationAware
    {
        #region Private members
        private bool _isInitialized = false;
        #endregion

        #region Global properties
        public GlobalState GlobalState 
        {
            get
            {
                return globalState;
            }
        }

        [ObservableProperty]
        private TestDTO _selectedTest;
        [ObservableProperty]
        private ObservableCollection<string> _temp = new ObservableCollection<string>()
        {
            "One",
            "Two",
            "Three",
            "Four",
            "Five"
        };
        #endregion

        #region Commands
        [RelayCommand]
        private void OnRunTest(Type type)
        {
            GlobalState.TestToRun = this.SelectedTest;
            Window testWindow = new TestWindow(serviceProvider);
            testWindow.Show();
            return;

            GlobalState.TestToRun = this.SelectedTest;
            _ = navigationService.NavigateWithHierarchy(type);
        }
        public void MoveItem(int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || newIndex < 0 || oldIndex >= Temp.Count || newIndex >= Temp.Count)
                return;

            var item = Temp[oldIndex];
            Temp.RemoveAt(oldIndex);
            Temp.Insert(newIndex, item);
        }
        #endregion

        #region INavigationAware
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
        #endregion

        #region Private helpers
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
        #endregion
    }
}
