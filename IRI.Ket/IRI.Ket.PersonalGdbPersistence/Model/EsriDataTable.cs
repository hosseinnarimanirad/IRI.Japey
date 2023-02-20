using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.PersonalGdbPersistence.Model; 

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class DETableInfo
{

    private string catalogPathField;

    private string nameField;

    private bool childrenExpandedField;

    private string datasetTypeField;

    private ushort dSIDField;

    private bool versionedField;

    private bool canVersionField;

    private object configurationKeywordField;

    private decimal requiredGeodatabaseClientVersionField;

    private bool hasOIDField;

    private string oIDFieldNameField;

    private DETableInfoGPFieldInfoEx[] gPFieldInfoExsField;

    private string cLSIDField;

    private object eXTCLSIDField;

    private object relationshipClassNamesField;

    private string aliasNameField;

    private object modelNameField;

    private bool hasGlobalIDField;

    private object globalIDFieldNameField;

    private object rasterFieldNameField;

    private DETableInfoExtensionProperties extensionPropertiesField;

    private object controllerMembershipsField;

    private bool editorTrackingEnabledField;

    private object creatorFieldNameField;

    private object createdAtFieldNameField;

    private object editorFieldNameField;

    private object editedAtFieldNameField;

    private bool isTimeInUTCField;

    private bool changeTrackedField;

    /// <remarks/>
    public string CatalogPath
    {
        get
        {
            return this.catalogPathField;
        }
        set
        {
            this.catalogPathField = value;
        }
    }

    /// <remarks/>
    public string Name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    public bool ChildrenExpanded
    {
        get
        {
            return this.childrenExpandedField;
        }
        set
        {
            this.childrenExpandedField = value;
        }
    }

    /// <remarks/>
    public string DatasetType
    {
        get
        {
            return this.datasetTypeField;
        }
        set
        {
            this.datasetTypeField = value;
        }
    }

    /// <remarks/>
    public ushort DSID
    {
        get
        {
            return this.dSIDField;
        }
        set
        {
            this.dSIDField = value;
        }
    }

    /// <remarks/>
    public bool Versioned
    {
        get
        {
            return this.versionedField;
        }
        set
        {
            this.versionedField = value;
        }
    }

    /// <remarks/>
    public bool CanVersion
    {
        get
        {
            return this.canVersionField;
        }
        set
        {
            this.canVersionField = value;
        }
    }

    /// <remarks/>
    public object ConfigurationKeyword
    {
        get
        {
            return this.configurationKeywordField;
        }
        set
        {
            this.configurationKeywordField = value;
        }
    }

    /// <remarks/>
    public decimal RequiredGeodatabaseClientVersion
    {
        get
        {
            return this.requiredGeodatabaseClientVersionField;
        }
        set
        {
            this.requiredGeodatabaseClientVersionField = value;
        }
    }

    /// <remarks/>
    public bool HasOID
    {
        get
        {
            return this.hasOIDField;
        }
        set
        {
            this.hasOIDField = value;
        }
    }

    /// <remarks/>
    public string OIDFieldName
    {
        get
        {
            return this.oIDFieldNameField;
        }
        set
        {
            this.oIDFieldNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("GPFieldInfoEx", IsNullable = false)]
    public DETableInfoGPFieldInfoEx[] GPFieldInfoExs
    {
        get
        {
            return this.gPFieldInfoExsField;
        }
        set
        {
            this.gPFieldInfoExsField = value;
        }
    }

    /// <remarks/>
    public string CLSID
    {
        get
        {
            return this.cLSIDField;
        }
        set
        {
            this.cLSIDField = value;
        }
    }

    /// <remarks/>
    public object EXTCLSID
    {
        get
        {
            return this.eXTCLSIDField;
        }
        set
        {
            this.eXTCLSIDField = value;
        }
    }

    /// <remarks/>
    public object RelationshipClassNames
    {
        get
        {
            return this.relationshipClassNamesField;
        }
        set
        {
            this.relationshipClassNamesField = value;
        }
    }

    /// <remarks/>
    public string AliasName
    {
        get
        {
            return this.aliasNameField;
        }
        set
        {
            this.aliasNameField = value;
        }
    }

    /// <remarks/>
    public object ModelName
    {
        get
        {
            return this.modelNameField;
        }
        set
        {
            this.modelNameField = value;
        }
    }

    /// <remarks/>
    public bool HasGlobalID
    {
        get
        {
            return this.hasGlobalIDField;
        }
        set
        {
            this.hasGlobalIDField = value;
        }
    }

    /// <remarks/>
    public object GlobalIDFieldName
    {
        get
        {
            return this.globalIDFieldNameField;
        }
        set
        {
            this.globalIDFieldNameField = value;
        }
    }

    /// <remarks/>
    public object RasterFieldName
    {
        get
        {
            return this.rasterFieldNameField;
        }
        set
        {
            this.rasterFieldNameField = value;
        }
    }

    /// <remarks/>
    public DETableInfoExtensionProperties ExtensionProperties
    {
        get
        {
            return this.extensionPropertiesField;
        }
        set
        {
            this.extensionPropertiesField = value;
        }
    }

    /// <remarks/>
    public object ControllerMemberships
    {
        get
        {
            return this.controllerMembershipsField;
        }
        set
        {
            this.controllerMembershipsField = value;
        }
    }

    /// <remarks/>
    public bool EditorTrackingEnabled
    {
        get
        {
            return this.editorTrackingEnabledField;
        }
        set
        {
            this.editorTrackingEnabledField = value;
        }
    }

    /// <remarks/>
    public object CreatorFieldName
    {
        get
        {
            return this.creatorFieldNameField;
        }
        set
        {
            this.creatorFieldNameField = value;
        }
    }

    /// <remarks/>
    public object CreatedAtFieldName
    {
        get
        {
            return this.createdAtFieldNameField;
        }
        set
        {
            this.createdAtFieldNameField = value;
        }
    }

    /// <remarks/>
    public object EditorFieldName
    {
        get
        {
            return this.editorFieldNameField;
        }
        set
        {
            this.editorFieldNameField = value;
        }
    }

    /// <remarks/>
    public object EditedAtFieldName
    {
        get
        {
            return this.editedAtFieldNameField;
        }
        set
        {
            this.editedAtFieldNameField = value;
        }
    }

    /// <remarks/>
    public bool IsTimeInUTC
    {
        get
        {
            return this.isTimeInUTCField;
        }
        set
        {
            this.isTimeInUTCField = value;
        }
    }

    /// <remarks/>
    public bool ChangeTracked
    {
        get
        {
            return this.changeTrackedField;
        }
        set
        {
            this.changeTrackedField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class DETableInfoGPFieldInfoEx
{

    private string nameField;

    private string aliasNameField;

    private string modelNameField;

    private string domainNameField;

    private string fieldTypeField;

    private bool isNullableField;

    private bool requiredField;

    private bool requiredFieldSpecified;

    private bool editableField;

    private bool editableFieldSpecified;

    /// <remarks/>
    public string Name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    public string AliasName
    {
        get
        {
            return this.aliasNameField;
        }
        set
        {
            this.aliasNameField = value;
        }
    }

    /// <remarks/>
    public string ModelName
    {
        get
        {
            return this.modelNameField;
        }
        set
        {
            this.modelNameField = value;
        }
    }

    /// <remarks/>
    public string DomainName
    {
        get
        {
            return this.domainNameField;
        }
        set
        {
            this.domainNameField = value;
        }
    }

    /// <remarks/>
    public string FieldType
    {
        get
        {
            return this.fieldTypeField;
        }
        set
        {
            this.fieldTypeField = value;
        }
    }

    /// <remarks/>
    public bool IsNullable
    {
        get
        {
            return this.isNullableField;
        }
        set
        {
            this.isNullableField = value;
        }
    }

    /// <remarks/>
    public bool Required
    {
        get
        {
            return this.requiredField;
        }
        set
        {
            this.requiredField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool RequiredSpecified
    {
        get
        {
            return this.requiredFieldSpecified;
        }
        set
        {
            this.requiredFieldSpecified = value;
        }
    }

    /// <remarks/>
    public bool Editable
    {
        get
        {
            return this.editableField;
        }
        set
        {
            this.editableField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool EditableSpecified
    {
        get
        {
            return this.editableFieldSpecified;
        }
        set
        {
            this.editableFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class DETableInfoExtensionProperties
{

    private object propertyArrayField;

    /// <remarks/>
    public object PropertyArray
    {
        get
        {
            return this.propertyArrayField;
        }
        set
        {
            this.propertyArrayField = value;
        }
    }
}

