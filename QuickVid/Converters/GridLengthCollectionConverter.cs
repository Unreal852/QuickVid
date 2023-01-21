using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using QuickVid.Controls;

namespace QuickVid.Converters;

internal sealed class GridLengthCollectionConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string s)
            return ParseString(s);
        return base.ConvertFrom(context, culture, value);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value,
                                      Type destinationType)
    {
        if (destinationType == typeof(string) && value is GridLengthCollection collection)
            return ToString(collection);
        return base.ConvertTo(context, culture, value, destinationType);
    }

    private static string ToString(GridLengthCollection value)
    {
        var converter = new GridLengthConverter();
        return string.Join(",", value.Select(v => converter.ConvertToString(v)));
    }

    private static GridLengthCollection ParseString(string s)
    {
        var converter = new GridLengthConverter();
        var lengths = s.Split(',').Select(p =>
                (GridLength)(converter.ConvertFromString(p.Trim()) ?? throw new InvalidOperationException()));
        return new GridLengthCollection(lengths.ToArray());
    }
}