using System.Collections.Generic;
using IRI.Msh.Common.Primitives;
using IRI.Ket.Persistence.DataSources;
using IRI.Jab.Common.Model;

namespace IRI.Jab.Common
{
    public class GridLayer : BaseLayer
    {
        public override LayerType Type { get; protected set; } = LayerType.VectorLayer;

        public override BoundingBox Extent { get; protected set; }

        public override RenderingApproach Rendering { get; protected set; }

        public GridDataSource DataSource { get; set; }

        public GridLayer(GridDataSource source)
        {
            DataSource = source;
        }

        public List<Geometry<Point>> GetLines(BoundingBox boundingBox)
        {
            if (DataSource == null)
            {
                return null;
            }

            return DataSource.GetGeometries(boundingBox);
        }
    }
}
