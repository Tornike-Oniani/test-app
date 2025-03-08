using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using UiDesktopApp2.DataAccess.Repositories;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;


namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class TestManageViewModel(TestRepository testRepo, ImageSetRepository imageSetRepo, GlobalState globalState, IContentDialogService contentDialogService, ISnackbarService snackbarService) : ObservableObject
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
                TestId = GlobalState.TestToManage.Id,
                Number = GlobalState.TestToManage.ImageSets.Count + 1
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
                try
                {
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

                    await testRepo.UpdateTest(GlobalState.TestToManage);
                }
                catch(Exception ex)
                {
                    snackbarService.Show(
                        "Error",
                        "Something went wrong, make sure that files have correct name of pattern - name followed by space followed by number.",
                        Wpf.Ui.Controls.ControlAppearance.Secondary,
                        new SymbolIcon(SymbolRegular.ErrorCircle24),
                        TimeSpan.FromSeconds(7)
                        );
                }
            }
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
        [RelayCommand]
        private void OnEnterNumberEditMode(ImageSetDTO imageSet)
        {
            imageSet.IsNumberInEditMode = true;
            imageSet.IsEditModeFocused = true;
        }
        [RelayCommand]
        private void OnExitNumberEditMode(ImageSetDTO imageSet)
        {
            imageSet.IsNumberInEditMode = false;
            imageSet.IsEditModeFocused = false;
        }
        [RelayCommand]
        private async Task OnChangeNumber(object parameter)
        {
            if (parameter is object[] parameters && parameters.Length == 2)
            {
                ImageSetDTO imageSet = parameters[0] as ImageSetDTO;
                string input = parameters[1] as string;
                int newNumber;
                ObservableCollection<ImageSetDTO> imageSets = GlobalState.TestToManage.ImageSets;

                if (!int.TryParse(input, out newNumber))
                {
                    return;
                }

                if (newNumber > imageSets.Count || newNumber < 1)
                {
                    return;
                }

                // Find the imageset at the new index
                ImageSetDTO targetImageSet = imageSets[newNumber - 1];
                targetImageSet.Number = imageSet.Number;
                imageSet.Number = newNumber;
                SwapItems<ImageSetDTO>(imageSets, newNumber - 1, targetImageSet.Number - 1);

                imageSet.IsNumberInEditMode = false;
                imageSet.IsEditModeFocused = false;

                await testRepo.UpdateTest(GlobalState.TestToManage);
                return;
            }

            throw new ArgumentException("Invalid argument");
        }
        [RelayCommand]
        private async Task OnMoveUp(ImageSetDTO imageSet)
        {
            if (imageSet.Number <= 1)
            {
                return;
            }

            ObservableCollection<ImageSetDTO> imageSets = GlobalState.TestToManage.ImageSets;
            int newNumber = imageSet.Number - 1;
            ImageSetDTO targetImageSet = imageSets[newNumber - 1];
            targetImageSet.Number = imageSet.Number;
            imageSet.Number = newNumber;
            SwapItems<ImageSetDTO>(imageSets, newNumber - 1, targetImageSet.Number - 1);

            await testRepo.UpdateTest(GlobalState.TestToManage);
        }
        [RelayCommand]
        private async Task OnMoveDown(ImageSetDTO imageSet)
        {
            ObservableCollection<ImageSetDTO> imageSets = GlobalState.TestToManage.ImageSets;

            if (imageSet.Number >= imageSets.Count)
            {
                return;
            }

            int newNumber = imageSet.Number + 1;
            ImageSetDTO targetImageSet = imageSets[newNumber - 1];
            targetImageSet.Number = imageSet.Number;
            imageSet.Number = newNumber;
            SwapItems<ImageSetDTO>(imageSets, newNumber - 1, targetImageSet.Number - 1);

            await testRepo.UpdateTest(GlobalState.TestToManage);
        }
        #endregion

        #region Private helpers
        void SwapItems<T>(ObservableCollection<T> collection, int index1, int index2)
        {
            if (index1 >= 0 && index1 < collection.Count && index2 >= 0 && index2 < collection.Count && index1 != index2)
            {
                collection.Move(index1, index2);

                // Adjust the second move to account for index shift
                if (index2 > index1)
                {
                    collection.Move(index2 - 1, index1);
                }
                else
                {
                    collection.Move(index2 + 1, index1);
                }
            }
        }
        #endregion
    }
}
