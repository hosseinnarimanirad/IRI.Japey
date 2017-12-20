using IRI.Ham.SpatialBase;
using IRI.Ham.SpatialBase.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Controls.Model.CoordinateEditor
{
    public class PointEditorModel : CoordinateEditor
    {

        private IPoint _point;

        public IPoint Point
        {
            get { return _point; }
            set
            {
                _point = value;
                RaisePropertyChanged();
            }
        }



        public PointEditorModel()
        {

        }

        public PointEditorModel(Geometry point)
        {
            if (point.Type != GeometryType.Point)
            {
                throw new NotImplementedException();
            }

            this.Point = new Point(point.Points[0].X, point.Points[0].Y);

            this.Srid = point.Srid;
        }

        public override Geometry GetGeometry()
        {
            return Geometry.CreatePointOrLineString(new IPoint[] { Point }, Srid);
        }
    }
}
