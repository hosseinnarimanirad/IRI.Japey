﻿using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry = IRI.Sta.Spatial.Primitives.Geometry<IRI.Sta.Common.Primitives.Point>;


namespace IRI.Jab.Controls.Model
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


        public override Geometry<Point> GetGeometry()
        {
            return new Geometry<Point>(LineStrings.Select(i => i.GetGeometry()).ToList(), GeometryType.MultiLineString, Srid);
        }
    }
}
