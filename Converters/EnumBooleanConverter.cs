using System;
using System.Globalization;
using System.Windows.Data;

namespace KeyboardHeatmapPro.Converters;

public class EnumBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is string parameterString && value != null)
        {
            if (Enum.IsDefined(value.GetType(), value))
            {
                object parameterValue = Enum.Parse(value.GetType(), parameterString);
                return parameterValue.Equals(value);
            }
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is string parameterString && value is bool boolValue && boolValue)
        {
            return Enum.Parse(targetType, parameterString);
        }
        return Binding.DoNothing;
    }
}
