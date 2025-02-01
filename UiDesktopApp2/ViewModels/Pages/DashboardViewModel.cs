using Microsoft.EntityFrameworkCore;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UiDesktopApp2.DataAccess;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class DashboardViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly ApplicationDbContext _context;
        private bool _isInitialized = false;

        public GlobalState GlobalState { get; set; }

        [ObservableProperty]
        private TestDTO _selectedTest;

        public DashboardViewModel(GlobalState globalState, INavigationService navigationService, ApplicationDbContext context)
        {
            _navigationService = navigationService;
            _context = context;
            GlobalState = globalState;
        }        

        [RelayCommand]
        private void OnRunTest(Type type)
        {
            GlobalState.TestToRun = this.SelectedTest;
            _ = this._navigationService.NavigateWithHierarchy(type);
        }

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

        private async Task Initialize()
        {
            List<TestDTO> tests = await _context.Tests
                .Include(t => t.ImageSets)
                .ThenInclude(ims => ims.ImageVariants)
                .Select(t => new TestDTO()
                                    {
                                        Id = t.Id,
                                        Name = t.Name
                                    })
                .ToListAsync();

            GlobalState.Tests.Clear();
            foreach (var test in tests)
            {
                GlobalState.Tests.Add(test);
            }

            _isInitialized = true;
        }
    }
}
