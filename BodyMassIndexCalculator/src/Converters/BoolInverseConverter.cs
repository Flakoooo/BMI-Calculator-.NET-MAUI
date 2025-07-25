using System.Globalization;

namespace BodyMassIndexCalculator.src.Converters
{
    public class BoolInverseConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) 
            => value is not null and bool boolean ? !boolean : value;

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) 
            => value;
    }
}
