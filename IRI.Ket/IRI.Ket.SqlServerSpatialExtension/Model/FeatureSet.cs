using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SqlServerSpatialExtension.Model
{
    public class FeatureSet
    {
        public List<Field> Fields { get; set; }

        public List<Feature> Features { get; set; }
    }
}
