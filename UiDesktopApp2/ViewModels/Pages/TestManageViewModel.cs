using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.IO;
using UiDesktopApp2.DataAccess;
using UiDesktopApp2.DataAccess.Entities;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using UiDesktopApp2.Services;


namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class TestManageViewModel : ObservableObject
    {
        private TestRepository _testRepo;

        public GlobalState GlobalState { get; set; }

        public TestManageViewModel(TestRepository testRepo, GlobalState globalState)
        {
            _testRepo = testRepo;
            GlobalState = globalState;
        }

        [RelayCommand]
        public async void OnAddSet()
        {
            GlobalState.TestToManage.ImageSets.Add(new ImageSetDTO());
            await _testRepo.UpdateTest(GlobalState.TestToManage);
        }

        [RelayCommand]
        private async void OnAddImageVariant(ImageSetDTO imageSet)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            if (ofd.ShowDialog() == true)
            {
                ImageVariantDTO image = new ImageVariantDTO()
                {
                    Name = Path.GetFileName(ofd.FileName),
                    Source = ofd.FileName
                };
                imageSet.Images.Add(image);
            }

            await _testRepo.UpdateTest(GlobalState.TestToManage);
        }
    }
}
