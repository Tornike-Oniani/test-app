using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UiDesktopApp2.Helpers
{
    public class SubtractConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double numberProperty = (double)value;
            double subtractNumber = double.Parse((string)parameter);

            double result = numberProperty - subtractNumber;
            return result < 1 ? 0 : result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
