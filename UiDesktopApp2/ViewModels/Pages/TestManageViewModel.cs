using Microsoft.Win32;
using System.IO;
using UiDesktopApp2.DataAccess.Repositories;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;


namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class TestManageViewModel(TestRepository testRepo, ImageSetRepository imageSetRepo, GlobalState globalState, IContentDialogService contentDialogService) : ObservableObject
    {
        #region Public properties
        public GlobalState GlobalState 
        {
            get
            {
                return globalState;
            }
        }
        public bool HasTestAlreadyRun 
        { 
            get
            {
                return GlobalState.TestToManage.HasAlreadyRun;
            }
        }
        #endregion

        #region Commands
        [RelayCommand]
        public async Task OnAddSet()
        {
            ImageSetDTO imageSet = new ImageSetDTO()
            {
                TestId = GlobalState.TestToManage.Id
            };
            int id = await imageSetRepo.CreateImageSet(imageSet);
            imageSet.Id = id;
            GlobalState.TestToManage.ImageSets.Add(imageSet);
        }

        [RelayCommand]
        public async Task OnRemoveSet(ImageSetDTO imageSet)
        {
            ContentDialogResult result = await contentDialogService.ShowSimpleDialogAsync(
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
                await testRepo.UpdateTest(GlobalState.TestToManage);
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

                // Set image name based on file
                string imageSetName = Path.GetFileNameWithoutExtension(selectedFiles[0]).Split(' ')[0];
                imageSet.Name = imageSetName;

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

            await testRepo.UpdateTest(GlobalState.TestToManage);
        }

        [RelayCommand]
        private async Task OnRemoveImageVariant(object parameter)
        {
            if (parameter is object[] parameters && parameters.Length == 2)
            {
                ImageSetDTO imageSet = parameters[0] as ImageSetDTO;
                ImageVariantDTO imageVariant = parameters[1] as ImageVariantDTO;
                imageSet.Images.Remove(imageVariant);
                await testRepo.UpdateTest(GlobalState.TestToManage);
                return;
            }

            throw new ArgumentException("Invalid argument");
        }

        [RelayCommand]
        private async Task OnApplyUnknownStatus(ImageSetDTO imageSet)
        {
            await imageSetRepo.UpdateImageSet(imageSet);
        }
        #endregion
    }
}
