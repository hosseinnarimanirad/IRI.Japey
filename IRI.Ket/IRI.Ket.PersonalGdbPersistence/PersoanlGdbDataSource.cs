using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IRI.Extensions;
using IRI.Msh.Common.Primitives;
using IRI.Ket.Persistence.DataSources;
using IRI.Msh.Common.Model;
using IRI.Msh.CoordinateSystem.MapProjection;

namespace IRI.Ket.PersonalGdbPersistence;

public class PersoanlGdbDataSource : VectorDataSource<Feature<Point>, Point>// RelationalDbSource<Feature<Point>>
{
    protected BoundingBox _extent = BoundingBox.NaN;

    private const string _defaultSpatialColumnName = "SHAPE";

    public override BoundingBox Extent
    {
        get
        {
            if (double.IsNaN(_extent.Width) || double.IsNaN(_extent.Height) && _spatialColumnName != null)
            {
                //this._extent = GetGeometries().GetBoundingBox();
                //this._extent = GetBoundingBoxAndSrid();
                this.SetBoundingBoxAndSrid();
            }

            return _extent;
        }
        protected set
        {
            _extent = value;
        }
    }

    protected string _mdbFileName;

    protected string _tableName;

    protected string _spatialColumnName;

    protected string? _labelColumnName;

    private Func<Point, Point> _onTheFlyProj;

    public Action<IGeometryAware<Point>>? AddAction;

    public Action<int>? RemoveAction;

    public Action<IGeometryAware<Point>>? UpdateAction;

    public string? IdColumnName { get; set; }
    public override int Srid { get; protected set; }


    public PersoanlGdbDataSource(string mdbFileName, string tableName, string? spatialColumnName = null, string? labelColumnName = null, Func<Point, Point> onTheFlyProj = null)
    {
        this._mdbFileName = mdbFileName;

        this._tableName = tableName;

        this._spatialColumnName = spatialColumnName ?? _defaultSpatialColumnName;

        this._labelColumnName = labelColumnName;

        _onTheFlyProj = onTheFlyProj;

        SetBoundingBoxAndSrid();
    }

    //


    protected static string GetWhereClause(string spatialColumnName, BoundingBox boundingBox, int srid)
    {
        return FormattableString.Invariant($" {spatialColumnName}.STIntersects(GEOMETRY::STPolyFromText('{boundingBox.AsWkt()}',{srid})) = 1 ");
    }

    protected string MakeWhereClause(string whereClause)
    {
        return string.IsNullOrWhiteSpace(whereClause) ? string.Empty : FormattableString.Invariant($" WHERE ({whereClause}) ");
    }


    protected string MakeSelectCommand(string? whereClause, bool returnOnlyGeometry)
    {
        if (returnOnlyGeometry)
        {
            return FormattableString.Invariant($"SELECT {_spatialColumnName} FROM {_tableName} {MakeWhereClause(whereClause)}");
        }
        else
        {
            return FormattableString.Invariant($"SELECT * FROM {_tableName} {MakeWhereClause(whereClause)}");
        }
    }

    protected string MakeSelectCommandWithWkt(string wktGeometryFilter, bool returnOnlyGeometry)
    {
        if (string.IsNullOrWhiteSpace(wktGeometryFilter))
        {
            return MakeSelectCommand(string.Empty, returnOnlyGeometry);
        }

        if (returnOnlyGeometry)
        {
            return FormattableString.Invariant($@"
                DECLARE @filter GEOMETRY;
                SET @filter = GEOMETRY::STGeomFromText({wktGeometryFilter},{Srid});
                SELECT {_spatialColumnName} FROM {_tableName} WHERE {_spatialColumnName}.STIntersects(@filter)=1");
        }
        else
        {
            return FormattableString.Invariant($@"
                DECLARE @filter GEOMETRY;
                SET @filter = GEOMETRY::STGeomFromText({wktGeometryFilter},{Srid});
                SELECT * FROM {_tableName} WHERE {_spatialColumnName}.STIntersects(@filter)=1");
        }
    }

    protected string MakeSelectCommandWithWkb(byte[] wkbGeometryFilter, bool returnOnlyGeometry)
    {
        if (wkbGeometryFilter == null)
        {
            return MakeSelectCommand(string.Empty, returnOnlyGeometry);
        }

        var wkbString = Common.Helpers.HexStringHelper.ByteToHexBitFiddle(wkbGeometryFilter, true);

        if (returnOnlyGeometry)
        {
            return FormattableString.Invariant($@"
                DECLARE @filter GEOMETRY;
                SET @filter = GEOMETRY::STGeomFromWKB({wkbString},{Srid});
                SELECT {_spatialColumnName} FROM {_tableName} WHERE {_spatialColumnName}.STIntersects(@filter)=1");
        }
        else
        { 
            var geometry = $"GEOMETRY::STGeomFromWKB({wkbString},{Srid})";

            return FormattableString.Invariant($@" 
                SELECT * FROM {_tableName} WHERE {_spatialColumnName}.STIntersects({geometry})=1");
        }
    }



    //


    public void SetBoundingBoxAndSrid()
    {
        using (var conn = new OleDbConnection(GetConnectionString()))
        {
            conn.Open();

            var query = FormattableString.Invariant(
                        @$"SELECT        TableName, FieldName, ExtentLeft, ExtentBottom, ExtentRight, ExtentTop, SRID
                            FROM            GDB_GeomColumns
                            WHERE        (TableName = '{_tableName}')");

            var cmd = new OleDbCommand(query, conn);

            using (var dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    var minX = (double)dataReader["ExtentLeft"];
                    var maxX = (double)dataReader["ExtentRight"];
                    var minY = (double)dataReader["ExtentBottom"];
                    var maxY = (double)dataReader["ExtentTop"];
                    var srid = (int)dataReader["SRID"];

                    this.Srid = srid;

                    this._extent = new BoundingBox(minX, minY, maxX, maxY).Transform(_onTheFlyProj);
                    //return new BoundingBox(minX, minY, maxX, maxY);
                }
            }
        }

        //return BoundingBox.NaN;
    }

    private string GetConnectionString()
    {
        if (System.IO.File.Exists(_mdbFileName))
        {
            return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={_mdbFileName};Persist Security Info=False;";
        }
        else
        {
            return string.Empty;
        }
    }

    protected List<Geometry<Point>> SelectGeometries(string selectQuery)
    {
        List<Geometry<Point>> geometries = new List<Geometry<Point>>();

        using (var conn = new OleDbConnection(GetConnectionString()))
        {
            conn.Open();

            var query = $"SELECT {_spatialColumnName} FROM {_tableName}";

            var cmd = new OleDbCommand(query, conn);

            using (var dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    var esriByteGeometry = (byte[])dataReader[0];

                    var esriShape = IRI.Sta.PersonalGdb.EsriPGdbHelper.ParseToEsriShape(esriByteGeometry, 0);

                    geometries.Add(esriShape.AsGeometry());
                }
            }
        }

        return geometries;
    }

    protected FeatureSet<Point> Select(string selectQuery)
    {
        FeatureSet result = new FeatureSet() { Srid = Srid, Fields = new List<Field>(), Features = new List<Feature<Point>>() };

        using (var conn = new OleDbConnection(GetConnectionString()))
        {
            var command = new OleDbCommand(selectQuery, conn);

            conn.Open();

            using (var dataReader = command.ExecuteReader())
            {
                int geoIndex = -1;

                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    var type = dataReader.GetFieldType(i);

                    //if (type != typeof(SqlGeometry))
                    if (dataReader.GetName(0) != "Shape")
                    {
                        result.Fields.Add(new Field() { Name = dataReader.GetName(i), Type = type.ToString() });
                    }
                    else
                    {
                        geoIndex = i;
                    }
                }

                if (!dataReader.HasRows)
                {
                    return result;
                }

                while (dataReader.Read())
                {
                    var dict = new Dictionary<string, object>();

                    var feature = new Feature<Point>();

                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        var fieldName = dataReader.GetName(i);

                        while (dict.Keys.Contains(fieldName))
                        {
                            fieldName = $"{fieldName}_";
                        }

                        if (dataReader.IsDBNull(i))
                        {
                            dict.Add(fieldName, null);
                        }
                        else
                        {
                            //if (dataReader[i] is SqlGeometry)
                            if (i == geoIndex)
                            {
                                var esriByteGeometry = (byte[])dataReader[i];

                                var esriShape = IRI.Sta.PersonalGdb.EsriPGdbHelper.ParseToEsriShape(esriByteGeometry, 0);

                                feature.TheGeometry = esriShape.AsGeometry().Transform(_onTheFlyProj, SridHelper.WebMercator);
                            }
                            else
                            {
                                dict.Add(fieldName, dataReader[i]);
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(IdColumnName))
                    {
                        feature.Id = (int)dict[IdColumnName];
                    }

                    feature.Attributes = dict;

                    result.Features.Add(feature);
                }
            }
        }

        return result;
    }


    protected override Feature<Point> ToFeatureMappingFunc(Feature<Point> geometryAware)
    {
        return geometryAware;
    }


    public override FeatureSet<Point> GetAsFeatureSet(Geometry<Point>? geometry)
    {
        if (geometry is not null)
        {
            var selectQuery = MakeSelectCommandWithWkb(geometry.AsWkb(), false);

            return Select(selectQuery);
        }
        else
        {
            return Select(MakeSelectCommand(null, false));
        }
    }

    public override List<Feature<Point>> GetGeometryAwares(Geometry<Point>? geometry)
    {
        return GetAsFeatureSet(geometry).Features;
    }

    public override void Add(Feature<Point> newValue)
    {
        throw new NotImplementedException();
    }

    public override void Remove(Feature<Point> value)
    {
        throw new NotImplementedException();
    }

    public override void Update(Feature<Point> newValue)
    {
        throw new NotImplementedException();
    }

    public override void SaveChanges()
    {
        throw new NotImplementedException();
    }
}
