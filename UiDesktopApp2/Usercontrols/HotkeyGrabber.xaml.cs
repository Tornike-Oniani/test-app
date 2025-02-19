using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace UiDesktopApp2.Usercontrols
{
    /// <summary>
    /// Interaction logic for HotkeyGrabber.xaml
    /// </summary>
    public partial class HotkeyGrabber : UserControl, INotifyPropertyChanged
    {
        private bool _isGrabbingHotkey;

        private static readonly DependencyProperty HotkeyProperty =
            DependencyProperty.Register("Hotkey", typeof(string), typeof(HotkeyGrabber), new PropertyMetadata(String.Empty));

        public string Hotkey 
        { 
            get
            {
                return (string)GetValue(HotkeyProperty);
            }
            set
            {
                SetValue(HotkeyProperty, value);
            }
        }

        public bool IsGrabbingHotkey
        {
            get { return _isGrabbingHotkey; }
            set { _isGrabbingHotkey = value; }
        }

        public HotkeyGrabber()
        {
            InitializeComponent();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (IsGrabbingHotkey)
            {
                e.Handled = true;
                Hotkey = e.Key.ToString();
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
