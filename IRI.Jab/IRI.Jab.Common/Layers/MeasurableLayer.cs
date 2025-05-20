using System;
using IRI.Jab.Common.Enums;
using IRI.Jab.Common.Model;
using IRI.Sta.Common.Primitives;

namespace IRI.Jab.Common.Layers;

public class MeasurableLayer : BaseLayer
{
    public override LayerType Type { get => LayerType.EditableItem; protected set => throw new NotImplementedException(); }

    public override BoundingBox Extent { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

    public override RenderingApproach Rendering { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
}
