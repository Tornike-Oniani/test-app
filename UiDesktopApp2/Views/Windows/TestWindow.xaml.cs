using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UiDesktopApp2.DataAccess.Repositories;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.ViewModels.Windows;
using Wpf.Ui;

namespace UiDesktopApp2.Views.Windows
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindowViewModel ViewModel { get; set; }

        public TestWindow(IServiceProvider serviceProvider)
        {
            ViewModel = new TestWindowViewModel(
                serviceProvider.GetService(typeof(GlobalState)) as GlobalState,
                serviceProvider.GetService(typeof(Settings)) as Settings,
                serviceProvider.GetService(typeof(ResultRepository)) as ResultRepository,
                this
                );
            DataContext = this;
            InitializeComponent();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ViewModel.StopTest();
                this.Close();
            }
        }
    }
}
