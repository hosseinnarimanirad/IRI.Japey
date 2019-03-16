using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Jab.Common.Model;
using IRI.Jab.Common.Model.Symbology;
using IRI.Ket.DataManagement.DataSource.MemorySources;
using IRI.Ket.DataManagement.Model;
using IRI.Msh.Common.Primitives;
using Microsoft.SqlServer.Types;

namespace IRI.Jab.Common
{
    public class GridLayer : BaseLayer
    {
        public override LayerType Type { get; protected set; }

        public override BoundingBox Extent { get; protected set; }

        public override RenderingApproach Rendering { get; protected set; }

        public GridDataSource DataSource { get; set; }

        public GridLayer(GridDataSource source)
        {
            DataSource = source;
        }

        public List<SqlGeometry> GetLines(BoundingBox boundingBox)
        {
            if (DataSource == null)
            {
                return null;
            }

            return DataSource.GetGeometries(boundingBox);
        }
    }
}
