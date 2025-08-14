using IRI.Maptor.Jab.Common;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using IRI.Maptor.Extensions;

namespace IRI.Maptor.Res.TrajectoryCompression;

public class CustomSqlGeometry : Notifier
{
    private string _name;

    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            RaisePropertyChanged();
        }
    }



    public Guid Id { get; set; }

    public CustomSqlGeometry(string name, SqlGeometry geometry)
    {
        this.Id = Guid.NewGuid();

        this.Name = name;

        this.Geometry = geometry;

        var propertyInfos = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList();

        var result = new List<string>();

        for (int i = 0; i < propertyInfos.Count; i++)
        {
            //Debug.Print(i + " " + propertyInfos[i].Name);
            result.Add($"{propertyInfos[i].Name}: {propertyInfos[i].GetValue(this)}");
        }

        this.Info = string.Join(",\n", result);
    }

    public string OpenGisType
    {
        get { return Geometry.GetOpenGisType().ToString(); }
    }

    public bool HasM
    {
        get { return Geometry.HasM; ; }
    }

    public bool HasZ
    {
        get { return Geometry.HasZ; }
    }

    public string IsValidDetail
    {
        get { return Geometry.IsValidDetailed(); }
    }

    public string X
    {
        get { return Parse(Geometry.STX); }
    }

    public string Y
    {
        get { return Parse(Geometry.STY); }
    }

    public string M
    {
        get { return Parse(Geometry.M); }
    }

    public string Z
    {
        get { return Parse(Geometry.Z); }
    }

    public string Area
    {
        get { return Parse(Geometry.STArea()); }
    }

    public string IsClosed
    {
        get { return Parse(Geometry.STIsClosed()); }
    }

    public string IsEmpty
    {
        get { return Parse(Geometry.STIsEmpty()); }
    }

    public string IsRing
    {
        get { return Parse(Geometry.STIsRing()); }
    }

    public string IsSimple
    {
        get { return Parse(Geometry.STIsSimple()); }
    }

    public string IsValid
    {
        get { return Parse(Geometry.STIsValid()); }
    }

    public string Length
    {
        get { return Parse(Geometry.STLength()); }
    }

    public string NumberOfCurves
    {
        get { return Parse(Geometry.STNumCurves()); }
    }

    public string NumberOfGeometries
    {
        get { return Parse(Geometry.STNumGeometries()); }
    }

    public string NumberOfInteriorRing
    {
        get { return Parse(Geometry.STNumInteriorRing()); }
    }

    public string NumberOfPoints
    {
        get { return Parse(Geometry.STNumPoints()); }
    }

    public string EndPoint
    {
        get { return Geometry.STEndPoint().AsWkt(); }
    }

    public string StartPoint
    {
        get { return Geometry.STStartPoint().AsWkt(); }
    }

    public string Srid
    {
        get { return Parse(Geometry.STSrid); }
    }

    string Parse(SqlBoolean value) => value.IsNull ? "NULL" : value.Value.ToString();

    string Parse(SqlDouble value) => value.IsNull ? "NULL" : value.Value.ToString();

    string Parse(SqlInt32 value) => value.IsNull ? "NULL" : value.Value.ToString();

    private SqlGeometry _geometry;

    public SqlGeometry Geometry
    {
        get { return _geometry; }
        set
        {
            _geometry = value;
            RaisePropertyChanged();
            RaisePropertyChanged("Wkt");
        }
    }

    public string Wkt
    {
        get { return this.Geometry.AsWkt(); }
    }

    public string WktZm
    {
        get { return Geometry.AsWktZM(); }
    }

    private string _info;

    public string Info
    {
        get { return _info; }
        set
        {
            _info = value;
            RaisePropertyChanged();
        }
    }

}

