﻿using System.Collections.Generic;

using IRI.Jab.Common.Model;
using IRI.Jab.Common.Enums;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Persistence.DataSources;
using System.Linq;

namespace IRI.Jab.Common;

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

    public List<Geometry<Point>>? GetLines(BoundingBox boundingBox)
    {
        if (DataSource is null)
        {
            return null;
        }

        return DataSource.GetAsFeatureSet(boundingBox).Features.Select(f => f.TheGeometry).ToList(); ;
    }
}
