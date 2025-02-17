using System.Windows.Controls;
using UiDesktopApp2.ViewModels.Pages;

namespace UiDesktopApp2.Views.Pages
{
    /// <summary>
    /// Interaction logic for TestResultPage.xaml
    /// </summary>
    public partial class TestResultPage : Page
    {
        public TestResultViewModel ViewModel { get; }

        public TestResultPage(TestResultViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
