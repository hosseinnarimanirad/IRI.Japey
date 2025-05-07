using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Sta.Spatial.Primitives; using IRI.Sta.Common.Primitives;
using IRI.Jab.Common.Model;

namespace IRI.Jab.Common.Layers
{
    public class MeasurableLayer : BaseLayer
    {
        public override LayerType Type { get => LayerType.EditableItem; protected set => throw new NotImplementedException(); }

        public override BoundingBox Extent { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        public override RenderingApproach Rendering { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }



    }
}
