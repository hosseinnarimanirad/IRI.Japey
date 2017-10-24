using IRI.Jab.Common;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Cartography.Model
{
    public class GeometryMarkupLabelTriple : Notifier
    {
        private SqlGeometry _geometryWm;

        public SqlGeometry GeometryWm
        {
            get { return _geometryWm; }
            set
            {
                _geometryWm = value;
                RaisePropertyChanged();
            }
        }

        private string _pathMarkup;

        public string PathMarkup
        {
            get { return _pathMarkup; }
            set
            {
                _pathMarkup = value;
                RaisePropertyChanged();
            }
        }

        private string _label;

        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                RaisePropertyChanged();
            }
        }

        public GeometryMarkupLabelTriple(string label, string markup, SqlGeometry geometryWm)
        {
            this.Label = label;

            this.PathMarkup = markup;

            this.GeometryWm = geometryWm;
        }
    }
}
