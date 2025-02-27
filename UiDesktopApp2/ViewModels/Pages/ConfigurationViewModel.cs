using UiDesktopApp2.DataAccess.Repositories;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class ConfigurationViewModel : ObservableObject, INavigationAware
    {
        #region Private memebers
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly TestRepository _testRepo;
        #endregion

        #region Observable properties
        [ObservableProperty]
        private string? _newTestName;
        #endregion

        #region Public properties
        public GlobalState GlobalState { get; set; }
        #endregion

        #region Constructors
        public ConfigurationViewModel
            (
                INavigationService navigationService, 
                IContentDialogService contentDialogService,
                TestRepository testRepo,
                GlobalState globalState
            )
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
            _testRepo = testRepo;
            GlobalState = globalState;
        }
        #endregion

        #region Commands
        [RelayCommand]
        private async Task OnShowDialog(object content)
        {
            ContentDialogResult result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Create new test",
                    Content = content,
                    PrimaryButtonText = "Save",
                    CloseButtonText = "Cancel"
                }
            );

            // If user clicked yes and input was valid add the new test
            if (result == ContentDialogResult.Primary && !String.IsNullOrEmpty(NewTestName))
            {

                var testDto = new TestDTO()
                {
                    Name = NewTestName
                };

                // Add test to database and retrieve its id
                int id = await _testRepo.CreateTest(testDto);

                // Set the id to our local copy in runtime memory
                testDto.Id = id;

                // Add test to global state
                GlobalState.Tests.Add(testDto);
            }

            NewTestName = String.Empty;
        }

        [RelayCommand]
        private void OnNavigateForward(object parameter)
        {
            if (parameter is object[] parameters && parameters.Length == 2)
            {
                TestDTO test = parameters[0] as TestDTO;
                Type pageType = parameters[1] as Type;
                GlobalState.TestToManage = test;
                _ = this._navigationService.NavigateWithHierarchy(pageType);
                return;
            }

            throw new ArgumentException("Invalid argument");
        }

        public void OnNavigatedTo()
        {
            foreach (var test in GlobalState.Tests)
            {
                test.CheckForEmptySets();
            }
        }

        public void OnNavigatedFrom()
        {

        }
        #endregion
    }
}
