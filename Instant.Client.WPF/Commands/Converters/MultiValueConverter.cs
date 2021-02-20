using System;
using System.Globalization;
using System.Windows.Data;

namespace Instant.Client.WPF.Commands.Converters
{
    public class MultiValueConverter : IMultiValueConverter
    {
        public object Convert(
            object[] values, 
            Type targetType, 
            object parameter, 
            CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(
            object value, 
            Type[] targetTypes, 
            object parameter, 
            CultureInfo culture)
        {
            var values = (object[]) value;
            return values;
        }
    }
}
