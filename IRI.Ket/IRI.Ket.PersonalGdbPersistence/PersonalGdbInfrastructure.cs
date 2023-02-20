using IRI.Ket.PersonalGdbPersistence.Model;
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

    //var query = FormattableString.Invariant(
    //         @$"SELECT   Name, PhysicalName, Path, DatasetSubtype1, DatasetSubtype2, DatasetInfo1, Definition
    //                FROM    GDB_Items
    //                WHERE   (Definition LIKE '<DEFeatureDataset%') OR
    //                        (Definition LIKE '<DEFeatureClassInfo%')");


    private static List<GdbItem> GetGdbItems(OleDbConnection connection, string definitionStartsWith)
    {
        //< DEFeatureDataset
        try
        {
            var query = FormattableString.Invariant(
                @$"SELECT   Name, PhysicalName, Path, DatasetSubtype1, DatasetSubtype2, DatasetInfo1, Definition
                    FROM    GDB_Items
                    WHERE   (Definition LIKE '{definitionStartsWith}%')");

            using (var cmd = new OleDbCommand(query, connection))
            {
                List<GdbItem> result = new List<GdbItem>();

                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        GdbItem gdbItem = new GdbItem()
                        {
                            Name = dataReader[nameof(GdbItem.Name)].ToString()!,
                            Path = dataReader[nameof(GdbItem.Path)].ToString()!,
                            PhysicalName = dataReader[nameof(GdbItem.PhysicalName)].ToString()!,
                            Definition = dataReader[nameof(GdbItem.Definition)].ToString()!,
                            DatasetInfo1 = dataReader[nameof(GdbItem.DatasetInfo1)]?.ToString(),
                            DatasetSubtype1 = dataReader[nameof(GdbItem.DatasetSubtype1)]?.ToString(),
                            DatasetSubtype2 = dataReader[nameof(GdbItem.DatasetSubtype2)]?.ToString(),
                        };

                        result.Add(gdbItem);
                    }
                }

                return result;
            }
        }
        catch (Exception ex)
        {
            throw new NotImplementedException("PersonalGdbInfrastructure > GetSpatialReferenceSystems");
        }
    }

    public static List<GdbItem> GetFeatureDatasets(OleDbConnection connection)
    {
        // in 'GDB_Items' table, the 'Definition' column starts
        // with the '<DEFeatureDataset' strings for featureDatasets
        return GetGdbItems(connection, "<DEFeatureDataset");
    }

    public static List<GdbItem> DEFeatureClassInfo(OleDbConnection connection)
    {
        // in 'GDB_Items' table, the 'Definition' column starts
        // with the '<DEFeatureClassInfo' strings for featureDatasets
        return GetGdbItems(connection, "<DEFeatureClassInfo");
    }
}
