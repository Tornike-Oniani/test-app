using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer == null) return;

            bool canScroll = scrollViewer.ExtentWidth > scrollViewer.ViewportWidth;

            if (canScroll)
            {
                // Scroll the sublist if necessary
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
                e.Handled = true;  // Prevent parent scroll
            }
            else
            {
                // Forward event to parent manually
                var parent = FindParent<Page>(scrollViewer);
                parent?.RaiseEvent(new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent
                });
            }
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as T;
        }
    }
}
