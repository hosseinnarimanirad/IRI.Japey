using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IRI.Maptor.Ket.KmlFormat.Primitives;

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class SubPremiseType
{

    private AddressLine[] addressLineField;

    private SubPremiseTypeSubPremiseName[] subPremiseNameField;

    private object[] itemsField;

    private SubPremiseTypeSubPremiseNumberPrefix[] subPremiseNumberPrefixField;

    private SubPremiseTypeSubPremiseNumberSuffix[] subPremiseNumberSuffixField;

    private BuildingNameType[] buildingNameField;

    private FirmType firmField;

    private MailStopType mailStopField;

    private PostalCode postalCodeField;

    private SubPremiseType subPremiseField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("SubPremiseName")]
    public SubPremiseTypeSubPremiseName[] SubPremiseName
    {
        get
        {
            return this.subPremiseNameField;
        }
        set
        {
            this.subPremiseNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("SubPremiseLocation", typeof(SubPremiseTypeSubPremiseLocation))]
    [System.Xml.Serialization.XmlElementAttribute("SubPremiseNumber", typeof(SubPremiseTypeSubPremiseNumber))]
    public object[] Items
    {
        get
        {
            return this.itemsField;
        }
        set
        {
            this.itemsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("SubPremiseNumberPrefix")]
    public SubPremiseTypeSubPremiseNumberPrefix[] SubPremiseNumberPrefix
    {
        get
        {
            return this.subPremiseNumberPrefixField;
        }
        set
        {
            this.subPremiseNumberPrefixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("SubPremiseNumberSuffix")]
    public SubPremiseTypeSubPremiseNumberSuffix[] SubPremiseNumberSuffix
    {
        get
        {
            return this.subPremiseNumberSuffixField;
        }
        set
        {
            this.subPremiseNumberSuffixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("BuildingName")]
    public BuildingNameType[] BuildingName
    {
        get
        {
            return this.buildingNameField;
        }
        set
        {
            this.buildingNameField = value;
        }
    }

    /// <remarks/>
    public FirmType Firm
    {
        get
        {
            return this.firmField;
        }
        set
        {
            this.firmField = value;
        }
    }

    /// <remarks/>
    public MailStopType MailStop
    {
        get
        {
            return this.mailStopField;
        }
        set
        {
            this.mailStopField = value;
        }
    }

    /// <remarks/>
    public PostalCode PostalCode
    {
        get
        {
            return this.postalCodeField;
        }
        set
        {
            this.postalCodeField = value;
        }
    }

    /// <remarks/>
    public SubPremiseType SubPremise
    {
        get
        {
            return this.subPremiseField;
        }
        set
        {
            this.subPremiseField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class AddressLine
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class SubPremiseTypeSubPremiseName
{

    private string typeField;

    private SubPremiseTypeSubPremiseNameTypeOccurrence typeOccurrenceField;

    private bool typeOccurrenceFieldSpecified;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public SubPremiseTypeSubPremiseNameTypeOccurrence TypeOccurrence
    {
        get
        {
            return this.typeOccurrenceField;
        }
        set
        {
            this.typeOccurrenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool TypeOccurrenceSpecified
    {
        get
        {
            return this.typeOccurrenceFieldSpecified;
        }
        set
        {
            this.typeOccurrenceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum SubPremiseTypeSubPremiseNameTypeOccurrence
{

    /// <remarks/>
    Before,

    /// <remarks/>
    After,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class SubPremiseTypeSubPremiseLocation
{

    private string codeField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class SubPremiseTypeSubPremiseNumber
{

    private string indicatorField;

    private SubPremiseTypeSubPremiseNumberIndicatorOccurrence indicatorOccurrenceField;

    private bool indicatorOccurrenceFieldSpecified;

    private SubPremiseTypeSubPremiseNumberNumberTypeOccurrence numberTypeOccurrenceField;

    private bool numberTypeOccurrenceFieldSpecified;

    private string premiseNumberSeparatorField;

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Indicator
    {
        get
        {
            return this.indicatorField;
        }
        set
        {
            this.indicatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public SubPremiseTypeSubPremiseNumberIndicatorOccurrence IndicatorOccurrence
    {
        get
        {
            return this.indicatorOccurrenceField;
        }
        set
        {
            this.indicatorOccurrenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool IndicatorOccurrenceSpecified
    {
        get
        {
            return this.indicatorOccurrenceFieldSpecified;
        }
        set
        {
            this.indicatorOccurrenceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public SubPremiseTypeSubPremiseNumberNumberTypeOccurrence NumberTypeOccurrence
    {
        get
        {
            return this.numberTypeOccurrenceField;
        }
        set
        {
            this.numberTypeOccurrenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool NumberTypeOccurrenceSpecified
    {
        get
        {
            return this.numberTypeOccurrenceFieldSpecified;
        }
        set
        {
            this.numberTypeOccurrenceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string PremiseNumberSeparator
    {
        get
        {
            return this.premiseNumberSeparatorField;
        }
        set
        {
            this.premiseNumberSeparatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum SubPremiseTypeSubPremiseNumberIndicatorOccurrence
{

    /// <remarks/>
    Before,

    /// <remarks/>
    After,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum SubPremiseTypeSubPremiseNumberNumberTypeOccurrence
{

    /// <remarks/>
    Before,

    /// <remarks/>
    After,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class SubPremiseTypeSubPremiseNumberPrefix
{

    private string numberPrefixSeparatorField;

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NumberPrefixSeparator
    {
        get
        {
            return this.numberPrefixSeparatorField;
        }
        set
        {
            this.numberPrefixSeparatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class SubPremiseTypeSubPremiseNumberSuffix
{

    private string numberSuffixSeparatorField;

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NumberSuffixSeparator
    {
        get
        {
            return this.numberSuffixSeparatorField;
        }
        set
        {
            this.numberSuffixSeparatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class BuildingNameType
{

    private string typeField;

    private BuildingNameTypeTypeOccurrence typeOccurrenceField;

    private bool typeOccurrenceFieldSpecified;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public BuildingNameTypeTypeOccurrence TypeOccurrence
    {
        get
        {
            return this.typeOccurrenceField;
        }
        set
        {
            this.typeOccurrenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool TypeOccurrenceSpecified
    {
        get
        {
            return this.typeOccurrenceFieldSpecified;
        }
        set
        {
            this.typeOccurrenceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum BuildingNameTypeTypeOccurrence
{

    /// <remarks/>
    Before,

    /// <remarks/>
    After,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class FirmType
{

    private AddressLine[] addressLineField;

    private FirmTypeFirmName[] firmNameField;

    private Department[] departmentField;

    private MailStopType mailStopField;

    private PostalCode postalCodeField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("FirmName")]
    public FirmTypeFirmName[] FirmName
    {
        get
        {
            return this.firmNameField;
        }
        set
        {
            this.firmNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Department")]
    public Department[] Department
    {
        get
        {
            return this.departmentField;
        }
        set
        {
            this.departmentField = value;
        }
    }

    /// <remarks/>
    public MailStopType MailStop
    {
        get
        {
            return this.mailStopField;
        }
        set
        {
            this.mailStopField = value;
        }
    }

    /// <remarks/>
    public PostalCode PostalCode
    {
        get
        {
            return this.postalCodeField;
        }
        set
        {
            this.postalCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class FirmTypeFirmName
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class Department
{

    private AddressLine[] addressLineField;

    private DepartmentDepartmentName[] departmentNameField;

    private MailStopType mailStopField;

    private PostalCode postalCodeField;

    private System.Xml.Linq.XElement[] anyField;
    
    private string typeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("DepartmentName")]
    public DepartmentDepartmentName[] DepartmentName
    {
        get
        {
            return this.departmentNameField;
        }
        set
        {
            this.departmentNameField = value;
        }
    }

    /// <remarks/>
    public MailStopType MailStop
    {
        get
        {
            return this.mailStopField;
        }
        set
        {
            this.mailStopField = value;
        }
    }

    /// <remarks/>
    public PostalCode PostalCode
    {
        get
        {
            return this.postalCodeField;
        }
        set
        {
            this.postalCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class DepartmentDepartmentName
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class MailStopType
{

    private AddressLine[] addressLineField;

    private MailStopTypeMailStopName mailStopNameField;

    private MailStopTypeMailStopNumber mailStopNumberField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    public MailStopTypeMailStopName MailStopName
    {
        get
        {
            return this.mailStopNameField;
        }
        set
        {
            this.mailStopNameField = value;
        }
    }

    /// <remarks/>
    public MailStopTypeMailStopNumber MailStopNumber
    {
        get
        {
            return this.mailStopNumberField;
        }
        set
        {
            this.mailStopNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class MailStopTypeMailStopName
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class MailStopTypeMailStopNumber
{

    private string nameNumberSeparatorField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NameNumberSeparator
    {
        get
        {
            return this.nameNumberSeparatorField;
        }
        set
        {
            this.nameNumberSeparatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class PostalCode
{

    private AddressLine[] addressLineField;

    private PostalCodePostalCodeNumber[] postalCodeNumberField;

    private PostalCodePostalCodeNumberExtension[] postalCodeNumberExtensionField;

    private PostalCodePostTown postTownField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PostalCodeNumber")]
    public PostalCodePostalCodeNumber[] PostalCodeNumber
    {
        get
        {
            return this.postalCodeNumberField;
        }
        set
        {
            this.postalCodeNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PostalCodeNumberExtension")]
    public PostalCodePostalCodeNumberExtension[] PostalCodeNumberExtension
    {
        get
        {
            return this.postalCodeNumberExtensionField;
        }
        set
        {
            this.postalCodeNumberExtensionField = value;
        }
    }

    /// <remarks/>
    public PostalCodePostTown PostTown
    {
        get
        {
            return this.postTownField;
        }
        set
        {
            this.postTownField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PostalCodePostalCodeNumber
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PostalCodePostalCodeNumberExtension
{

    private string typeField;

    private string numberExtensionSeparatorField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NumberExtensionSeparator
    {
        get
        {
            return this.numberExtensionSeparatorField;
        }
        set
        {
            this.numberExtensionSeparatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PostalCodePostTown
{

    private AddressLine[] addressLineField;

    private PostalCodePostTownPostTownName[] postTownNameField;

    private PostalCodePostTownPostTownSuffix postTownSuffixField;

    private string typeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PostTownName")]
    public PostalCodePostTownPostTownName[] PostTownName
    {
        get
        {
            return this.postTownNameField;
        }
        set
        {
            this.postTownNameField = value;
        }
    }

    /// <remarks/>
    public PostalCodePostTownPostTownSuffix PostTownSuffix
    {
        get
        {
            return this.postTownSuffixField;
        }
        set
        {
            this.postTownSuffixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PostalCodePostTownPostTownName
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PostalCodePostTownPostTownSuffix
{

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PostalRouteType
{

    private AddressLine[] addressLineField;

    private object[] itemsField;

    private PostBox postBoxField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PostalRouteName", typeof(PostalRouteTypePostalRouteName))]
    [System.Xml.Serialization.XmlElementAttribute("PostalRouteNumber", typeof(PostalRouteTypePostalRouteNumber))]
    public object[] Items
    {
        get
        {
            return this.itemsField;
        }
        set
        {
            this.itemsField = value;
        }
    }

    /// <remarks/>
    public PostBox PostBox
    {
        get
        {
            return this.postBoxField;
        }
        set
        {
            this.postBoxField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PostalRouteTypePostalRouteName
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PostalRouteTypePostalRouteNumber
{

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class PostBox
{

    private AddressLine[] addressLineField;

    private PostBoxPostBoxNumber postBoxNumberField;

    private PostBoxPostBoxNumberPrefix postBoxNumberPrefixField;

    private PostBoxPostBoxNumberSuffix postBoxNumberSuffixField;

    private PostBoxPostBoxNumberExtension postBoxNumberExtensionField;

    private FirmType firmField;

    private PostalCode postalCodeField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private string indicatorField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    public PostBoxPostBoxNumber PostBoxNumber
    {
        get
        {
            return this.postBoxNumberField;
        }
        set
        {
            this.postBoxNumberField = value;
        }
    }

    /// <remarks/>
    public PostBoxPostBoxNumberPrefix PostBoxNumberPrefix
    {
        get
        {
            return this.postBoxNumberPrefixField;
        }
        set
        {
            this.postBoxNumberPrefixField = value;
        }
    }

    /// <remarks/>
    public PostBoxPostBoxNumberSuffix PostBoxNumberSuffix
    {
        get
        {
            return this.postBoxNumberSuffixField;
        }
        set
        {
            this.postBoxNumberSuffixField = value;
        }
    }

    /// <remarks/>
    public PostBoxPostBoxNumberExtension PostBoxNumberExtension
    {
        get
        {
            return this.postBoxNumberExtensionField;
        }
        set
        {
            this.postBoxNumberExtensionField = value;
        }
    }

    /// <remarks/>
    public FirmType Firm
    {
        get
        {
            return this.firmField;
        }
        set
        {
            this.firmField = value;
        }
    }

    /// <remarks/>
    public PostalCode PostalCode
    {
        get
        {
            return this.postalCodeField;
        }
        set
        {
            this.postalCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Indicator
    {
        get
        {
            return this.indicatorField;
        }
        set
        {
            this.indicatorField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PostBoxPostBoxNumber
{

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PostBoxPostBoxNumberPrefix
{

    private string numberPrefixSeparatorField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NumberPrefixSeparator
    {
        get
        {
            return this.numberPrefixSeparatorField;
        }
        set
        {
            this.numberPrefixSeparatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PostBoxPostBoxNumberSuffix
{

    private string numberSuffixSeparatorField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NumberSuffixSeparator
    {
        get
        {
            return this.numberSuffixSeparatorField;
        }
        set
        {
            this.numberSuffixSeparatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PostBoxPostBoxNumberExtension
{

    private string numberExtensionSeparatorField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NumberExtensionSeparator
    {
        get
        {
            return this.numberExtensionSeparatorField;
        }
        set
        {
            this.numberExtensionSeparatorField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class DependentLocalityType
{

    private AddressLine[] addressLineField;

    private DependentLocalityTypeDependentLocalityName[] dependentLocalityNameField;

    private DependentLocalityTypeDependentLocalityNumber dependentLocalityNumberField;

    private object itemField;

    private Thoroughfare thoroughfareField;

    private Premise premiseField;

    private DependentLocalityType dependentLocalityField;

    private PostalCode postalCodeField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private string usageTypeField;

    private string connectorField;

    private string indicatorField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("DependentLocalityName")]
    public DependentLocalityTypeDependentLocalityName[] DependentLocalityName
    {
        get
        {
            return this.dependentLocalityNameField;
        }
        set
        {
            this.dependentLocalityNameField = value;
        }
    }

    /// <remarks/>
    public DependentLocalityTypeDependentLocalityNumber DependentLocalityNumber
    {
        get
        {
            return this.dependentLocalityNumberField;
        }
        set
        {
            this.dependentLocalityNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("LargeMailUser", typeof(LargeMailUserType))]
    [System.Xml.Serialization.XmlElementAttribute("PostBox", typeof(PostBox))]
    [System.Xml.Serialization.XmlElementAttribute("PostOffice", typeof(PostOffice))]
    [System.Xml.Serialization.XmlElementAttribute("PostalRoute", typeof(PostalRouteType))]
    public object Item
    {
        get
        {
            return this.itemField;
        }
        set
        {
            this.itemField = value;
        }
    }

    /// <remarks/>
    public Thoroughfare Thoroughfare
    {
        get
        {
            return this.thoroughfareField;
        }
        set
        {
            this.thoroughfareField = value;
        }
    }

    /// <remarks/>
    public Premise Premise
    {
        get
        {
            return this.premiseField;
        }
        set
        {
            this.premiseField = value;
        }
    }

    /// <remarks/>
    public DependentLocalityType DependentLocality
    {
        get
        {
            return this.dependentLocalityField;
        }
        set
        {
            this.dependentLocalityField = value;
        }
    }

    /// <remarks/>
    public PostalCode PostalCode
    {
        get
        {
            return this.postalCodeField;
        }
        set
        {
            this.postalCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string UsageType
    {
        get
        {
            return this.usageTypeField;
        }
        set
        {
            this.usageTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Connector
    {
        get
        {
            return this.connectorField;
        }
        set
        {
            this.connectorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Indicator
    {
        get
        {
            return this.indicatorField;
        }
        set
        {
            this.indicatorField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class DependentLocalityTypeDependentLocalityName
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class DependentLocalityTypeDependentLocalityNumber
{

    private DependentLocalityTypeDependentLocalityNumberNameNumberOccurrence nameNumberOccurrenceField;

    private bool nameNumberOccurrenceFieldSpecified;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public DependentLocalityTypeDependentLocalityNumberNameNumberOccurrence NameNumberOccurrence
    {
        get
        {
            return this.nameNumberOccurrenceField;
        }
        set
        {
            this.nameNumberOccurrenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool NameNumberOccurrenceSpecified
    {
        get
        {
            return this.nameNumberOccurrenceFieldSpecified;
        }
        set
        {
            this.nameNumberOccurrenceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum DependentLocalityTypeDependentLocalityNumberNameNumberOccurrence
{

    /// <remarks/>
    Before,

    /// <remarks/>
    After,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class LargeMailUserType
{

    private AddressLine[] addressLineField;

    private LargeMailUserTypeLargeMailUserName[] largeMailUserNameField;

    private LargeMailUserTypeLargeMailUserIdentifier largeMailUserIdentifierField;

    private BuildingNameType[] buildingNameField;

    private Department departmentField;

    private PostBox postBoxField;

    private Thoroughfare thoroughfareField;

    private PostalCode postalCodeField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("LargeMailUserName")]
    public LargeMailUserTypeLargeMailUserName[] LargeMailUserName
    {
        get
        {
            return this.largeMailUserNameField;
        }
        set
        {
            this.largeMailUserNameField = value;
        }
    }

    /// <remarks/>
    public LargeMailUserTypeLargeMailUserIdentifier LargeMailUserIdentifier
    {
        get
        {
            return this.largeMailUserIdentifierField;
        }
        set
        {
            this.largeMailUserIdentifierField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("BuildingName")]
    public BuildingNameType[] BuildingName
    {
        get
        {
            return this.buildingNameField;
        }
        set
        {
            this.buildingNameField = value;
        }
    }

    /// <remarks/>
    public Department Department
    {
        get
        {
            return this.departmentField;
        }
        set
        {
            this.departmentField = value;
        }
    }

    /// <remarks/>
    public PostBox PostBox
    {
        get
        {
            return this.postBoxField;
        }
        set
        {
            this.postBoxField = value;
        }
    }

    /// <remarks/>
    public Thoroughfare Thoroughfare
    {
        get
        {
            return this.thoroughfareField;
        }
        set
        {
            this.thoroughfareField = value;
        }
    }

    /// <remarks/>
    public PostalCode PostalCode
    {
        get
        {
            return this.postalCodeField;
        }
        set
        {
            this.postalCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class LargeMailUserTypeLargeMailUserName
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class LargeMailUserTypeLargeMailUserIdentifier
{

    private string typeField;

    private string indicatorField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Indicator
    {
        get
        {
            return this.indicatorField;
        }
        set
        {
            this.indicatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class Thoroughfare
{

    private AddressLine[] addressLineField;

    private object[] itemsField;

    private ThoroughfareNumberPrefix[] thoroughfareNumberPrefixField;

    private ThoroughfareNumberSuffix[] thoroughfareNumberSuffixField;

    private ThoroughfarePreDirectionType thoroughfarePreDirectionField;

    private ThoroughfareLeadingTypeType thoroughfareLeadingTypeField;

    private ThoroughfareNameType[] thoroughfareNameField;

    private ThoroughfareTrailingTypeType thoroughfareTrailingTypeField;

    private ThoroughfarePostDirectionType thoroughfarePostDirectionField;

    private ThoroughfareDependentThoroughfare dependentThoroughfareField;

    private object itemField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private ThoroughfareDependentThoroughfares dependentThoroughfaresField;

    private bool dependentThoroughfaresFieldSpecified;

    private string dependentThoroughfaresIndicatorField;

    private string dependentThoroughfaresConnectorField;

    private string dependentThoroughfaresTypeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ThoroughfareNumber", typeof(ThoroughfareNumber))]
    [System.Xml.Serialization.XmlElementAttribute("ThoroughfareNumberRange", typeof(ThoroughfareThoroughfareNumberRange))]
    public object[] Items
    {
        get
        {
            return this.itemsField;
        }
        set
        {
            this.itemsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ThoroughfareNumberPrefix")]
    public ThoroughfareNumberPrefix[] ThoroughfareNumberPrefix
    {
        get
        {
            return this.thoroughfareNumberPrefixField;
        }
        set
        {
            this.thoroughfareNumberPrefixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ThoroughfareNumberSuffix")]
    public ThoroughfareNumberSuffix[] ThoroughfareNumberSuffix
    {
        get
        {
            return this.thoroughfareNumberSuffixField;
        }
        set
        {
            this.thoroughfareNumberSuffixField = value;
        }
    }

    /// <remarks/>
    public ThoroughfarePreDirectionType ThoroughfarePreDirection
    {
        get
        {
            return this.thoroughfarePreDirectionField;
        }
        set
        {
            this.thoroughfarePreDirectionField = value;
        }
    }

    /// <remarks/>
    public ThoroughfareLeadingTypeType ThoroughfareLeadingType
    {
        get
        {
            return this.thoroughfareLeadingTypeField;
        }
        set
        {
            this.thoroughfareLeadingTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ThoroughfareName")]
    public ThoroughfareNameType[] ThoroughfareName
    {
        get
        {
            return this.thoroughfareNameField;
        }
        set
        {
            this.thoroughfareNameField = value;
        }
    }

    /// <remarks/>
    public ThoroughfareTrailingTypeType ThoroughfareTrailingType
    {
        get
        {
            return this.thoroughfareTrailingTypeField;
        }
        set
        {
            this.thoroughfareTrailingTypeField = value;
        }
    }

    /// <remarks/>
    public ThoroughfarePostDirectionType ThoroughfarePostDirection
    {
        get
        {
            return this.thoroughfarePostDirectionField;
        }
        set
        {
            this.thoroughfarePostDirectionField = value;
        }
    }

    /// <remarks/>
    public ThoroughfareDependentThoroughfare DependentThoroughfare
    {
        get
        {
            return this.dependentThoroughfareField;
        }
        set
        {
            this.dependentThoroughfareField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("DependentLocality", typeof(DependentLocalityType))]
    [System.Xml.Serialization.XmlElementAttribute("Firm", typeof(FirmType))]
    [System.Xml.Serialization.XmlElementAttribute("PostalCode", typeof(PostalCode))]
    [System.Xml.Serialization.XmlElementAttribute("Premise", typeof(Premise))]
    public object Item
    {
        get
        {
            return this.itemField;
        }
        set
        {
            this.itemField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ThoroughfareDependentThoroughfares DependentThoroughfares
    {
        get
        {
            return this.dependentThoroughfaresField;
        }
        set
        {
            this.dependentThoroughfaresField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool DependentThoroughfaresSpecified
    {
        get
        {
            return this.dependentThoroughfaresFieldSpecified;
        }
        set
        {
            this.dependentThoroughfaresFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string DependentThoroughfaresIndicator
    {
        get
        {
            return this.dependentThoroughfaresIndicatorField;
        }
        set
        {
            this.dependentThoroughfaresIndicatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string DependentThoroughfaresConnector
    {
        get
        {
            return this.dependentThoroughfaresConnectorField;
        }
        set
        {
            this.dependentThoroughfaresConnectorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string DependentThoroughfaresType
    {
        get
        {
            return this.dependentThoroughfaresTypeField;
        }
        set
        {
            this.dependentThoroughfaresTypeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class ThoroughfareNumber
{

    private ThoroughfareNumberNumberType numberTypeField;

    private bool numberTypeFieldSpecified;

    private string typeField;

    private string indicatorField;

    private ThoroughfareNumberIndicatorOccurrence indicatorOccurrenceField;

    private bool indicatorOccurrenceFieldSpecified;

    private ThoroughfareNumberNumberOccurrence numberOccurrenceField;

    private bool numberOccurrenceFieldSpecified;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ThoroughfareNumberNumberType NumberType
    {
        get
        {
            return this.numberTypeField;
        }
        set
        {
            this.numberTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool NumberTypeSpecified
    {
        get
        {
            return this.numberTypeFieldSpecified;
        }
        set
        {
            this.numberTypeFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Indicator
    {
        get
        {
            return this.indicatorField;
        }
        set
        {
            this.indicatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ThoroughfareNumberIndicatorOccurrence IndicatorOccurrence
    {
        get
        {
            return this.indicatorOccurrenceField;
        }
        set
        {
            this.indicatorOccurrenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool IndicatorOccurrenceSpecified
    {
        get
        {
            return this.indicatorOccurrenceFieldSpecified;
        }
        set
        {
            this.indicatorOccurrenceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ThoroughfareNumberNumberOccurrence NumberOccurrence
    {
        get
        {
            return this.numberOccurrenceField;
        }
        set
        {
            this.numberOccurrenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool NumberOccurrenceSpecified
    {
        get
        {
            return this.numberOccurrenceFieldSpecified;
        }
        set
        {
            this.numberOccurrenceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum ThoroughfareNumberNumberType
{

    /// <remarks/>
    Single,

    /// <remarks/>
    Range,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum ThoroughfareNumberIndicatorOccurrence
{

    /// <remarks/>
    Before,

    /// <remarks/>
    After,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum ThoroughfareNumberNumberOccurrence
{

    /// <remarks/>
    BeforeName,

    /// <remarks/>
    AfterName,

    /// <remarks/>
    BeforeType,

    /// <remarks/>
    AfterType,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class ThoroughfareThoroughfareNumberRange
{

    private AddressLine[] addressLineField;

    private ThoroughfareThoroughfareNumberRangeThoroughfareNumberFrom thoroughfareNumberFromField;

    private ThoroughfareThoroughfareNumberRangeThoroughfareNumberTo thoroughfareNumberToField;

    private ThoroughfareThoroughfareNumberRangeRangeType rangeTypeField;

    private bool rangeTypeFieldSpecified;

    private string indicatorField;

    private string separatorField;

    private ThoroughfareThoroughfareNumberRangeIndicatorOccurrence indicatorOccurrenceField;

    private bool indicatorOccurrenceFieldSpecified;

    private ThoroughfareThoroughfareNumberRangeNumberRangeOccurrence numberRangeOccurrenceField;

    private bool numberRangeOccurrenceFieldSpecified;

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    public ThoroughfareThoroughfareNumberRangeThoroughfareNumberFrom ThoroughfareNumberFrom
    {
        get
        {
            return this.thoroughfareNumberFromField;
        }
        set
        {
            this.thoroughfareNumberFromField = value;
        }
    }

    /// <remarks/>
    public ThoroughfareThoroughfareNumberRangeThoroughfareNumberTo ThoroughfareNumberTo
    {
        get
        {
            return this.thoroughfareNumberToField;
        }
        set
        {
            this.thoroughfareNumberToField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ThoroughfareThoroughfareNumberRangeRangeType RangeType
    {
        get
        {
            return this.rangeTypeField;
        }
        set
        {
            this.rangeTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool RangeTypeSpecified
    {
        get
        {
            return this.rangeTypeFieldSpecified;
        }
        set
        {
            this.rangeTypeFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Indicator
    {
        get
        {
            return this.indicatorField;
        }
        set
        {
            this.indicatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Separator
    {
        get
        {
            return this.separatorField;
        }
        set
        {
            this.separatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ThoroughfareThoroughfareNumberRangeIndicatorOccurrence IndicatorOccurrence
    {
        get
        {
            return this.indicatorOccurrenceField;
        }
        set
        {
            this.indicatorOccurrenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool IndicatorOccurrenceSpecified
    {
        get
        {
            return this.indicatorOccurrenceFieldSpecified;
        }
        set
        {
            this.indicatorOccurrenceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ThoroughfareThoroughfareNumberRangeNumberRangeOccurrence NumberRangeOccurrence
    {
        get
        {
            return this.numberRangeOccurrenceField;
        }
        set
        {
            this.numberRangeOccurrenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool NumberRangeOccurrenceSpecified
    {
        get
        {
            return this.numberRangeOccurrenceFieldSpecified;
        }
        set
        {
            this.numberRangeOccurrenceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class ThoroughfareThoroughfareNumberRangeThoroughfareNumberFrom
{

    private AddressLine[] addressLineField;

    private ThoroughfareNumberPrefix[] thoroughfareNumberPrefixField;

    private ThoroughfareNumber[] thoroughfareNumberField;

    private ThoroughfareNumberSuffix[] thoroughfareNumberSuffixField;

    private string[] textField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ThoroughfareNumberPrefix")]
    public ThoroughfareNumberPrefix[] ThoroughfareNumberPrefix
    {
        get
        {
            return this.thoroughfareNumberPrefixField;
        }
        set
        {
            this.thoroughfareNumberPrefixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ThoroughfareNumber")]
    public ThoroughfareNumber[] ThoroughfareNumber
    {
        get
        {
            return this.thoroughfareNumberField;
        }
        set
        {
            this.thoroughfareNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ThoroughfareNumberSuffix")]
    public ThoroughfareNumberSuffix[] ThoroughfareNumberSuffix
    {
        get
        {
            return this.thoroughfareNumberSuffixField;
        }
        set
        {
            this.thoroughfareNumberSuffixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class ThoroughfareNumberPrefix
{

    private string numberPrefixSeparatorField;

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NumberPrefixSeparator
    {
        get
        {
            return this.numberPrefixSeparatorField;
        }
        set
        {
            this.numberPrefixSeparatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class ThoroughfareNumberSuffix
{

    private string numberSuffixSeparatorField;

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NumberSuffixSeparator
    {
        get
        {
            return this.numberSuffixSeparatorField;
        }
        set
        {
            this.numberSuffixSeparatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class ThoroughfareThoroughfareNumberRangeThoroughfareNumberTo
{

    private AddressLine[] addressLineField;

    private ThoroughfareNumberPrefix[] thoroughfareNumberPrefixField;

    private ThoroughfareNumber[] thoroughfareNumberField;

    private ThoroughfareNumberSuffix[] thoroughfareNumberSuffixField;

    private string[] textField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ThoroughfareNumberPrefix")]
    public ThoroughfareNumberPrefix[] ThoroughfareNumberPrefix
    {
        get
        {
            return this.thoroughfareNumberPrefixField;
        }
        set
        {
            this.thoroughfareNumberPrefixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ThoroughfareNumber")]
    public ThoroughfareNumber[] ThoroughfareNumber
    {
        get
        {
            return this.thoroughfareNumberField;
        }
        set
        {
            this.thoroughfareNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ThoroughfareNumberSuffix")]
    public ThoroughfareNumberSuffix[] ThoroughfareNumberSuffix
    {
        get
        {
            return this.thoroughfareNumberSuffixField;
        }
        set
        {
            this.thoroughfareNumberSuffixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum ThoroughfareThoroughfareNumberRangeRangeType
{

    /// <remarks/>
    Odd,

    /// <remarks/>
    Even,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum ThoroughfareThoroughfareNumberRangeIndicatorOccurrence
{

    /// <remarks/>
    Before,

    /// <remarks/>
    After,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum ThoroughfareThoroughfareNumberRangeNumberRangeOccurrence
{

    /// <remarks/>
    BeforeName,

    /// <remarks/>
    AfterName,

    /// <remarks/>
    BeforeType,

    /// <remarks/>
    AfterType,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class ThoroughfarePreDirectionType
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class ThoroughfareLeadingTypeType
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class ThoroughfareNameType
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class ThoroughfareTrailingTypeType
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class ThoroughfarePostDirectionType
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class ThoroughfareDependentThoroughfare
{

    private AddressLine[] addressLineField;

    private ThoroughfarePreDirectionType thoroughfarePreDirectionField;

    private ThoroughfareLeadingTypeType thoroughfareLeadingTypeField;

    private ThoroughfareNameType[] thoroughfareNameField;

    private ThoroughfareTrailingTypeType thoroughfareTrailingTypeField;

    private ThoroughfarePostDirectionType thoroughfarePostDirectionField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    public ThoroughfarePreDirectionType ThoroughfarePreDirection
    {
        get
        {
            return this.thoroughfarePreDirectionField;
        }
        set
        {
            this.thoroughfarePreDirectionField = value;
        }
    }

    /// <remarks/>
    public ThoroughfareLeadingTypeType ThoroughfareLeadingType
    {
        get
        {
            return this.thoroughfareLeadingTypeField;
        }
        set
        {
            this.thoroughfareLeadingTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ThoroughfareName")]
    public ThoroughfareNameType[] ThoroughfareName
    {
        get
        {
            return this.thoroughfareNameField;
        }
        set
        {
            this.thoroughfareNameField = value;
        }
    }

    /// <remarks/>
    public ThoroughfareTrailingTypeType ThoroughfareTrailingType
    {
        get
        {
            return this.thoroughfareTrailingTypeField;
        }
        set
        {
            this.thoroughfareTrailingTypeField = value;
        }
    }

    /// <remarks/>
    public ThoroughfarePostDirectionType ThoroughfarePostDirection
    {
        get
        {
            return this.thoroughfarePostDirectionField;
        }
        set
        {
            this.thoroughfarePostDirectionField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class Premise
{

    private AddressLine[] addressLineField;

    private PremisePremiseName[] premiseNameField;

    private object[] itemsField;

    private PremiseNumberPrefix[] premiseNumberPrefixField;

    private PremiseNumberSuffix[] premiseNumberSuffixField;

    private BuildingNameType[] buildingNameField;

    private object[] items1Field;

    private MailStopType mailStopField;

    private PostalCode postalCodeField;

    private Premise premise1Field;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private string premiseDependencyField;

    private string premiseDependencyTypeField;

    private string premiseThoroughfareConnectorField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PremiseName")]
    public PremisePremiseName[] PremiseName
    {
        get
        {
            return this.premiseNameField;
        }
        set
        {
            this.premiseNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PremiseLocation", typeof(PremisePremiseLocation))]
    [System.Xml.Serialization.XmlElementAttribute("PremiseNumber", typeof(PremiseNumber))]
    [System.Xml.Serialization.XmlElementAttribute("PremiseNumberRange", typeof(PremisePremiseNumberRange))]
    public object[] Items
    {
        get
        {
            return this.itemsField;
        }
        set
        {
            this.itemsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PremiseNumberPrefix")]
    public PremiseNumberPrefix[] PremiseNumberPrefix
    {
        get
        {
            return this.premiseNumberPrefixField;
        }
        set
        {
            this.premiseNumberPrefixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PremiseNumberSuffix")]
    public PremiseNumberSuffix[] PremiseNumberSuffix
    {
        get
        {
            return this.premiseNumberSuffixField;
        }
        set
        {
            this.premiseNumberSuffixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("BuildingName")]
    public BuildingNameType[] BuildingName
    {
        get
        {
            return this.buildingNameField;
        }
        set
        {
            this.buildingNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Firm", typeof(FirmType))]
    [System.Xml.Serialization.XmlElementAttribute("SubPremise", typeof(SubPremiseType))]
    public object[] Items1
    {
        get
        {
            return this.items1Field;
        }
        set
        {
            this.items1Field = value;
        }
    }

    /// <remarks/>
    public MailStopType MailStop
    {
        get
        {
            return this.mailStopField;
        }
        set
        {
            this.mailStopField = value;
        }
    }

    /// <remarks/>
    public PostalCode PostalCode
    {
        get
        {
            return this.postalCodeField;
        }
        set
        {
            this.postalCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Premise")]
    public Premise Premise1
    {
        get
        {
            return this.premise1Field;
        }
        set
        {
            this.premise1Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string PremiseDependency
    {
        get
        {
            return this.premiseDependencyField;
        }
        set
        {
            this.premiseDependencyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string PremiseDependencyType
    {
        get
        {
            return this.premiseDependencyTypeField;
        }
        set
        {
            this.premiseDependencyTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string PremiseThoroughfareConnector
    {
        get
        {
            return this.premiseThoroughfareConnectorField;
        }
        set
        {
            this.premiseThoroughfareConnectorField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PremisePremiseName
{

    private string typeField;

    private PremisePremiseNameTypeOccurrence typeOccurrenceField;

    private bool typeOccurrenceFieldSpecified;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public PremisePremiseNameTypeOccurrence TypeOccurrence
    {
        get
        {
            return this.typeOccurrenceField;
        }
        set
        {
            this.typeOccurrenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool TypeOccurrenceSpecified
    {
        get
        {
            return this.typeOccurrenceFieldSpecified;
        }
        set
        {
            this.typeOccurrenceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum PremisePremiseNameTypeOccurrence
{

    /// <remarks/>
    Before,

    /// <remarks/>
    After,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PremisePremiseLocation
{

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class PremiseNumber
{

    private PremiseNumberNumberType numberTypeField;

    private bool numberTypeFieldSpecified;

    private string typeField;

    private string indicatorField;

    private PremiseNumberIndicatorOccurrence indicatorOccurrenceField;

    private bool indicatorOccurrenceFieldSpecified;

    private PremiseNumberNumberTypeOccurrence numberTypeOccurrenceField;

    private bool numberTypeOccurrenceFieldSpecified;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public PremiseNumberNumberType NumberType
    {
        get
        {
            return this.numberTypeField;
        }
        set
        {
            this.numberTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool NumberTypeSpecified
    {
        get
        {
            return this.numberTypeFieldSpecified;
        }
        set
        {
            this.numberTypeFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Indicator
    {
        get
        {
            return this.indicatorField;
        }
        set
        {
            this.indicatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public PremiseNumberIndicatorOccurrence IndicatorOccurrence
    {
        get
        {
            return this.indicatorOccurrenceField;
        }
        set
        {
            this.indicatorOccurrenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool IndicatorOccurrenceSpecified
    {
        get
        {
            return this.indicatorOccurrenceFieldSpecified;
        }
        set
        {
            this.indicatorOccurrenceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public PremiseNumberNumberTypeOccurrence NumberTypeOccurrence
    {
        get
        {
            return this.numberTypeOccurrenceField;
        }
        set
        {
            this.numberTypeOccurrenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool NumberTypeOccurrenceSpecified
    {
        get
        {
            return this.numberTypeOccurrenceFieldSpecified;
        }
        set
        {
            this.numberTypeOccurrenceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum PremiseNumberNumberType
{

    /// <remarks/>
    Single,

    /// <remarks/>
    Range,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum PremiseNumberIndicatorOccurrence
{

    /// <remarks/>
    Before,

    /// <remarks/>
    After,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum PremiseNumberNumberTypeOccurrence
{

    /// <remarks/>
    Before,

    /// <remarks/>
    After,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PremisePremiseNumberRange
{

    private PremisePremiseNumberRangePremiseNumberRangeFrom premiseNumberRangeFromField;

    private PremisePremiseNumberRangePremiseNumberRangeTo premiseNumberRangeToField;

    private string rangeTypeField;

    private string indicatorField;

    private string separatorField;

    private string typeField;

    private PremisePremiseNumberRangeIndicatorOccurence indicatorOccurenceField;

    private bool indicatorOccurenceFieldSpecified;

    private PremisePremiseNumberRangeNumberRangeOccurence numberRangeOccurenceField;

    private bool numberRangeOccurenceFieldSpecified;

    /// <remarks/>
    public PremisePremiseNumberRangePremiseNumberRangeFrom PremiseNumberRangeFrom
    {
        get
        {
            return this.premiseNumberRangeFromField;
        }
        set
        {
            this.premiseNumberRangeFromField = value;
        }
    }

    /// <remarks/>
    public PremisePremiseNumberRangePremiseNumberRangeTo PremiseNumberRangeTo
    {
        get
        {
            return this.premiseNumberRangeToField;
        }
        set
        {
            this.premiseNumberRangeToField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string RangeType
    {
        get
        {
            return this.rangeTypeField;
        }
        set
        {
            this.rangeTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Indicator
    {
        get
        {
            return this.indicatorField;
        }
        set
        {
            this.indicatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Separator
    {
        get
        {
            return this.separatorField;
        }
        set
        {
            this.separatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public PremisePremiseNumberRangeIndicatorOccurence IndicatorOccurence
    {
        get
        {
            return this.indicatorOccurenceField;
        }
        set
        {
            this.indicatorOccurenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool IndicatorOccurenceSpecified
    {
        get
        {
            return this.indicatorOccurenceFieldSpecified;
        }
        set
        {
            this.indicatorOccurenceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public PremisePremiseNumberRangeNumberRangeOccurence NumberRangeOccurence
    {
        get
        {
            return this.numberRangeOccurenceField;
        }
        set
        {
            this.numberRangeOccurenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool NumberRangeOccurenceSpecified
    {
        get
        {
            return this.numberRangeOccurenceFieldSpecified;
        }
        set
        {
            this.numberRangeOccurenceFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PremisePremiseNumberRangePremiseNumberRangeFrom
{

    private AddressLine[] addressLineField;

    private PremiseNumberPrefix[] premiseNumberPrefixField;

    private PremiseNumber[] premiseNumberField;

    private PremiseNumberSuffix[] premiseNumberSuffixField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PremiseNumberPrefix")]
    public PremiseNumberPrefix[] PremiseNumberPrefix
    {
        get
        {
            return this.premiseNumberPrefixField;
        }
        set
        {
            this.premiseNumberPrefixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PremiseNumber")]
    public PremiseNumber[] PremiseNumber
    {
        get
        {
            return this.premiseNumberField;
        }
        set
        {
            this.premiseNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PremiseNumberSuffix")]
    public PremiseNumberSuffix[] PremiseNumberSuffix
    {
        get
        {
            return this.premiseNumberSuffixField;
        }
        set
        {
            this.premiseNumberSuffixField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class PremiseNumberPrefix
{

    private string numberPrefixSeparatorField;

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string valueField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NumberPrefixSeparator
    {
        get
        {
            return this.numberPrefixSeparatorField;
        }
        set
        {
            this.numberPrefixSeparatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class PremiseNumberSuffix
{

    private string numberSuffixSeparatorField;

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NumberSuffixSeparator
    {
        get
        {
            return this.numberSuffixSeparatorField;
        }
        set
        {
            this.numberSuffixSeparatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PremisePremiseNumberRangePremiseNumberRangeTo
{

    private AddressLine[] addressLineField;

    private PremiseNumberPrefix[] premiseNumberPrefixField;

    private PremiseNumber[] premiseNumberField;

    private PremiseNumberSuffix[] premiseNumberSuffixField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PremiseNumberPrefix")]
    public PremiseNumberPrefix[] PremiseNumberPrefix
    {
        get
        {
            return this.premiseNumberPrefixField;
        }
        set
        {
            this.premiseNumberPrefixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PremiseNumber")]
    public PremiseNumber[] PremiseNumber
    {
        get
        {
            return this.premiseNumberField;
        }
        set
        {
            this.premiseNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PremiseNumberSuffix")]
    public PremiseNumberSuffix[] PremiseNumberSuffix
    {
        get
        {
            return this.premiseNumberSuffixField;
        }
        set
        {
            this.premiseNumberSuffixField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum PremisePremiseNumberRangeIndicatorOccurence
{

    /// <remarks/>
    Before,

    /// <remarks/>
    After,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum PremisePremiseNumberRangeNumberRangeOccurence
{

    /// <remarks/>
    BeforeName,

    /// <remarks/>
    AfterName,

    /// <remarks/>
    BeforeType,

    /// <remarks/>
    AfterType,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum ThoroughfareDependentThoroughfares
{

    /// <remarks/>
    Yes,

    /// <remarks/>
    No,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class PostOffice
{

    private AddressLine[] addressLineField;

    private object[] itemsField;

    private PostalRouteType postalRouteField;

    private PostBox postBoxField;

    private PostalCode postalCodeField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private string indicatorField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PostOfficeName", typeof(PostOfficePostOfficeName))]
    [System.Xml.Serialization.XmlElementAttribute("PostOfficeNumber", typeof(PostOfficePostOfficeNumber))]
    public object[] Items
    {
        get
        {
            return this.itemsField;
        }
        set
        {
            this.itemsField = value;
        }
    }

    /// <remarks/>
    public PostalRouteType PostalRoute
    {
        get
        {
            return this.postalRouteField;
        }
        set
        {
            this.postalRouteField = value;
        }
    }

    /// <remarks/>
    public PostBox PostBox
    {
        get
        {
            return this.postBoxField;
        }
        set
        {
            this.postBoxField = value;
        }
    }

    /// <remarks/>
    public PostalCode PostalCode
    {
        get
        {
            return this.postalCodeField;
        }
        set
        {
            this.postalCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Indicator
    {
        get
        {
            return this.indicatorField;
        }
        set
        {
            this.indicatorField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PostOfficePostOfficeName
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class PostOfficePostOfficeNumber
{

    private string indicatorField;

    private PostOfficePostOfficeNumberIndicatorOccurrence indicatorOccurrenceField;

    private bool indicatorOccurrenceFieldSpecified;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Indicator
    {
        get
        {
            return this.indicatorField;
        }
        set
        {
            this.indicatorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public PostOfficePostOfficeNumberIndicatorOccurrence IndicatorOccurrence
    {
        get
        {
            return this.indicatorOccurrenceField;
        }
        set
        {
            this.indicatorOccurrenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool IndicatorOccurrenceSpecified
    {
        get
        {
            return this.indicatorOccurrenceFieldSpecified;
        }
        set
        {
            this.indicatorOccurrenceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public enum PostOfficePostOfficeNumberIndicatorOccurrence
{

    /// <remarks/>
    Before,

    /// <remarks/>
    After,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressLinesType
{

    private AddressLine[] addressLineField;

    private System.Xml.Linq.XElement[] anyField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class AddressDetails
{

    private AddressDetailsPostalServiceElements postalServiceElementsField;

    private object itemField;

    private System.Xml.Linq.XElement[] anyField;

    private string addressTypeField;

    private string currentStatusField;

    private string validFromDateField;

    private string validToDateField;

    private string usageField;

    private string codeField;

    private string addressDetailsKeyField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    public AddressDetailsPostalServiceElements PostalServiceElements
    {
        get
        {
            return this.postalServiceElementsField;
        }
        set
        {
            this.postalServiceElementsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Address", typeof(AddressDetailsAddress))]
    [System.Xml.Serialization.XmlElementAttribute("AddressLines", typeof(AddressLinesType))]
    [System.Xml.Serialization.XmlElementAttribute("AdministrativeArea", typeof(AdministrativeArea))]
    [System.Xml.Serialization.XmlElementAttribute("Country", typeof(AddressDetailsCountry))]
    [System.Xml.Serialization.XmlElementAttribute("Locality", typeof(Locality))]
    [System.Xml.Serialization.XmlElementAttribute("Thoroughfare", typeof(Thoroughfare))]
    public object Item
    {
        get
        {
            return this.itemField;
        }
        set
        {
            this.itemField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string AddressType
    {
        get
        {
            return this.addressTypeField;
        }
        set
        {
            this.addressTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CurrentStatus
    {
        get
        {
            return this.currentStatusField;
        }
        set
        {
            this.currentStatusField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ValidFromDate
    {
        get
        {
            return this.validFromDateField;
        }
        set
        {
            this.validFromDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ValidToDate
    {
        get
        {
            return this.validToDateField;
        }
        set
        {
            this.validToDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Usage
    {
        get
        {
            return this.usageField;
        }
        set
        {
            this.usageField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string AddressDetailsKey
    {
        get
        {
            return this.addressDetailsKeyField;
        }
        set
        {
            this.addressDetailsKeyField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressDetailsPostalServiceElements
{

    private AddressDetailsPostalServiceElementsAddressIdentifier[] addressIdentifierField;

    private AddressDetailsPostalServiceElementsEndorsementLineCode endorsementLineCodeField;

    private AddressDetailsPostalServiceElementsKeyLineCode keyLineCodeField;

    private AddressDetailsPostalServiceElementsBarcode barcodeField;

    private AddressDetailsPostalServiceElementsSortingCode sortingCodeField;

    private AddressDetailsPostalServiceElementsAddressLatitude addressLatitudeField;

    private AddressDetailsPostalServiceElementsAddressLatitudeDirection addressLatitudeDirectionField;

    private AddressDetailsPostalServiceElementsAddressLongitude addressLongitudeField;

    private AddressDetailsPostalServiceElementsAddressLongitudeDirection addressLongitudeDirectionField;

    private AddressDetailsPostalServiceElementsSupplementaryPostalServiceData[] supplementaryPostalServiceDataField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressIdentifier")]
    public AddressDetailsPostalServiceElementsAddressIdentifier[] AddressIdentifier
    {
        get
        {
            return this.addressIdentifierField;
        }
        set
        {
            this.addressIdentifierField = value;
        }
    }

    /// <remarks/>
    public AddressDetailsPostalServiceElementsEndorsementLineCode EndorsementLineCode
    {
        get
        {
            return this.endorsementLineCodeField;
        }
        set
        {
            this.endorsementLineCodeField = value;
        }
    }

    /// <remarks/>
    public AddressDetailsPostalServiceElementsKeyLineCode KeyLineCode
    {
        get
        {
            return this.keyLineCodeField;
        }
        set
        {
            this.keyLineCodeField = value;
        }
    }

    /// <remarks/>
    public AddressDetailsPostalServiceElementsBarcode Barcode
    {
        get
        {
            return this.barcodeField;
        }
        set
        {
            this.barcodeField = value;
        }
    }

    /// <remarks/>
    public AddressDetailsPostalServiceElementsSortingCode SortingCode
    {
        get
        {
            return this.sortingCodeField;
        }
        set
        {
            this.sortingCodeField = value;
        }
    }

    /// <remarks/>
    public AddressDetailsPostalServiceElementsAddressLatitude AddressLatitude
    {
        get
        {
            return this.addressLatitudeField;
        }
        set
        {
            this.addressLatitudeField = value;
        }
    }

    /// <remarks/>
    public AddressDetailsPostalServiceElementsAddressLatitudeDirection AddressLatitudeDirection
    {
        get
        {
            return this.addressLatitudeDirectionField;
        }
        set
        {
            this.addressLatitudeDirectionField = value;
        }
    }

    /// <remarks/>
    public AddressDetailsPostalServiceElementsAddressLongitude AddressLongitude
    {
        get
        {
            return this.addressLongitudeField;
        }
        set
        {
            this.addressLongitudeField = value;
        }
    }

    /// <remarks/>
    public AddressDetailsPostalServiceElementsAddressLongitudeDirection AddressLongitudeDirection
    {
        get
        {
            return this.addressLongitudeDirectionField;
        }
        set
        {
            this.addressLongitudeDirectionField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("SupplementaryPostalServiceData")]
    public AddressDetailsPostalServiceElementsSupplementaryPostalServiceData[] SupplementaryPostalServiceData
    {
        get
        {
            return this.supplementaryPostalServiceDataField;
        }
        set
        {
            this.supplementaryPostalServiceDataField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressDetailsPostalServiceElementsAddressIdentifier
{

    private string identifierTypeField;

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string IdentifierType
    {
        get
        {
            return this.identifierTypeField;
        }
        set
        {
            this.identifierTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressDetailsPostalServiceElementsEndorsementLineCode
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressDetailsPostalServiceElementsKeyLineCode
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressDetailsPostalServiceElementsBarcode
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressDetailsPostalServiceElementsSortingCode
{

    private string typeField;

    private string codeField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressDetailsPostalServiceElementsAddressLatitude
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressDetailsPostalServiceElementsAddressLatitudeDirection
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressDetailsPostalServiceElementsAddressLongitude
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressDetailsPostalServiceElementsAddressLongitudeDirection
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressDetailsPostalServiceElementsSupplementaryPostalServiceData
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressDetailsAddress
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class AdministrativeArea
{

    private AddressLine[] addressLineField;

    private AdministrativeAreaAdministrativeAreaName[] administrativeAreaNameField;

    private AdministrativeAreaSubAdministrativeArea subAdministrativeAreaField;

    private object itemField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private string usageTypeField;

    private string indicatorField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AdministrativeAreaName")]
    public AdministrativeAreaAdministrativeAreaName[] AdministrativeAreaName
    {
        get
        {
            return this.administrativeAreaNameField;
        }
        set
        {
            this.administrativeAreaNameField = value;
        }
    }

    /// <remarks/>
    public AdministrativeAreaSubAdministrativeArea SubAdministrativeArea
    {
        get
        {
            return this.subAdministrativeAreaField;
        }
        set
        {
            this.subAdministrativeAreaField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Locality", typeof(Locality))]
    [System.Xml.Serialization.XmlElementAttribute("PostOffice", typeof(PostOffice))]
    [System.Xml.Serialization.XmlElementAttribute("PostalCode", typeof(PostalCode))]
    public object Item
    {
        get
        {
            return this.itemField;
        }
        set
        {
            this.itemField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string UsageType
    {
        get
        {
            return this.usageTypeField;
        }
        set
        {
            this.usageTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Indicator
    {
        get
        {
            return this.indicatorField;
        }
        set
        {
            this.indicatorField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AdministrativeAreaAdministrativeAreaName
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AdministrativeAreaSubAdministrativeArea
{

    private AddressLine[] addressLineField;

    private AdministrativeAreaSubAdministrativeAreaSubAdministrativeAreaName[] subAdministrativeAreaNameField;

    private object itemField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private string usageTypeField;

    private string indicatorField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("SubAdministrativeAreaName")]
    public AdministrativeAreaSubAdministrativeAreaSubAdministrativeAreaName[] SubAdministrativeAreaName
    {
        get
        {
            return this.subAdministrativeAreaNameField;
        }
        set
        {
            this.subAdministrativeAreaNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Locality", typeof(Locality))]
    [System.Xml.Serialization.XmlElementAttribute("PostOffice", typeof(PostOffice))]
    [System.Xml.Serialization.XmlElementAttribute("PostalCode", typeof(PostalCode))]
    public object Item
    {
        get
        {
            return this.itemField;
        }
        set
        {
            this.itemField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string UsageType
    {
        get
        {
            return this.usageTypeField;
        }
        set
        {
            this.usageTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Indicator
    {
        get
        {
            return this.indicatorField;
        }
        set
        {
            this.indicatorField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AdministrativeAreaSubAdministrativeAreaSubAdministrativeAreaName
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class Locality
{

    private AddressLine[] addressLineField;

    private LocalityLocalityName[] localityNameField;

    private object itemField;

    private Thoroughfare thoroughfareField;

    private Premise premiseField;

    private DependentLocalityType dependentLocalityField;

    private PostalCode postalCodeField;

    private System.Xml.Linq.XElement[] anyField;

    private string typeField;

    private string usageTypeField;

    private string indicatorField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("LocalityName")]
    public LocalityLocalityName[] LocalityName
    {
        get
        {
            return this.localityNameField;
        }
        set
        {
            this.localityNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("LargeMailUser", typeof(LargeMailUserType))]
    [System.Xml.Serialization.XmlElementAttribute("PostBox", typeof(PostBox))]
    [System.Xml.Serialization.XmlElementAttribute("PostOffice", typeof(PostOffice))]
    [System.Xml.Serialization.XmlElementAttribute("PostalRoute", typeof(PostalRouteType))]
    public object Item
    {
        get
        {
            return this.itemField;
        }
        set
        {
            this.itemField = value;
        }
    }

    /// <remarks/>
    public Thoroughfare Thoroughfare
    {
        get
        {
            return this.thoroughfareField;
        }
        set
        {
            this.thoroughfareField = value;
        }
    }

    /// <remarks/>
    public Premise Premise
    {
        get
        {
            return this.premiseField;
        }
        set
        {
            this.premiseField = value;
        }
    }

    /// <remarks/>
    public DependentLocalityType DependentLocality
    {
        get
        {
            return this.dependentLocalityField;
        }
        set
        {
            this.dependentLocalityField = value;
        }
    }

    /// <remarks/>
    public PostalCode PostalCode
    {
        get
        {
            return this.postalCodeField;
        }
        set
        {
            this.postalCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string UsageType
    {
        get
        {
            return this.usageTypeField;
        }
        set
        {
            this.usageTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Indicator
    {
        get
        {
            return this.indicatorField;
        }
        set
        {
            this.indicatorField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class LocalityLocalityName
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressDetailsCountry
{

    private AddressLine[] addressLineField;

    private AddressDetailsCountryCountryNameCode[] countryNameCodeField;

    private CountryName[] countryNameField;

    private object itemField;

    private System.Xml.Linq.XElement[] anyField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
    public AddressLine[] AddressLine
    {
        get
        {
            return this.addressLineField;
        }
        set
        {
            this.addressLineField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("CountryNameCode")]
    public AddressDetailsCountryCountryNameCode[] CountryNameCode
    {
        get
        {
            return this.countryNameCodeField;
        }
        set
        {
            this.countryNameCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("CountryName")]
    public CountryName[] CountryName
    {
        get
        {
            return this.countryNameField;
        }
        set
        {
            this.countryNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AdministrativeArea", typeof(AdministrativeArea))]
    [System.Xml.Serialization.XmlElementAttribute("Locality", typeof(Locality))]
    [System.Xml.Serialization.XmlElementAttribute("Thoroughfare", typeof(Thoroughfare))]
    public object Item
    {
        get
        {
            return this.itemField;
        }
        set
        {
            this.itemField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.Linq.XElement[] Any
    {
        get
        {
            return this.anyField;
        }
        set
        {
            this.anyField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
public partial class AddressDetailsCountryCountryNameCode
{

    private string schemeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Scheme
    {
        get
        {
            return this.schemeField;
        }
        set
        {
            this.schemeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0", IsNullable = false)]
public partial class CountryName
{

    private string typeField;

    private string codeField;

    private System.Xml.Linq.XAttribute[] anyAttrField;

    private string[] textField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>

    public System.Xml.Linq.XAttribute[] AnyAttr
    {
        get
        {
            return this.anyAttrField;
        }
        set
        {
            this.anyAttrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text
    {
        get
        {
            return this.textField;
        }
        set
        {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Diagnostics.DebuggerStepThroughAttribute()]

[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2005/Atom")]
[System.Xml.Serialization.XmlRootAttribute("author", Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
public partial class atomPersonConstruct
{

    private string[] itemsField;

    private ItemsChoiceType[] itemsElementNameField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("email", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("name", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("uri", typeof(string))]
    [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
    public string[] Items
    {
        get
        {
            return this.itemsField;
        }
        set
        {
            this.itemsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public ItemsChoiceType[] ItemsElementName
    {
        get
        {
            return this.itemsElementNameField;
        }
        set
        {
            this.itemsElementNameField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[DataContract]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2005/Atom", IncludeInSchema = false)]
public enum ItemsChoiceType
{

    /// <remarks/>
    email,

    /// <remarks/>
    name,

    /// <remarks/>
    uri,
}
