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
    public partial class DashboardViewModel(INavigationService navigationService, TestRepository testRepo, SubjectRepository personRepo, ResultRepository resultRepository, GlobalState globalState, IServiceProvider serviceProvider) : ObservableObject, INavigationAware
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
            this.SelectedTest.HasAlreadyRun = true;
            GlobalState.TestToRun = this.SelectedTest;
            Window testWindow = new TestWindow(serviceProvider);
            testWindow.Show();
            return;
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

            foreach (var test in GlobalState.Tests)
            {
                test.CheckForEmptySets();
            }
        }

        public void OnNavigatedFrom()
        {

        }
        #endregion

        #region Private helpers
        private async Task Initialize()
        {
            List<SubjectDTO> subjects = await personRepo.GetAllSubjects();

            GlobalState.Subjects.Clear();
            foreach (var subject in subjects)
            {
                GlobalState.Subjects.Add(subject);
            }

            List<TestDTO> tests = await testRepo.GetAllTestsAsync();

            GlobalState.Tests.Clear();
            foreach (var test in tests)
            {
                test.HasAlreadyRun = await resultRepository.HasTestAlreadyRun(test.Id);
                GlobalState.Tests.Add(test);
            }

            _isInitialized = true;
        }
        #endregion
    }
}
