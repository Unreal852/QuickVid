using System;
using System.Globalization;
using System.Windows.Data;

namespace QuickVid.Converters;

internal sealed class TimeSpanToDoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan time)
        {
            return time.TotalSeconds;
        }

        return 0.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double time)
        {
            return TimeSpan.FromSeconds(time);
        }

        return TimeSpan.Zero;
    }
}