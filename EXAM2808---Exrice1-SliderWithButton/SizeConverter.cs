using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EXAM2808___Exrice1___SliderWithButton
{
    public class SizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double sliderValue = (double)value;
            if (sliderValue < 51)
                return "Small";
            if (sliderValue > 50 && sliderValue < 71)
                return "Medium";
            if (sliderValue > 70 && sliderValue < 90)
                return "Large";

                return "Extra Large";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
