using System.Windows.Controls;

namespace QuickVid.Controls;

internal sealed class SimpleGrid : Grid
{
    private GridLengthCollection? _rows;
    private GridLengthCollection? _columns;

    public GridLengthCollection? Rows
    {
        get => _rows;
        set
        {
            _rows = value;
            RowDefinitions.Clear();
            if (_rows == null)
                return;
            foreach (var length in _rows)
                RowDefinitions.Add(new RowDefinition { Height = length });
        }
    }

    public GridLengthCollection? Columns
    {
        get => _columns;
        set
        {
            _columns = value;
            if (_columns == null)
                return;
            ColumnDefinitions.Clear();
            foreach (var length in _columns)
                ColumnDefinitions.Add(new ColumnDefinition { Width = length });
        }
    }
}