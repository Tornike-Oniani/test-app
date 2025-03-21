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

namespace UiDesktopApp2.Views.Pages
{
    /// <summary>
    /// Interaction logic for TempPage.xaml
    /// </summary>
    public partial class TempPage : Page
    {

        public TempViewModel ViewModel { get; }

        public TempPage(TempViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
