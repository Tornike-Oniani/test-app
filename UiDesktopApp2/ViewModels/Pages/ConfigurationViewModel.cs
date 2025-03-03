using System.Collections.ObjectModel;
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
        private readonly ResultRepository _resultRepo;
        #endregion

        #region Observable properties
        [ObservableProperty]
        private string? _newTestName;
        [ObservableProperty]
        private string _currentTestNameToRename;
        [ObservableProperty]
        private string _testRenameName;
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
                ResultRepository resultRepo,
                GlobalState globalState
            )
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
            _testRepo = testRepo;
            _resultRepo = resultRepo;
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

        [RelayCommand]
        private void OnOpenFlyout(TestDTO test)
        {
            if (!test.IsFlyoutOpen)
            {
                test.IsFlyoutOpen = true;
            }
        }

        [RelayCommand]
        private async Task OnRenameTest(object parameter)
        {
            if (parameter is object[] parameters && parameters.Length == 2)
            {
                TestDTO test = parameters[0] as TestDTO;
                object content = parameters[1] as object;
                CurrentTestNameToRename = test.Name;

                ContentDialogResult result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Rename test",
                    Content = content,
                    PrimaryButtonText = "Save",
                    CloseButtonText = "Cancel"
                }
            );

                // If user clicked yes and input was valid add the new test
                if (result == ContentDialogResult.Primary && !String.IsNullOrEmpty(TestRenameName))
                {
                    test.Name = TestRenameName;

                    await _testRepo.UpdateTest(test);
                }

                TestRenameName = String.Empty;
            }
            else
            {
                throw new ArgumentException("Invalid argument");
            }
        }

        [RelayCommand]
        private async Task OnDuplicateTest(TestDTO test)
        {
            // Create a test with copied name
            TestDTO testCopy = new TestDTO()
            {
                Name = $"{test.Name} copy",
            };

            // Add the copy to database to set the id
            int id = await _testRepo.CreateTest(testCopy);
            testCopy.Id = id;

            // Copy image sets
            List<ImageSetDTO> imageSetsCopy = new List<ImageSetDTO>();

            foreach (ImageSetDTO imageSet in test.ImageSets)
            {
                // Copy image variants for each set copy
                List<ImageVariantDTO> imageVariantsCopy = new List<ImageVariantDTO>();

                foreach (ImageVariantDTO imageVariant in imageSet.Images)
                {
                    imageVariantsCopy.Add(new ImageVariantDTO()
                    {
                        Id = 0,
                        Name = imageVariant.Name,
                        Source = imageVariant.Source
                    });
                }

                ImageSetDTO imageSetCopy = new ImageSetDTO()
                {
                    Id = 0,
                    TestId = id,
                    Name = imageSet.Name,
                    IsUknown = imageSet.IsUknown,
                    Images = new ObservableCollection<ImageVariantDTO>(imageVariantsCopy)
                };

                imageSetsCopy.Add(imageSetCopy);
            }

            // Update the copy test to include image set copies
            testCopy.ImageSets = new ObservableCollection<ImageSetDTO>(imageSetsCopy);
            GlobalState.Tests.Add(testCopy);
            await _testRepo.UpdateTest(testCopy);
        }

        [RelayCommand]
        private async Task OnDeleteTest(TestDTO test)
        {
            ContentDialogResult result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Delete test",
                    Content = $"Are you sure you want to delete '{test.Name}'? It will also delete corresponding results.",
                    PrimaryButtonText = "Delete",
                    CloseButtonText = "Cancel"
                }
            );

            // If user clicked yes and input was valid add the new test
            if (result == ContentDialogResult.Primary)
            {
                await _testRepo.DeleteTestById(test.Id);
                GlobalState.Tests.Remove(test);
            }
        }
        #endregion

        #region INavigationAware
        public async void OnNavigatedTo()
        {
            // Update test has already run status
            foreach (var test in GlobalState.Tests)
            {
                test.HasAlreadyRun = await _resultRepo.HasTestAlreadyRun(test.Id);
            }

            // Check empty sets
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
