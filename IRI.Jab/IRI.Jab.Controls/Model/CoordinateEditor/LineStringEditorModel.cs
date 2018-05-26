using IRI.Sta.Common.Primitives;
using IRI.Sta.Common.Primitives;
using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Controls.Model.CoordinateEditor
{
    public class LineStringEditorModel : CoordinateEditor
    {
        private bool _isClosed;

        public bool IsClosed
        {
            get { return _isClosed; }
            set
            {
                _isClosed = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<IPoint> _points;

        public ObservableCollection<IPoint> Points
        {
            get { return _points; }
            set
            {
                _points = value;
                RaisePropertyChanged();
            }
        }



        public LineStringEditorModel()
        {
            this.Points = new ObservableCollection<IPoint>();
        }

        public LineStringEditorModel(Geometry lineString)
        {
            if (lineString.Type != GeometryType.LineString)
            {
                throw new NotImplementedException();
            }

            this.Points = new ObservableCollection<IPoint>(lineString.Points);

            this.Srid = lineString.Srid;
        }

        public override Geometry GetGeometry()
        {
            return Geometry.Create(_points.ToArray(), GeometryType.LineString, Srid);
        }
    }
}
