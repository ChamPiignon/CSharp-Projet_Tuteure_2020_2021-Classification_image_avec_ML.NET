using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace TFModelScorerWPF
{
    class percentageConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fraction = decimal.Parse(value.ToString());
            return fraction.ToString("P3");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueWithoutPercentage = value.ToString().TrimEnd(' ', '%');
            return decimal.Parse(valueWithoutPercentage) / 100;
        }
    }
}
