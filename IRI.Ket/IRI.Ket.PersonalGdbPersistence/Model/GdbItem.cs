using DocumentFormat.OpenXml.Spreadsheet;
using IRI.Ket.PersonalGdbPersistence.Xml;
using IRI.Sta.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.PersonalGdbPersistence.Model;

public class GdbItem
{
    public string Name { get; set; }

    public string PhysicalName { get; set; }

    public string Path { get; set; }

    public string Definition { get; set; }

    public GdbXml_FeatureClass? DefinitionInfo => string.IsNullOrWhiteSpace(Definition) ?
                                                null :
                                                XmlHelper.ParseFromXml<GdbXml_FeatureClass>(Definition);

    public string? AliasName => DefinitionInfo?.AliasName;

    public string? ShapeType => DefinitionInfo?.ShapeType;

    public string? ShapeFieldName => DefinitionInfo?.ShapeFieldName;

    public bool HasM => DefinitionInfo?.HasM ?? false;

    public bool HasZ => DefinitionInfo?.HasZ ?? false;


    public override string ToString() => $"Name: {Name}, AliasName: {AliasName}, Path:{Path}, ShapeType: {ShapeType}";
}
