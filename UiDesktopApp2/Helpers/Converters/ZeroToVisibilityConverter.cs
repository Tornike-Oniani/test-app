using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UiDesktopApp2.Helpers
{
    public class ZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int number = (int)value;
            string mode = parameter == null ? String.Empty : parameter.ToString();

            if (number == 0)
            {
                if (mode == "Inverse")
                {
                    return Visibility.Visible;
                }

                return Visibility.Hidden;
            }

            if (mode == "Inverse")
            {
                return Visibility.Hidden;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
