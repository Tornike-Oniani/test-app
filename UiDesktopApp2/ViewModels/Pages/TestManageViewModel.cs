using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Win32;
using System.Configuration;
using System.IO;
using UiDesktopApp2.DataAccess;
using UiDesktopApp2.DataAccess.Entities;
using UiDesktopApp2.DataAccess.Repositories;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;


namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class TestManageViewModel : ObservableObject
    {
        private readonly TestRepository _testRepo;
        private readonly IContentDialogService _contentDialogService;

        public GlobalState GlobalState { get; set; }

        public TestManageViewModel(TestRepository testRepo, GlobalState globalState, IContentDialogService contentDialogService)
        {
            _testRepo = testRepo;
            _contentDialogService = contentDialogService;
            GlobalState = globalState;
        }

        [RelayCommand]
        public async Task OnAddSet()
        {
            GlobalState.TestToManage.ImageSets.Add(new ImageSetDTO());
            await _testRepo.UpdateTest(GlobalState.TestToManage);
        }

        [RelayCommand]
        public async Task RemoveSet(ImageSetDTO imageSet)
        {
            ContentDialogResult result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Create new subject",
                    Content = "Are you sure you want to delete the image set?",
                    PrimaryButtonText = "Delete",
                    CloseButtonText = "Cancel"
                }
            );

            // If user clicked yes and input was valid add the new test
            if (result == ContentDialogResult.Primary)
            {
                GlobalState.TestToManage.ImageSets.Remove(imageSet);
                await _testRepo.UpdateTest(GlobalState.TestToManage);
            }
        }

        [RelayCommand]
        private async Task OnAddImageVariant(ImageSetDTO imageSet)
        {            
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Multiselect = true
            };

            if (ofd.ShowDialog() == true)
            {
                string[] selectedFiles = ofd.FileNames;

                // Edge case: no files were selected
                if (selectedFiles.Length == 0)
                {
                    return;
                }

                // Validate the selection
                //  - files must have the same name with different numbers
                //  - file name must have following pattern "{name} {number}.{extension}"

                // Sort by number descending
                var sortedFiles = selectedFiles
                    .OrderByDescending(item =>
                    {
                        string nameWithoutExtension = Path.GetFileNameWithoutExtension(item);
                        string[] parts = nameWithoutExtension.Split(' ');
                        return int.Parse(parts[^1]); // Extract the last part and parse it as an integer
                    })
                    .ToArray();

                foreach (string file in sortedFiles)
                {
                    // Check anc create images folder in root
                    string imagesDir = Path.Combine(Environment.CurrentDirectory, "Images");
                    if (!Directory.Exists(imagesDir))
                    {
                        Directory.CreateDirectory(imagesDir);
                    }

                    string fileName = Path.GetFileName(file);
                    string localFilePath = Path.Combine(Environment.CurrentDirectory, "Images", fileName);

                    // Copy only if file doesn't exist
                    if (!File.Exists(localFilePath))
                    {
                        File.Copy(file, localFilePath);
                    }

                    // Add new path to db
                    ImageVariantDTO image = new ImageVariantDTO()
                    {
                        Name = fileName,
                        Source = localFilePath
                    };
                    imageSet.Images.Add(image);
                }
            }

            await _testRepo.UpdateTest(GlobalState.TestToManage);
        }
    }
}
