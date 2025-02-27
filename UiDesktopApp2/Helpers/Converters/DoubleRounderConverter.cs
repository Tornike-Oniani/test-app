using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UiDesktopApp2.Helpers
{
    public class DoubleRounderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dVal = (double)value;
            int decimals = 2;

            if (parameter != null)
            {
                decimals = int.Parse(parameter.ToString());
            }

            return Math.Round(dVal, decimals);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
