using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using QuickVid.Converters;

namespace QuickVid.Controls;

[TypeConverter(typeof(GridLengthCollectionConverter))]
internal sealed class GridLengthCollection : ReadOnlyCollection<GridLength>
{
    public GridLengthCollection(IList<GridLength> lengths) : base(lengths)
    {
    }
}