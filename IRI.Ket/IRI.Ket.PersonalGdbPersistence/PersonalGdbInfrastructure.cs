using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Sta.PersonalGdb;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.PersonalGdbPersistence;

public static class PersonalGdbInfrastructure
{
    internal static readonly string GdbSpatialRefTable = "GDB_SpatialRefs";

    internal static readonly string GdbGeomColumnsTable = "GDB_GeomColumns";

    public static Dictionary<int, SrsBase> GetSpatialReferenceSystems(OleDbConnection connection)
    {
        try
        {
            //connection.Open();
            var query = FormattableString.Invariant(@$"SELECT SRID, SRTEXT FROM {GdbSpatialRefTable}");

            var cmd = new OleDbCommand(query, connection);

            Dictionary<int, SrsBase> result = new Dictionary<int, SrsBase>();

            using (var dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    var srid = (int)dataReader["SRID"];
                    var srtext = (string)dataReader["SRTEXT"];

                    var srsbase = IRI.Sta.ShapefileFormat.Prj.EsriPrjFile.Parse(srtext).AsMapProjection();

                    result.Add(srid, srsbase);
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new NotImplementedException("PersonalGdbInfrastructure > GetSpatialReferenceSystems");
        }
    }
}
