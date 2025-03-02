using System.Collections.ObjectModel;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using Wpf.Ui.Controls;
using Wpf.Ui;
using Wpf.Ui.Extensions;
using UiDesktopApp2.DataAccess.Repositories;
using UiDesktopApp2.Views.Pages;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class SubjectsViewModel(SubjectRepository subjectRepo, ResultRepository resultRepo, TestRepository testRepo, IContentDialogService contentDialogService, GlobalState globalState, INavigationService navigationService) : ObservableObject, INavigationAware
    {
        #region Public properties
        public GlobalState GlobalState { get { return globalState; } }
        public ObservableCollection<SubjectDTO> Subjects { get; set; } = new ObservableCollection<SubjectDTO>();
        #endregion

        #region Observable properties
        [ObservableProperty]
        private string? _newPersonFirstName;
        [ObservableProperty]
        private string? _newPersonLastName;
        #endregion

        #region Commands
        [RelayCommand]
        private async Task OnPersonAdd(object content)
        {
            ContentDialogResult result = await contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Create new subject",
                    Content = content,
                    PrimaryButtonText = "Save",
                    CloseButtonText = "Cancel"
                }
            );

            // If user clicked yes and input was valid add the new test
            if (result == ContentDialogResult.Primary && IsPersonFormValid())
            {
                var personDto = new SubjectDTO()
                {
                    FirstName = NewPersonFirstName,
                    LastName = NewPersonLastName,
                };

                // Add test to database and retrieve its id
                int id = await subjectRepo.CreateSubject(personDto);
                personDto.Id = id;

                // Add test to global state
                GlobalState.Subjects.Add(personDto);

                await Initialize();
            }

            ClearForm();
        }

        [RelayCommand]
        private void OnOpenTestResult(ResultDTO result)
        {
            globalState.ResultToBrowse = result;
            _ = navigationService.NavigateWithHierarchy(typeof(TestResultPage));
        }

        [RelayCommand]
        private void OnOpenFlyout(ResultDTO result)
        {
            if (!result.IsFlyoutOpen)
            {
                result.IsFlyoutOpen = true;
            }
        }

        [RelayCommand]
        private async Task OnDeleteResult(ResultDTO result)
        {
            ContentDialogResult dialogResult = await contentDialogService.ShowSimpleDialogAsync(
               new SimpleContentDialogCreateOptions()
               {
                   Title = "Delete result",
                   Content = $"Are you sure you want to delete the result of\n'{result.Test.Name}' test?",
                   PrimaryButtonText = "Delete",
                   CloseButtonText = "Cancel"
               }
           );

            if (dialogResult == ContentDialogResult.Primary)
            {
                await resultRepo.DeleteResult(result.Id);
                await Initialize();
            }
        }

        [RelayCommand]
        private async Task OnDeleteSubject(SubjectDTO subject)
        {
            ContentDialogResult dialogResult = await contentDialogService.ShowSimpleDialogAsync(
               new SimpleContentDialogCreateOptions()
               {
                   Title = "Delete subject",
                   Content = $"Are you sure you want to delete '{subject.FullName}'\nand all their results?",
                   PrimaryButtonText = "Delete",
                   CloseButtonText = "Cancel"
               }
           );

            if (dialogResult == ContentDialogResult.Primary)
            {
                // Remove from database
                await subjectRepo.DeleteSubject(subject.Id);
                await Initialize();

                // Remove from memory
                globalState.Subjects.Remove(globalState.Subjects.Single(s => s.Id == subject.Id));
            }
        }
        #endregion

        #region INavigationAware
        public async void OnNavigatedTo()
        {
            await Initialize();
        }
        public void OnNavigatedFrom()
        {
            
        }
        #endregion

        #region Private helpers
        private bool IsPersonFormValid()
        {
            return !String.IsNullOrEmpty(NewPersonFirstName) && !String.IsNullOrEmpty(NewPersonLastName);
        }
        private void ClearForm()
        {
            NewPersonFirstName = String.Empty;
            NewPersonLastName = String.Empty;
        }
        private async Task Initialize()
        {
            List<SubjectDTO> subjects = await subjectRepo.GetAllSubjectsWithTests();
            Subjects.Clear();
            foreach (SubjectDTO subject in subjects)
            {
                Subjects.Add(subject);
            }
        }
        #endregion
    }
}
