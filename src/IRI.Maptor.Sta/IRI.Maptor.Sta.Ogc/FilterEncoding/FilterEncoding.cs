using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace IRI.Maptor.Sta.Ogc;

#region Main Filter Classes

[XmlRoot("Filter", Namespace = SldNamespaces.FES)]
public class Filter
{
    [XmlElement("PropertyIsEqualTo", typeof(PropertyIsEqualTo))]
    [XmlElement("PropertyIsNotEqualTo", typeof(PropertyIsNotEqualTo))]
    [XmlElement("PropertyIsLessThan", typeof(PropertyIsLessThan))]
    [XmlElement("PropertyIsGreaterThan", typeof(PropertyIsGreaterThan))]
    [XmlElement("PropertyIsLessThanOrEqualTo", typeof(PropertyIsLessThanOrEqualTo))]
    [XmlElement("PropertyIsGreaterThanOrEqualTo", typeof(PropertyIsGreaterThanOrEqualTo))]
    [XmlElement("PropertyIsLike", typeof(PropertyIsLike))]
    [XmlElement("PropertyIsNull", typeof(PropertyIsNull))]
    [XmlElement("PropertyIsNil", typeof(PropertyIsNil))]
    [XmlElement("PropertyIsBetween", typeof(PropertyIsBetween))]
    [XmlElement("And", typeof(And))]
    [XmlElement("Or", typeof(Or))]
    [XmlElement("Not", typeof(Not))]
    [XmlElement("Equals", typeof(EqualsSpatialy))]
    [XmlElement("Disjoint", typeof(Disjoint))]
    [XmlElement("Touches", typeof(Touches))]
    [XmlElement("Within", typeof(Within))]
    [XmlElement("Overlaps", typeof(Overlaps))]
    [XmlElement("Crosses", typeof(Crosses))]
    [XmlElement("Intersects", typeof(Intersects))]
    [XmlElement("Contains", typeof(Contains))]
    [XmlElement("DWithin", typeof(DWithin))]
    [XmlElement("Beyond", typeof(Beyond))]
    [XmlElement("BBOX", typeof(BBOX))]
    [XmlElement("After", typeof(After))]
    [XmlElement("Before", typeof(Before))]
    [XmlElement("Begins", typeof(Begins))]
    [XmlElement("BegunBy", typeof(BegunBy))]
    [XmlElement("TContains", typeof(TContains))]
    [XmlElement("During", typeof(During))]
    [XmlElement("EndedBy", typeof(EndedBy))]
    [XmlElement("Ends", typeof(Ends))]
    [XmlElement("TEquals", typeof(TEquals))]
    [XmlElement("Meets", typeof(Meets))]
    [XmlElement("MetBy", typeof(MetBy))]
    [XmlElement("TOverlaps", typeof(TOverlaps))]
    [XmlElement("OverlappedBy", typeof(OverlappedBy))]
    [XmlElement("AnyInteracts", typeof(AnyInteracts))]
    [XmlElement("ResourceId", typeof(ResourceId))]
    [XmlElement("Function", typeof(Function))]
    public IFilter Predicate { get; set; }


    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Xmlns { get; set; }

    public Filter()
    {
        Xmlns = new XmlSerializerNamespaces();
        Xmlns.Add("fes", SldNamespaces.FES);
        Xmlns.Add("ogc", SldNamespaces.OGC);
    }
}

public interface IFilter { }

#endregion

#region Comparison Operators

public abstract class ComparisonOperator : IFilter { }

[XmlType("PropertyIsEqualTo", Namespace = SldNamespaces.FES)]
public class PropertyIsEqualTo : ComparisonOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();

    [XmlAttribute("matchCase")]
    public bool MatchCase { get; set; } = true;

    [XmlAttribute("matchAction")]
    public MatchActionType MatchAction { get; set; } = MatchActionType.Any;
}

[XmlType("PropertyIsNotEqualTo", Namespace = SldNamespaces.FES)]
public class PropertyIsNotEqualTo : ComparisonOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();

    [XmlAttribute("matchCase")]
    public bool MatchCase { get; set; } = true;

    [XmlAttribute("matchAction")]
    public MatchActionType MatchAction { get; set; } = MatchActionType.Any;
}

[XmlType("PropertyIsLessThan", Namespace = SldNamespaces.FES)]
public class PropertyIsLessThan : ComparisonOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("PropertyIsGreaterThan", Namespace = SldNamespaces.FES)]
public class PropertyIsGreaterThan : ComparisonOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("PropertyIsLessThanOrEqualTo", Namespace = SldNamespaces.FES)]
public class PropertyIsLessThanOrEqualTo : ComparisonOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("PropertyIsGreaterThanOrEqualTo", Namespace = SldNamespaces.FES)]
public class PropertyIsGreaterThanOrEqualTo : ComparisonOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("PropertyIsLike", Namespace = SldNamespaces.FES)]
public class PropertyIsLike : ComparisonOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();

    [XmlAttribute("wildCard")]
    public string WildCard { get; set; }

    [XmlAttribute("singleChar")]
    public string SingleChar { get; set; }

    [XmlAttribute("escapeChar")]
    public string EscapeChar { get; set; }

    [XmlAttribute("matchCase")]
    public bool MatchCase { get; set; } = true;
}

[XmlType("PropertyIsNull", Namespace = SldNamespaces.FES)]
public class PropertyIsNull : ComparisonOperator
{
    [XmlElement("expression")]
    public Expression Expression { get; set; }
}

[XmlType("PropertyIsNil", Namespace = SldNamespaces.FES)]
public class PropertyIsNil : ComparisonOperator
{
    [XmlElement("expression")]
    public Expression Expression { get; set; }

    [XmlAttribute("nilReason")]
    public string NilReason { get; set; }
}

[XmlType("PropertyIsBetween", Namespace = SldNamespaces.FES)]
public class PropertyIsBetween : ComparisonOperator
{
    [XmlElement("expression")]
    public Expression Expression { get; set; }

    [XmlElement("LowerBoundary")]
    public LowerBoundary LowerBoundary { get; set; }

    [XmlElement("UpperBoundary")]
    public UpperBoundary UpperBoundary { get; set; }
}

public class LowerBoundary
{
    [XmlElement("expression")]
    public Expression Expression { get; set; }
}

public class UpperBoundary
{
    [XmlElement("expression")]
    public Expression Expression { get; set; }
}



#endregion

#region Logical Operators

public abstract class LogicalOperator : IFilter { }

[XmlType("And", Namespace = SldNamespaces.FES)]
public class And : LogicalOperator
{
    [XmlElement("PropertyIsEqualTo", typeof(PropertyIsEqualTo))]
    [XmlElement("PropertyIsNotEqualTo", typeof(PropertyIsNotEqualTo))]
    [XmlElement("PropertyIsLessThan", typeof(PropertyIsLessThan))]
    [XmlElement("PropertyIsGreaterThan", typeof(PropertyIsGreaterThan))]
    [XmlElement("PropertyIsLessThanOrEqualTo", typeof(PropertyIsLessThanOrEqualTo))]
    [XmlElement("PropertyIsGreaterThanOrEqualTo", typeof(PropertyIsGreaterThanOrEqualTo))]
    [XmlElement("PropertyIsLike", typeof(PropertyIsLike))]
    [XmlElement("PropertyIsNull", typeof(PropertyIsNull))]
    [XmlElement("PropertyIsNil", typeof(PropertyIsNil))]
    [XmlElement("PropertyIsBetween", typeof(PropertyIsBetween))]
    [XmlElement("And", typeof(And))]
    [XmlElement("Or", typeof(Or))]
    [XmlElement("Not", typeof(Not))]
    [XmlElement("Equals", typeof(EqualsSpatialy))]
    [XmlElement("Disjoint", typeof(Disjoint))]
    [XmlElement("Touches", typeof(Touches))]
    [XmlElement("Within", typeof(Within))]
    [XmlElement("Overlaps", typeof(Overlaps))]
    [XmlElement("Crosses", typeof(Crosses))]
    [XmlElement("Intersects", typeof(Intersects))]
    [XmlElement("Contains", typeof(Contains))]
    [XmlElement("DWithin", typeof(DWithin))]
    [XmlElement("Beyond", typeof(Beyond))]
    [XmlElement("BBOX", typeof(BBOX))]
    public List<object> Predicates { get; set; } = new();

}

[XmlType("Or", Namespace = SldNamespaces.FES)]
public class Or : LogicalOperator
{
    [XmlElement("PropertyIsEqualTo", typeof(PropertyIsEqualTo))]
    [XmlElement("PropertyIsNotEqualTo", typeof(PropertyIsNotEqualTo))]
    [XmlElement("PropertyIsLessThan", typeof(PropertyIsLessThan))]
    [XmlElement("PropertyIsGreaterThan", typeof(PropertyIsGreaterThan))]
    [XmlElement("PropertyIsLessThanOrEqualTo", typeof(PropertyIsLessThanOrEqualTo))]
    [XmlElement("PropertyIsGreaterThanOrEqualTo", typeof(PropertyIsGreaterThanOrEqualTo))]
    [XmlElement("PropertyIsLike", typeof(PropertyIsLike))]
    [XmlElement("PropertyIsNull", typeof(PropertyIsNull))]
    [XmlElement("PropertyIsNil", typeof(PropertyIsNil))]
    [XmlElement("PropertyIsBetween", typeof(PropertyIsBetween))]
    [XmlElement("And", typeof(And))]
    [XmlElement("Or", typeof(Or))]
    [XmlElement("Not", typeof(Not))]
    [XmlElement("Equals", typeof(EqualsSpatialy))]
    [XmlElement("Disjoint", typeof(Disjoint))]
    [XmlElement("Touches", typeof(Touches))]
    [XmlElement("Within", typeof(Within))]
    [XmlElement("Overlaps", typeof(Overlaps))]
    [XmlElement("Crosses", typeof(Crosses))]
    [XmlElement("Intersects", typeof(Intersects))]
    [XmlElement("Contains", typeof(Contains))]
    [XmlElement("DWithin", typeof(DWithin))]
    [XmlElement("Beyond", typeof(Beyond))]
    [XmlElement("BBOX", typeof(BBOX))]
    public List<object> Predicates { get; set; } = new();
}

[XmlType("Not", Namespace = SldNamespaces.FES)]
public class Not : LogicalOperator
{
    [XmlElement("PropertyIsEqualTo", typeof(PropertyIsEqualTo))]
    [XmlElement("PropertyIsNotEqualTo", typeof(PropertyIsNotEqualTo))]
    [XmlElement("PropertyIsLessThan", typeof(PropertyIsLessThan))]
    [XmlElement("PropertyIsGreaterThan", typeof(PropertyIsGreaterThan))]
    [XmlElement("PropertyIsLessThanOrEqualTo", typeof(PropertyIsLessThanOrEqualTo))]
    [XmlElement("PropertyIsGreaterThanOrEqualTo", typeof(PropertyIsGreaterThanOrEqualTo))]
    [XmlElement("PropertyIsLike", typeof(PropertyIsLike))]
    [XmlElement("PropertyIsNull", typeof(PropertyIsNull))]
    [XmlElement("PropertyIsNil", typeof(PropertyIsNil))]
    [XmlElement("PropertyIsBetween", typeof(PropertyIsBetween))]
    [XmlElement("And", typeof(And))]
    [XmlElement("Or", typeof(Or))]
    [XmlElement("Not", typeof(Not))]
    [XmlElement("Equals", typeof(EqualsSpatialy))]
    [XmlElement("Disjoint", typeof(Disjoint))]
    [XmlElement("Touches", typeof(Touches))]
    [XmlElement("Within", typeof(Within))]
    [XmlElement("Overlaps", typeof(Overlaps))]
    [XmlElement("Crosses", typeof(Crosses))]
    [XmlElement("Intersects", typeof(Intersects))]
    [XmlElement("Contains", typeof(Contains))]
    [XmlElement("DWithin", typeof(DWithin))]
    [XmlElement("Beyond", typeof(Beyond))]
    [XmlElement("BBOX", typeof(BBOX))]
    public object Predicate { get; set; }
}

#endregion

#region Spatial Operators

public abstract class SpatialOperator : IFilter { }

[XmlType("Equals", Namespace = SldNamespaces.FES)]
public class EqualsSpatialy : SpatialOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("Disjoint", Namespace = SldNamespaces.FES)]
public class Disjoint : SpatialOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("Touches", Namespace = SldNamespaces.FES)]
public class Touches : SpatialOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("Within", Namespace = SldNamespaces.FES)]
public class Within : SpatialOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("Overlaps", Namespace = SldNamespaces.FES)]
public class Overlaps : SpatialOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("Crosses", Namespace = SldNamespaces.FES)]
public class Crosses : SpatialOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("Intersects", Namespace = SldNamespaces.FES)]
public class Intersects : SpatialOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("Contains", Namespace = SldNamespaces.FES)]
public class Contains : SpatialOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("DWithin", Namespace = SldNamespaces.FES)]
public class DWithin : SpatialOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();

    [XmlElement("Distance")]
    public Measure Distance { get; set; }
}

[XmlType("Beyond", Namespace = SldNamespaces.FES)]
public class Beyond : SpatialOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();

    [XmlElement("Distance")]
    public Measure Distance { get; set; }
}

[XmlType("BBOX", Namespace = SldNamespaces.FES)]
public class BBOX : SpatialOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

public class Measure
{
    [XmlText]
    public double Value { get; set; }

    [XmlAttribute("uom")]
    public string Uom { get; set; }
}

#endregion

#region Temporal Operators

public abstract class TemporalOperator : IFilter { }

[XmlType("After", Namespace = SldNamespaces.FES)]
public class After : TemporalOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("Before", Namespace = SldNamespaces.FES)]
public class Before : TemporalOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("Begins", Namespace = SldNamespaces.FES)]
public class Begins : TemporalOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("BegunBy", Namespace = SldNamespaces.FES)]
public class BegunBy : TemporalOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("TContains", Namespace = SldNamespaces.FES)]
public class TContains : TemporalOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("During", Namespace = SldNamespaces.FES)]
public class During : TemporalOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("EndedBy", Namespace = SldNamespaces.FES)]
public class EndedBy : TemporalOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("Ends", Namespace = SldNamespaces.FES)]
public class Ends : TemporalOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("TEquals", Namespace = SldNamespaces.FES)]
public class TEquals : TemporalOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("Meets", Namespace = SldNamespaces.FES)]
public class Meets : TemporalOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("MetBy", Namespace = SldNamespaces.FES)]
public class MetBy : TemporalOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("TOverlaps", Namespace = SldNamespaces.FES)]
public class TOverlaps : TemporalOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("OverlappedBy", Namespace = SldNamespaces.FES)]
public class OverlappedBy : TemporalOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("AnyInteracts", Namespace = SldNamespaces.FES)]
public class AnyInteracts : TemporalOperator
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

#endregion

#region Expressions and Identifiers

[XmlInclude(typeof(Literal))]
[XmlInclude(typeof(PropertyName))]
[XmlInclude(typeof(Function))]
[XmlInclude(typeof(Add))]
[XmlInclude(typeof(Sub))]
[XmlInclude(typeof(Mul))]
[XmlInclude(typeof(Div))]
public abstract class Expression : IFilter { }

public class Literal : Expression
{
    [XmlText]
    public string Value { get; set; }
}

public class PropertyName : Expression
{
    [XmlText]
    public string Value { get; set; }
}

[XmlType("Function", Namespace = SldNamespaces.OGC)]
public class Function : Expression
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlElement("expression")]
    public List<Expression> Arguments { get; set; }
}

[XmlType("Add", Namespace = SldNamespaces.OGC)]
public class Add : Expression
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("Sub", Namespace = SldNamespaces.OGC)]
public class Sub : Expression
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("Mul", Namespace = SldNamespaces.OGC)]
public class Mul : Expression
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("Div", Namespace = SldNamespaces.OGC)]
public class Div : Expression
{
    [XmlElement("expression")]
    public List<Expression> Expressions { get; set; } = new List<Expression>();
}

[XmlType("ResourceId", Namespace = SldNamespaces.FES)]
public class ResourceId
{
    [XmlAttribute("rid")]
    public string Rid { get; set; }

    [XmlAttribute("previousRid")]
    public string PreviousRid { get; set; }

    [XmlAttribute("version")]
    public string Version { get; set; }

    [XmlAttribute("startDate")]
    public DateTime StartDate { get; set; }

    [XmlAttribute("endDate")]
    public DateTime EndDate { get; set; }

    [XmlIgnore]
    public bool StartDateSpecified { get; set; }

    [XmlIgnore]
    public bool EndDateSpecified { get; set; }
}

#endregion

#region Query Classes

[XmlType("AbstractQueryExpression", Namespace = SldNamespaces.FES)]
public abstract class AbstractQueryExpression 
{
    [XmlAttribute("handle")]
    public string Handle { get; set; }
}

[XmlType("AbstractAdhocQueryExpression", Namespace = SldNamespaces.FES)]
public abstract class AbstractAdhocQueryExpression : AbstractQueryExpression
{
    [XmlElement("AbstractProjectionClause")]
    public List<object> ProjectionClauses { get; set; } = new();

    [XmlElement("AbstractSelectionClause")]
    public object SelectionClause { get; set; }

    [XmlElement("AbstractSortingClause")]
    public object SortingClause { get; set; }

    [XmlAttribute("typeNames")]
    public string TypeNames { get; set; }

    [XmlAttribute("aliases")]
    public string Aliases { get; set; }
}

#endregion

#region Filter Capabilities

[XmlRoot("Filter_Capabilities", Namespace = SldNamespaces.OGC)]
public class FilterCapabilities
{
    [XmlElement("Spatial_Capabilities")]
    public SpatialCapabilitiesType SpatialCapabilities { get; set; }

    [XmlElement("Scalar_Capabilities")]
    public ScalarCapabilitiesType ScalarCapabilities { get; set; }

    [XmlElement("Id_Capabilities")]
    public IdCapabilitiesType IdCapabilities { get; set; }
}

public class SpatialCapabilitiesType
{
    [XmlElement("GeometryOperands")]
    public GeometryOperandsType GeometryOperands { get; set; }

    [XmlElement("SpatialOperators")]
    public SpatialOperatorsType SpatialOperators { get; set; }
}

public class GeometryOperandsType
{
    [XmlElement("GeometryOperand")]
    public List<GeometryOperandType> GeometryOperands { get; set; } = new();
}

public class SpatialOperatorsType
{
    [XmlElement("SpatialOperator")]
    public List<SpatialOperatorType> SpatialOperators { get; set; } = new();
}

public class SpatialOperatorType
{
    [XmlElement("GeometryOperands")]
    public GeometryOperandsType GeometryOperands { get; set; }

    [XmlAttribute("name")]
    public SpatialOperatorNameType Name { get; set; }
}

public class ScalarCapabilitiesType
{
    [XmlElement("LogicalOperators")]
    public LogicalOperators LogicalOperators { get; set; }

    [XmlElement("ComparisonOperators")]
    public ComparisonOperatorsType ComparisonOperators { get; set; }

    [XmlElement("ArithmeticOperators")]
    public ArithmeticOperatorsType ArithmeticOperators { get; set; }
}

public class ComparisonOperatorsType
{
    [XmlElement("ComparisonOperator")]
    public List<ComparisonOperatorType> ComparisonOperators { get; set; } = new();
}

public class ArithmeticOperatorsType
{
    [XmlElement("SimpleArithmetic")]
    public SimpleArithmetic SimpleArithmetic { get; set; }

    [XmlElement("Functions")]
    public FunctionsType Functions { get; set; }
}

public class FunctionsType
{
    [XmlElement("FunctionNames")]
    public FunctionNamesType FunctionNames { get; set; }
}

public class FunctionNamesType
{
    [XmlElement("FunctionName")]
    public List<FunctionNameType> FunctionNames { get; set; } = new();
}

public class FunctionNameType
{
    [XmlText]
    public string Value { get; set; }

    [XmlAttribute("nArgs")]
    public string NArgs { get; set; }
}

public class IdCapabilitiesType
{
    [XmlElement("EID")]
    public List<EID> EIDs { get; set; } = new();

    [XmlElement("FID")]
    public List<FID> FIDs { get; set; } = new();
}

public class LogicalOperators { }
public class SimpleArithmetic { }
public class EID { }
public class FID { }


#endregion





//#region Serialization Helper

//public static class FilterSerializer
//{
//    public static string Serialize(Filter filter)
//    {
//        var serializer = new XmlSerializer(typeof(Filter));
//        var namespaces = new XmlSerializerNamespaces();
//        namespaces.Add("fes", SldNamespaces.FES);
//        namespaces.Add("ogc", SldNamespaces.OGC);

//        using (var writer = new System.IO.StringWriter())
//        {
//            serializer.Serialize(writer, filter, namespaces);
//            return writer.ToString();
//        }
//    }

//    public static Filter Deserialize(string xml)
//    {
//        var serializer = new XmlSerializer(typeof(Filter));
//        using (var reader = new System.IO.StringReader(xml))
//        {
//            return (Filter)serializer.Deserialize(reader);
//        }
//    }

//    public static string SerializeCapabilities(FilterCapabilities capabilities)
//    {
//        var serializer = new XmlSerializer(typeof(FilterCapabilities));
//        var namespaces = new XmlSerializerNamespaces();
//        namespaces.Add("ogc", SldNamespaces.OGC);

//        using (var writer = new System.IO.StringWriter())
//        {
//            serializer.Serialize(writer, capabilities, namespaces);
//            return writer.ToString();
//        }
//    }

//    public static FilterCapabilities DeserializeCapabilities(string xml)
//    {
//        var serializer = new XmlSerializer(typeof(FilterCapabilities));
//        using (var reader = new System.IO.StringReader(xml))
//        {
//            return (FilterCapabilities)serializer.Deserialize(reader);
//        }
//    }
//}

//#endregion

//#region Usage Examples

//public static class FilterExamples
//{
//    public static Filter CreateSimplePropertyFilter(string propertyName, string value)
//    {
//        return new Filter
//        {
//            Predicate = new PropertyIsEqualTo
//            {
//                Expressions = new List<Expression>
//                {
//                        new PropertyName { Value = propertyName },
//                        new Literal { Value = value }
//                }
//            }
//        };
//    }



//    public static Filter CreateLogicalAndFilter(params object[] predicates)
//    {
//        return new Filter
//        {
//            Predicate = new And { Predicates = predicates }
//        };
//    }

//    public static Filter CreateLogicalOrFilter(params object[] predicates)
//    {
//        return new Filter
//        {
//            Predicate = new Or { Predicates = predicates }
//        };
//    }
//}

//#endregion
