using IRI.Sta.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Controls.Model.CoordinateEditor
{
    public class MultiLineStringEditorModel : CoordinateEditor
    {

        private ObservableCollection<LineStringEditorModel> _lineStrings;

        public ObservableCollection<LineStringEditorModel> LineStrings
        {
            get { return _lineStrings; }
            set
            {
                _lineStrings = value;
                RaisePropertyChanged();
            }
        }

        public MultiLineStringEditorModel()
        {
            this.LineStrings = new ObservableCollection<LineStringEditorModel>();
        }

        public MultiLineStringEditorModel(Geometry multiLineString)
        {
            if (multiLineString.Type != GeometryType.MultiLineString)
            {
                throw new NotImplementedException();
            }

            LineStrings = new ObservableCollection<LineStringEditorModel>(multiLineString.Geometries.Select(g => new LineStringEditorModel(g)));

            this.Srid = multiLineString.Srid;
        }


        public override Geometry GetGeometry()
        {
            return new Geometry(LineStrings.Select(i => i.GetGeometry()).ToArray(), GeometryType.MultiLineString, Srid);
        }
    }
}
