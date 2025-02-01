using System.Windows.Controls;
using UiDesktopApp2.ViewModels.Pages;

namespace UiDesktopApp2.Views.Pages
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestManagePage : Page
    {
        public TestManageViewModel ViewModel { get; }

        public TestManagePage(TestManageViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();
        }
    }
}
