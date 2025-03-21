﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UiDesktopApp2.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.Views.Pages
{
    /// <summary>
    /// Interaction logic for ConfigurationPage.xaml
    /// </summary>
    public partial class ConfigurationPage : INavigableView<ConfigurationViewModel>
    {
        public ConfigurationViewModel ViewModel { get; }

        public ConfigurationPage(ConfigurationViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();
        }
    }
}
