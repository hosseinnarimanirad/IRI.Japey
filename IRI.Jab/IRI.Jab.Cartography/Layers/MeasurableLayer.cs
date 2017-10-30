using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Ham.SpatialBase;
using IRI.Jab.Common.Model;

namespace IRI.Jab.Cartography.Layers
{
    public class MeasurableLayer : BaseLayer
    {
        public override LayerType Type { get => LayerType.EditableItem; protected set => throw new NotImplementedException(); }

        public override BoundingBox Extent { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        public override RenderingApproach Rendering { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }



    }
}
