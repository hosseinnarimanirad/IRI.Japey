using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Primitives;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.DataSource;

public class PersoanlGdbDataSource : RelationalDbSource<SqlFeature>
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
                this._extent = GetBoundingBox();
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

    protected string _queryString;

    protected string _spatialColumnName;

    protected string _labelColumnName;

    public Action<ISqlGeometryAware> AddAction;

    public Action<int> RemoveAction;

    public Action<ISqlGeometryAware> UpdateAction;

    public string IdColumnName { get; set; }

    protected PersoanlGdbDataSource()
    {

    }

    public PersoanlGdbDataSource(string mdbFileName, string tableName, string spatialColumnName = _defaultSpatialColumnName, string labelColumnName = null)
    {
        this._mdbFileName = mdbFileName;

        this._tableName = tableName;

        this._spatialColumnName = spatialColumnName;

        this._labelColumnName = labelColumnName;

        if (spatialColumnName == null)
        {
            this.Extent = BoundingBox.NaN;
        }
        else
        {
            //IMPORTANT!
            //this.Extent = GetGeometries().GetBoundingBox();
        }

    }

    public static PersoanlGdbDataSource CreateForQueryString(string mdbFileName, string queryString, string spatialColumnName, string labelColumnName = null)
    {
        PersoanlGdbDataSource result = new PersoanlGdbDataSource(mdbFileName, null, spatialColumnName, labelColumnName)
        {
            _queryString = queryString,
        };

        return result;
    }


    private string GetTable()
    {
        return this._tableName ?? $" ({this._queryString}) A";
    }

    public BoundingBox GetBoundingBox()
    {
        //var query = string.Format(CultureInfo.InvariantCulture, "SELECT {0}.STEnvelope() FROM {1} ", _spatialColumnName, GetTable());
        var query = FormattableString.Invariant($"SELECT {_spatialColumnName}.STEnvelope() FROM {GetTable()} ");

        var envelopes = SelectGeometries(query);

        return IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.GetBoundingBoxFromEnvelopes(envelopes);
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

    protected List<SqlGeometry> SelectGeometries(string selectQuery, string connectionString = null)
    {
        if (connectionString == null)
            connectionString = _mdbFileName;

        List<SqlGeometry> geometries = new List<SqlGeometry>();

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

                    geometries.Add(esriShape.AsGeometry().AsSqlGeometry());
                }
            }
        } 

        return geometries;
    }






    public override void Add(ISqlGeometryAware newValue)
    {
        throw new NotImplementedException();
    }

    public override List<object> GetAttributes(string attributeColumn, string whereClause)
    {
        throw new NotImplementedException();
    }

    public override List<SqlFeature> GetFeatures(SqlGeometry geometry)
    {
        throw new NotImplementedException();
    }

    public override List<SqlGeometry> GetGeometries()
    {
        throw new NotImplementedException();
    }

    public override List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry)
    {
        throw new NotImplementedException();
    }

    public override SqlFeatureSet GetSqlFeatures()
    {
        throw new NotImplementedException();
    }

    public override void Remove(ISqlGeometryAware value)
    {
        throw new NotImplementedException();
    }

    public override void SaveChanges()
    {
        throw new NotImplementedException();
    }

    public override void Update(ISqlGeometryAware newValue)
    {
        throw new NotImplementedException();
    }

    public override void UpdateFeature(ISqlGeometryAware feature)
    {
        throw new NotImplementedException();
    }
}
