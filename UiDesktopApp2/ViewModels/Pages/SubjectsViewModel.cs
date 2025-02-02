using System.Collections.ObjectModel;
using UiDesktopApp2.DataAccess.Entities;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using UiDesktopApp2.Services;
using Wpf.Ui.Controls;
using Wpf.Ui;
using Wpf.Ui.Extensions;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class SubjectsViewModel(PersonRepository personRepo, IContentDialogService contentDialogService, GlobalState globalState) : ObservableObject
    {
        #region Public properties
        public GlobalState GlobalState { get { return globalState; } }
        #endregion

        #region Observable properties
        [ObservableProperty]
        private string? _newPersonFirstName;
        [ObservableProperty]
        private string? _newPersonLastName;
        #endregion

        #region Commands
        [RelayCommand]
        private async void OnPersonAdd(object content)
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
                var person = new Person()
                {
                    FirstName = NewPersonFirstName,
                    LastName = NewPersonLastName,
                };

                // Add test to database and retrieve its id
                await personRepo.CreatePerson(person);

                // Add test to global state
                GlobalState.Subjects.Add(person);
            }

            ClearForm();
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
        #endregion
    }
}
