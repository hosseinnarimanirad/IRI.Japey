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
public partial class DEFeatureDataset
{

    private string catalogPathField;

    private string nameField;

    private bool childrenExpandedField;

    private string datasetTypeField;

    private byte dSIDField;

    private bool versionedField;

    private bool canVersionField;

    private object configurationKeywordField;

    private decimal requiredGeodatabaseClientVersionField;

    private object extentField;

    private DEFeatureDatasetSpatialReference spatialReferenceField;

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
    public byte DSID
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
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Extent
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
    public DEFeatureDatasetSpatialReference SpatialReference
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
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class DEFeatureDatasetSpatialReference
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

