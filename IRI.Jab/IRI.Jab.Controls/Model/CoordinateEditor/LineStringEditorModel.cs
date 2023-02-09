using IRI.Msh.Common.Primitives; 
using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry = IRI.Msh.Common.Primitives.Geometry<IRI.Msh.Common.Primitives.Point>;

namespace IRI.Jab.Controls.Model
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

        private ObservableCollection<Point> _points;

        public ObservableCollection<Point> Points
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
            this.Points = new ObservableCollection<Point>();
        }

        public LineStringEditorModel(Geometry lineString)
        {
            if (lineString.Type != GeometryType.LineString)
            {
                throw new NotImplementedException();
            }

            this.Points = new ObservableCollection<Point>(lineString.Points);

            this.Srid = lineString.Srid;
        }

        public override Geometry GetGeometry()
        {
            return Geometry.Create(_points.ToList(), GeometryType.LineString, Srid);
        }
    }
}
