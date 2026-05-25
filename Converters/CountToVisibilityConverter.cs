using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RocoKingdomEggQuery.Converters
{
    public class CountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isInverted = parameter is string param && param.Equals("Inverted", StringComparison.OrdinalIgnoreCase);
            
            if (value is int count)
            {
                if (isInverted)
                {
                    return count > 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                return count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
