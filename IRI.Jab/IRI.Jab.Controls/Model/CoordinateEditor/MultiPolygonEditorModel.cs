using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry = IRI.Sta.Spatial.Primitives.Geometry<IRI.Sta.Common.Primitives.Point>;

namespace IRI.Jab.Controls.Model;

public class MultiPolygonEditorModel: CoordinateEditor
{
    private ObservableCollection<PolygonEditorModel> _polygons;

    public ObservableCollection<PolygonEditorModel> Polygons
    {
        get { return _polygons; }
        set
        {
            _polygons = value;
            RaisePropertyChanged();
        }
    }

    public MultiPolygonEditorModel()
    {
        this.Polygons = new ObservableCollection<PolygonEditorModel>();
    }

    public MultiPolygonEditorModel(Geometry multiPolygon)
    {
        if (multiPolygon.Type != GeometryType.MultiPolygon)
        {
            throw new NotImplementedException();
        }

        Polygons = new ObservableCollection<PolygonEditorModel>(multiPolygon.Geometries.Select(g => new PolygonEditorModel(g)));

        this.Srid = multiPolygon.Srid;
    }


    public override Geometry GetGeometry()
    {
        return new Geometry(Polygons.Select(i => i.GetGeometry()).ToList(), GeometryType.MultiPolygon, Srid);
    }
}
