//using IRI.Sta.Common.Abstrations;
//using IRI.Sta.Spatial.Primitives;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IRI.Jab.Common.Symbology;

//public class SingleSymbology<T> : SymbologyStrategy<T> where T : IPoint, new()
//{
//    public VisualParameters Parameters { get; }

//    public SingleSymbology(VisualParameters parameters)
//    {
//        Parameters = parameters;
//    }

//    public override VisualParameters GetParameters(Feature<T> feature)
//    {
//        return Parameters;
//    }
//}
