using System;
using System.Globalization;
using System.Windows.Data;

namespace KeyboardHeatmapPro.Converters;

public class BoolToTrackingTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "Stop" : "Start";
        }
        return "Start";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
