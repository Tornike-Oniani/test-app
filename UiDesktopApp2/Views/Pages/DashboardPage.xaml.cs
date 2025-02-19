using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shell;
using UiDesktopApp2.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.ListViewItem)
            {
                System.Windows.Controls.ListViewItem draggedItem = sender as System.Windows.Controls.ListViewItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        private void ListViewItem_Drop(object sender, DragEventArgs e)
        {
            string droppedData = e.Data.GetData(typeof(string)) as string;
            string target = ((System.Windows.Controls.ListViewItem)(sender)).DataContext as string;

            int removedIdx = temp.Items.IndexOf(droppedData);
            int targetIdx = temp.Items.IndexOf(target);

            if (removedIdx < targetIdx)
            {
                ViewModel.Temp.Insert(targetIdx + 1, droppedData);
                ViewModel.Temp.RemoveAt(removedIdx);
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (ViewModel.Temp.Count + 1 > remIdx)
                {
                    ViewModel.Temp.Insert(targetIdx, droppedData);
                    ViewModel.Temp.RemoveAt(remIdx);
                }
            }
        }
    }
}
