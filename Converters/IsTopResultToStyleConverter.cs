using System.Globalization;
using System.Windows;
using System.Windows.Data;
using RocoKingdomEggQuery.Models;

namespace RocoKingdomEggQuery.Converters
{
    public class IsTopResultToStyleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3)
                return DependencyProperty.UnsetValue;

            if (values[0] is not CategoryModel model)
                return DependencyProperty.UnsetValue;

            if (values[1] is not Style baseStyle)
                return DependencyProperty.UnsetValue;

            if (values[2] is not Style topStyle)
                return DependencyProperty.UnsetValue;

            return model.IsTopResult ? topStyle : baseStyle;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
