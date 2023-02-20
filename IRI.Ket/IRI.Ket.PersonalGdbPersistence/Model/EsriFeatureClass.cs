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
public partial class DEFeatureClassInfo
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

    private DEFeatureClassInfoGPFieldInfoEx[] gPFieldInfoExsField;

    private string cLSIDField;

    private object eXTCLSIDField;

    private object relationshipClassNamesField;

    private string aliasNameField;

    private object modelNameField;

    private bool hasGlobalIDField;

    private object globalIDFieldNameField;

    private object rasterFieldNameField;

    private DEFeatureClassInfoExtensionProperties extensionPropertiesField;

    private object controllerMembershipsField;

    private bool editorTrackingEnabledField;

    private object creatorFieldNameField;

    private object createdAtFieldNameField;

    private object editorFieldNameField;

    private object editedAtFieldNameField;

    private bool isTimeInUTCField;

    private string featureTypeField;

    private string shapeTypeField;

    private string shapeFieldNameField;

    private bool hasMField;

    private bool hasZField;

    private bool hasSpatialIndexField;

    private object areaFieldNameField;

    private string lengthFieldNameField;

    private DEFeatureClassInfoExtent extentField;

    private DEFeatureClassInfoSpatialReference spatialReferenceField;

    private bool changeTrackedField;

    private bool fieldFilteringEnabledField;

    private object filteredFieldNamesField;

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
    public DEFeatureClassInfoGPFieldInfoEx[] GPFieldInfoExs
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
    public DEFeatureClassInfoExtensionProperties ExtensionProperties
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
    public string FeatureType
    {
        get
        {
            return this.featureTypeField;
        }
        set
        {
            this.featureTypeField = value;
        }
    }

    /// <remarks/>
    public string ShapeType
    {
        get
        {
            return this.shapeTypeField;
        }
        set
        {
            this.shapeTypeField = value;
        }
    }

    /// <remarks/>
    public string ShapeFieldName
    {
        get
        {
            return this.shapeFieldNameField;
        }
        set
        {
            this.shapeFieldNameField = value;
        }
    }

    /// <remarks/>
    public bool HasM
    {
        get
        {
            return this.hasMField;
        }
        set
        {
            this.hasMField = value;
        }
    }

    /// <remarks/>
    public bool HasZ
    {
        get
        {
            return this.hasZField;
        }
        set
        {
            this.hasZField = value;
        }
    }

    /// <remarks/>
    public bool HasSpatialIndex
    {
        get
        {
            return this.hasSpatialIndexField;
        }
        set
        {
            this.hasSpatialIndexField = value;
        }
    }

    /// <remarks/>
    public object AreaFieldName
    {
        get
        {
            return this.areaFieldNameField;
        }
        set
        {
            this.areaFieldNameField = value;
        }
    }

    /// <remarks/>
    public string LengthFieldName
    {
        get
        {
            return this.lengthFieldNameField;
        }
        set
        {
            this.lengthFieldNameField = value;
        }
    }

    /// <remarks/>
    public DEFeatureClassInfoExtent Extent
    {
        get
        {
            return this.extentField;
        }
        set
        {
            this.extentField = value;
        }
    }

    /// <remarks/>
    public DEFeatureClassInfoSpatialReference SpatialReference
    {
        get
        {
            return this.spatialReferenceField;
        }
        set
        {
            this.spatialReferenceField = value;
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

    /// <remarks/>
    public bool FieldFilteringEnabled
    {
        get
        {
            return this.fieldFilteringEnabledField;
        }
        set
        {
            this.fieldFilteringEnabledField = value;
        }
    }

    /// <remarks/>
    public object FilteredFieldNames
    {
        get
        {
            return this.filteredFieldNamesField;
        }
        set
        {
            this.filteredFieldNamesField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class DEFeatureClassInfoGPFieldInfoEx
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
public partial class DEFeatureClassInfoExtensionProperties
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

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class DEFeatureClassInfoExtent
{

    private decimal xMinField;

    private decimal yMinField;

    private decimal xMaxField;

    private decimal yMaxField;

    private DEFeatureClassInfoExtentSpatialReference spatialReferenceField;

    /// <remarks/>
    public decimal XMin
    {
        get
        {
            return this.xMinField;
        }
        set
        {
            this.xMinField = value;
        }
    }

    /// <remarks/>
    public decimal YMin
    {
        get
        {
            return this.yMinField;
        }
        set
        {
            this.yMinField = value;
        }
    }

    /// <remarks/>
    public decimal XMax
    {
        get
        {
            return this.xMaxField;
        }
        set
        {
            this.xMaxField = value;
        }
    }

    /// <remarks/>
    public decimal YMax
    {
        get
        {
            return this.yMaxField;
        }
        set
        {
            this.yMaxField = value;
        }
    }

    /// <remarks/>
    public DEFeatureClassInfoExtentSpatialReference SpatialReference
    {
        get
        {
            return this.spatialReferenceField;
        }
        set
        {
            this.spatialReferenceField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class DEFeatureClassInfoExtentSpatialReference
{

    private string wKTField;

    private short xOriginField;

    private short yOriginField;

    private decimal xYScaleField;

    private int zOriginField;

    private ushort zScaleField;

    private int mOriginField;

    private ushort mScaleField;

    private double xYToleranceField;

    private decimal zToleranceField;

    private decimal mToleranceField;

    private bool highPrecisionField;

    private short leftLongitudeField;

    private ushort wKIDField;

    private ushort latestWKIDField;

    /// <remarks/>
    public string WKT
    {
        get
        {
            return this.wKTField;
        }
        set
        {
            this.wKTField = value;
        }
    }

    /// <remarks/>
    public short XOrigin
    {
        get
        {
            return this.xOriginField;
        }
        set
        {
            this.xOriginField = value;
        }
    }

    /// <remarks/>
    public short YOrigin
    {
        get
        {
            return this.yOriginField;
        }
        set
        {
            this.yOriginField = value;
        }
    }

    /// <remarks/>
    public decimal XYScale
    {
        get
        {
            return this.xYScaleField;
        }
        set
        {
            this.xYScaleField = value;
        }
    }

    /// <remarks/>
    public int ZOrigin
    {
        get
        {
            return this.zOriginField;
        }
        set
        {
            this.zOriginField = value;
        }
    }

    /// <remarks/>
    public ushort ZScale
    {
        get
        {
            return this.zScaleField;
        }
        set
        {
            this.zScaleField = value;
        }
    }

    /// <remarks/>
    public int MOrigin
    {
        get
        {
            return this.mOriginField;
        }
        set
        {
            this.mOriginField = value;
        }
    }

    /// <remarks/>
    public ushort MScale
    {
        get
        {
            return this.mScaleField;
        }
        set
        {
            this.mScaleField = value;
        }
    }

    /// <remarks/>
    public double XYTolerance
    {
        get
        {
            return this.xYToleranceField;
        }
        set
        {
            this.xYToleranceField = value;
        }
    }

    /// <remarks/>
    public decimal ZTolerance
    {
        get
        {
            return this.zToleranceField;
        }
        set
        {
            this.zToleranceField = value;
        }
    }

    /// <remarks/>
    public decimal MTolerance
    {
        get
        {
            return this.mToleranceField;
        }
        set
        {
            this.mToleranceField = value;
        }
    }

    /// <remarks/>
    public bool HighPrecision
    {
        get
        {
            return this.highPrecisionField;
        }
        set
        {
            this.highPrecisionField = value;
        }
    }

    /// <remarks/>
    public short LeftLongitude
    {
        get
        {
            return this.leftLongitudeField;
        }
        set
        {
            this.leftLongitudeField = value;
        }
    }

    /// <remarks/>
    public ushort WKID
    {
        get
        {
            return this.wKIDField;
        }
        set
        {
            this.wKIDField = value;
        }
    }

    /// <remarks/>
    public ushort LatestWKID
    {
        get
        {
            return this.latestWKIDField;
        }
        set
        {
            this.latestWKIDField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class DEFeatureClassInfoSpatialReference
{

    private string wKTField;

    private short xOriginField;

    private short yOriginField;

    private decimal xYScaleField;

    private int zOriginField;

    private ushort zScaleField;

    private int mOriginField;

    private ushort mScaleField;

    private double xYToleranceField;

    private decimal zToleranceField;

    private decimal mToleranceField;

    private bool highPrecisionField;

    private short leftLongitudeField;

    private ushort wKIDField;

    private ushort latestWKIDField;

    /// <remarks/>
    public string WKT
    {
        get
        {
            return this.wKTField;
        }
        set
        {
            this.wKTField = value;
        }
    }

    /// <remarks/>
    public short XOrigin
    {
        get
        {
            return this.xOriginField;
        }
        set
        {
            this.xOriginField = value;
        }
    }

    /// <remarks/>
    public short YOrigin
    {
        get
        {
            return this.yOriginField;
        }
        set
        {
            this.yOriginField = value;
        }
    }

    /// <remarks/>
    public decimal XYScale
    {
        get
        {
            return this.xYScaleField;
        }
        set
        {
            this.xYScaleField = value;
        }
    }

    /// <remarks/>
    public int ZOrigin
    {
        get
        {
            return this.zOriginField;
        }
        set
        {
            this.zOriginField = value;
        }
    }

    /// <remarks/>
    public ushort ZScale
    {
        get
        {
            return this.zScaleField;
        }
        set
        {
            this.zScaleField = value;
        }
    }

    /// <remarks/>
    public int MOrigin
    {
        get
        {
            return this.mOriginField;
        }
        set
        {
            this.mOriginField = value;
        }
    }

    /// <remarks/>
    public ushort MScale
    {
        get
        {
            return this.mScaleField;
        }
        set
        {
            this.mScaleField = value;
        }
    }

    /// <remarks/>
    public double XYTolerance
    {
        get
        {
            return this.xYToleranceField;
        }
        set
        {
            this.xYToleranceField = value;
        }
    }

    /// <remarks/>
    public decimal ZTolerance
    {
        get
        {
            return this.zToleranceField;
        }
        set
        {
            this.zToleranceField = value;
        }
    }

    /// <remarks/>
    public decimal MTolerance
    {
        get
        {
            return this.mToleranceField;
        }
        set
        {
            this.mToleranceField = value;
        }
    }

    /// <remarks/>
    public bool HighPrecision
    {
        get
        {
            return this.highPrecisionField;
        }
        set
        {
            this.highPrecisionField = value;
        }
    }

    /// <remarks/>
    public short LeftLongitude
    {
        get
        {
            return this.leftLongitudeField;
        }
        set
        {
            this.leftLongitudeField = value;
        }
    }

    /// <remarks/>
    public ushort WKID
    {
        get
        {
            return this.wKIDField;
        }
        set
        {
            this.wKIDField = value;
        }
    }

    /// <remarks/>
    public ushort LatestWKID
    {
        get
        {
            return this.latestWKIDField;
        }
        set
        {
            this.latestWKIDField = value;
        }
    }
}

