using System.Windows.Controls;
using UiDesktopApp2.ViewModels.Pages;

namespace UiDesktopApp2.Views.Pages
{
    /// <summary>
    /// Interaction logic for TestRunPage.xaml
    /// </summary>
    public partial class TestRunPage : Page
    {
        public TestRunViewModel ViewModel { get; }

        public TestRunPage(TestRunViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();
        }
    }
}
