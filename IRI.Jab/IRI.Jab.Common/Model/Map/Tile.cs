using System.ComponentModel;

using IRI.Sta.Spatial.Model;

namespace IRI.Jab.Common.Model;

public class Tile : TileInfo, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public bool IsValid { get; set; }

    public bool IsProcessing { get; set; }

    public Tile(int row, int column, int zoomLevel) : base(row, column, zoomLevel)
    {
        this.IsValid = false;

        this.IsProcessing = false;
    }

    public static Tile Parse(TileInfo tileInfo)
    {
        return new Tile(tileInfo.RowNumber, tileInfo.ColumnNumber, tileInfo.ZoomLevel);
    }
}
