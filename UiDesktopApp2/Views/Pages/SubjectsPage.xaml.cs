using UiDesktopApp2.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.Views.Pages
{
    /// <summary>
    /// Interaction logic for SubjectsPage.xaml
    /// </summary>
    public partial class SubjectsPage : INavigableView<SubjectsViewModel>
    {

        public SubjectsViewModel ViewModel { get; }

        public SubjectsPage(SubjectsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
