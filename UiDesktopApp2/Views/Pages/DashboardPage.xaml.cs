﻿using System.Windows.Controls;
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
    }
}
