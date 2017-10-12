using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using IRI.Ket.ShapefileFormat.EsriType;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using IRI.Ket.ShapefileFormat.Reader;
using IRI.Ham.CoordinateSystem.MapProjection;

namespace IRI.Ket.ShapefileFormat
{

    public static class Shapefile
    {
        //private static System.ComponentModel.License license = null;

        public static Task<IShapeCollection> ReadAsync(string fileName)
        {
            return Task.Run(() => { return Read(fileName); });
        }

        public static Task<List<IShape>> ProjectAsync(string shpFileName, CoordinateReferenceSystemBase targetCrs)
        {
            return Task.Run(() => Project(shpFileName, targetCrs));
        }

        public static IShapeCollection Read(string shpFileName)
        {
            //System.ComponentModel.LicenseManager.Validate(typeof(Shapefile));

            if (!System.IO.File.Exists(shpFileName))
            {
                throw new System.IO.FileNotFoundException();
            }

            MainFileHeader MainHeader;

            using (System.IO.FileStream shpStream = new System.IO.FileStream(shpFileName, System.IO.FileMode.Open))
            {
                using (System.IO.BinaryReader shpReader = new System.IO.BinaryReader(shpStream))
                {
                    MainHeader = new MainFileHeader(shpReader.ReadBytes(ShapeConstants.MainHeaderLengthInBytes));
                }
            }

            switch (MainHeader.ShapeType)
            {
                case ShapeType.NullShape:
                    throw new NotImplementedException();

                case ShapeType.Point:
                    Reader.PointReader reader01 = new Reader.PointReader(shpFileName);
                    return reader01.elements;

                case ShapeType.PolyLine:
                    Reader.PolyLineReader reader02 = new Reader.PolyLineReader(shpFileName);
                    return reader02.elements;

                case ShapeType.Polygon:
                    Reader.PolygonReader reader03 = new Reader.PolygonReader(shpFileName);
                    return reader03.elements;

                case ShapeType.MultiPoint:
                    Reader.MultiPointReader reader04 = new Reader.MultiPointReader(shpFileName);
                    return reader04.elements;

                case ShapeType.PointZ:
                    Reader.PointZReader reader05 = new Reader.PointZReader(shpFileName);
                    return reader05.elements;

                case ShapeType.PolyLineZ:
                    Reader.PolyLineZReader reader06 = new Reader.PolyLineZReader(shpFileName);
                    return reader06.elements;

                case ShapeType.PolygonZ:
                    Reader.PolygonZReader reader07 = new Reader.PolygonZReader(shpFileName);
                    return reader07.elements;

                case ShapeType.MultiPointZ:
                    Reader.MultiPointZReader reader08 = new Reader.MultiPointZReader(shpFileName);
                    return reader08.elements;

                case ShapeType.PointM:
                    Reader.PointMReader reader09 = new Reader.PointMReader(shpFileName);
                    return reader09.elements;

                case ShapeType.PolyLineM:
                    Reader.PolyLineMReader reader10 = new Reader.PolyLineMReader(shpFileName);
                    return reader10.elements;

                case ShapeType.PolygonM:
                    Reader.PolygonMReader reader11 = new Reader.PolygonMReader(shpFileName);
                    return reader11.elements;

                case ShapeType.MultiPointM:
                    Reader.MultiPointMReader reader12 = new Reader.MultiPointMReader(shpFileName);
                    return reader12.elements;

                case ShapeType.MultiPatch:
                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }


        }

        public static MainFileHeader GetFileHeader(string fileName)
        {
            MainFileHeader MainHeader;

            using (System.IO.FileStream shpStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open))
            {
                using (System.IO.BinaryReader shpReader = new System.IO.BinaryReader(shpStream))
                {
                    MainHeader = new MainFileHeader(shpReader.ReadBytes(ShapeConstants.MainHeaderLengthInBytes));

                    shpReader.Close();

                    shpStream.Close();
                }
            }

            return MainHeader;
        }

        public static IRI.Ham.SpatialBase.BoundingBox CalculateBoundingBox(MainFileHeader header, params MainFileHeader[] headers)
        {
            double xMin = header.XMin;

            double xMax = header.XMax;

            double yMin = header.YMin;

            double yMax = header.YMax;

            if (headers.Length > 0)
            {
                for (int i = 0; i < headers.Length; i++)
                {
                    if (headers[i].XMin < xMin)
                    {
                        xMin = headers[i].XMin;
                    }
                    if (headers[i].YMin < yMin)
                    {
                        yMin = headers[i].YMin;
                    }
                    if (headers[i].XMax > xMax)
                    {
                        xMax = headers[i].XMax;
                    }
                    if (headers[i].YMax > yMax)
                    {
                        yMax = headers[i].YMax;
                    }
                }
            }
            return new IRI.Ham.SpatialBase.BoundingBox(xMin, yMin, xMax, yMax);
        }

        public static IRI.Ham.SpatialBase.BoundingBox CalculateBoundingBox(MainFileHeader[] headers)
        {
            if (headers.Length < 1)
                throw new NotImplementedException();

            double xMin = headers[0].XMin;

            double xMax = headers[0].XMax;

            double yMin = headers[0].YMin;

            double yMax = headers[0].YMax;

            for (int i = 1; i < headers.Length; i++)
            {
                if (headers[i].XMin < xMin)
                {
                    xMin = headers[i].XMin;
                }
                if (headers[i].YMin < yMin)
                {
                    yMin = headers[i].YMin;
                }
                if (headers[i].XMax > xMax)
                {
                    xMax = headers[i].XMax;
                }
                if (headers[i].YMax > yMax)
                {
                    yMax = headers[i].YMax;
                }
            }

            return new IRI.Ham.SpatialBase.BoundingBox(xMin, yMin, xMax, yMax);
        }

        public static string GetShxFileName(string shpFileName)
        {
            //string directoryName = System.IO.Path.GetDirectoryName(shpFileName);

            //string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(shpFileName);

            //return string.Format("{0}\\{1}.shx", directoryName, fileNameWithoutExtension);

            return System.IO.Path.ChangeExtension(shpFileName, "shx");
        }

        public static string GetDbfFileName(string shpFileName)
        {
            //string directoryName = System.IO.Path.GetDirectoryName(shpFileName);

            //string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(shpFileName);

            //return string.Format("{0}\\{1}.dbf", directoryName, fileNameWithoutExtension);

            return System.IO.Path.ChangeExtension(shpFileName, "dbf");
        }

        public static string GetPrjFileName(string shpFileName)
        {
            return System.IO.Path.ChangeExtension(shpFileName, "prj");
        }

        public static string GetCpgFileName(string shpFileName)
        {
            //string directoryName = System.IO.Path.GetDirectoryName(shpFileName);

            //string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(shpFileName);

            //return string.Format("{0}\\{1}.cpg", directoryName, fileNameWithoutExtension);

            return System.IO.Path.ChangeExtension(shpFileName, "cpg");
        }

        public static string GetIndexFileName(string shpFileName)
        {
            //string directoryName = System.IO.Path.GetDirectoryName(shpFileName);

            //string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(shpFileName);

            //return string.Format("{0}\\{1}.ind", directoryName, fileNameWithoutExtension);

            return System.IO.Path.ChangeExtension(shpFileName, "ind");
        }

        public static System.IO.FileMode GetMode(string fileName, bool overwrite)
        {
            return System.IO.File.Exists(fileName) && overwrite ? System.IO.FileMode.Create : System.IO.FileMode.CreateNew;
        }

        public static bool CheckAllNeededFilesExists(string fileName, bool checkPrjFileExist = false)
        {
            var result =
                System.IO.File.Exists(fileName) &&
                System.IO.File.Exists(GetShxFileName(fileName)) &&
                System.IO.File.Exists(GetDbfFileName(fileName));

            return checkPrjFileExist ? result && System.IO.File.Exists(GetPrjFileName(fileName)) : result;

        }

        public static List<IShape> Project(string shpFileName, CoordinateReferenceSystemBase targetCrs)
        {
            var sourcePrj = GetPrjFileName(shpFileName);

            if (!System.IO.File.Exists(sourcePrj))
            {
                throw new System.IO.FileNotFoundException($"prj file not found. {sourcePrj}");
            }

            var sourceCrs = new Prj.PrjFile(sourcePrj).AsMapProjection();

            var data = Read(shpFileName).ToList();

            return Project(data, sourceCrs, targetCrs);

            ////Old Method
            //var sourcePrj = GetPrjFileName(shpFileName);
            //if (!System.IO.File.Exists(sourcePrj))
            //{
            //    throw new System.IO.FileNotFoundException($"prj file not found. {sourcePrj}");
            //}
            //var sourceCrs = new Prj.PrjFile(sourcePrj).AsMapProjection();
            //var data = Read(shpFileName).ToList();
            //List<IShape> result = new List<IShape>(data.Count);
            //if (sourceCrs.Ellipsoid.AreTheSame(targetCrs.Ellipsoid))
            //{
            //    for (int i = 0; i < data.Count; i++)
            //    {
            //        var c1 = data[i].Transform(p => sourceCrs.ToGeodetic(p));
            //        result.Add(c1.Transform(p => targetCrs.FromGeodetic(p)));
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < data.Count; i++)
            //    {
            //        var c1 = data[i].Transform(p => sourceCrs.ToGeodetic(p));
            //        result.Add(c1.Transform(p => targetCrs.FromGeodetic(p, sourceCrs.Ellipsoid)));
            //    }
            //}
            //return result;
        }

        public static List<IShape> Project(List<IShape> values, string sourceEsriWktPrj, string targetEsriWktPrj)
        {
            var sourceSrs = Prj.PrjFile.Parse(sourceEsriWktPrj).AsMapProjection();

            var targetSrs = Prj.PrjFile.Parse(targetEsriWktPrj).AsMapProjection();

            return Project(values, sourceSrs, targetSrs);
        }

        public static List<IShape> Project(List<IShape> values, CoordinateReferenceSystemBase sourceSrs, CoordinateReferenceSystemBase targetSrs)
        {
            List<IShape> result = new List<IShape>(values.Count);

            if (sourceSrs.Ellipsoid.AreTheSame(targetSrs.Ellipsoid))
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var c1 = values[i].Transform(p => sourceSrs.ToGeodetic(p));

                    result.Add(c1.Transform(p => targetSrs.FromGeodetic(p)));
                }
            }
            else
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var c1 = values[i].Transform(p => sourceSrs.ToGeodetic(p));

                    result.Add(c1.Transform(p => targetSrs.FromGeodetic(p, sourceSrs.Ellipsoid)));
                }
            }

            return result;
        }

        public static void ProjectAndSaveAsShapefile(string sourceShapefileName, string targetShapefileName, CoordinateReferenceSystemBase targetCrs, bool overwrite = false)
        {
            if (!CheckAllNeededFilesExists(sourceShapefileName, true))
            {
                throw new NotImplementedException();
            }

            var result = Project(sourceShapefileName, targetCrs);
            //result.ToList().Select(i=>i.MinimumBoundingBox).ToList()
            SaveAsShapefile(targetShapefileName, result, false, targetCrs, overwrite);

            var destinationDbfFileName = GetDbfFileName(targetShapefileName);

            System.IO.File.Copy(GetDbfFileName(sourceShapefileName), destinationDbfFileName, overwrite);
        }

        public static void SaveAsShapefile(string shpFileName, IEnumerable<IShape> data, bool createEmptyDbf, CoordinateReferenceSystemBase coordinateReferenceSystem, bool overwrite = false)
        {
            Writer.ShpWriter.Write(shpFileName, data, createEmptyDbf, overwrite);

            var prj = GetPrjFileName(shpFileName);

            if (overwrite && System.IO.File.Exists(prj))
            {
                System.IO.File.Delete(prj);
            }

            coordinateReferenceSystem.AsEsriPrj().Save(prj);
        }

        #region Indexing
        //Indexing
        public static async Task CreateIndex(string shpFileName)
        {
            ShxReader shxFile = new ShxReader(Shapefile.GetShxFileName(shpFileName));

            var shapes = await ReadAsync(shpFileName);

            int numberOfRecords = shxFile.NumberOfRecords;

            List<Indexing.ShpIndex> indexes = new List<Indexing.ShpIndex>(numberOfRecords);

            for (int i = 0; i < numberOfRecords; i++)
            {
                int offset, contentLength;

                shxFile.GetRecord(i, out offset, out contentLength);

                if (contentLength == 2)
                    continue;

                indexes.Add(new Indexing.ShpIndex()
                {
                    RecordNumber = i,
                    MinimumBoundingBox = shapes[indexes.Count].MinimumBoundingBox
                });
            }

            Indexing.IndexIO.Write(GetIndexFileName(shpFileName), indexes);

        }

        public static async Task<IShapeCollection> Read(string shpFileName, IRI.Ham.SpatialBase.BoundingBox boundingBox)
        {
            var indexFileName = GetIndexFileName(shpFileName);

            if (!CheckAllNeededFilesExists(shpFileName) || !System.IO.File.Exists(indexFileName))
            {
                throw new NotImplementedException();
            }

            var shpIndexes = await Indexing.IndexIO.Read(indexFileName);

            var filtered = shpIndexes.Where(i => i.MinimumBoundingBox.Intersects(boundingBox)).ToList();

            var shxFile = new ShxReader(GetShxFileName(shpFileName));

            using (System.IO.FileStream shpStream = new System.IO.FileStream(shpFileName, System.IO.FileMode.Open))
            {
                using (var shpReader = new System.IO.BinaryReader(shpStream))
                {
                    switch (shxFile.MainHeader.ShapeType)
                    {
                        case ShapeType.Point:
                        case ShapeType.PointZ:
                        case ShapeType.PointM:
                            return ExtractPoints(shpReader, shxFile, filtered);

                        case ShapeType.MultiPoint:
                        case ShapeType.MultiPointZ:
                        case ShapeType.MultiPointM:
                            return ExtractMultiPoints(shpReader, shxFile, filtered);

                        case ShapeType.PolyLine:
                        case ShapeType.PolyLineZ:
                        case ShapeType.PolyLineM:
                            return ExtractPolyLines(shpReader, shxFile, filtered);

                        case ShapeType.Polygon:
                        case ShapeType.PolygonZ:
                        case ShapeType.PolygonM:
                            return ExtractPolygons(shpReader, shxFile, filtered);

                        case ShapeType.NullShape:
                        case ShapeType.MultiPatch:
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

        private static IShapeCollection ExtractPoints(System.IO.BinaryReader shpReader, ShxReader shxReader, List<Indexing.ShpIndex> indexes)
        {
            List<EsriPoint> geometries = new List<EsriPoint>(indexes.Count);

            for (int i = 0; i < indexes.Count; i++)
            {
                int offset, contentLength;

                shxReader.GetRecord(indexes[i].RecordNumber, out offset, out contentLength);

                geometries[i] = PointReader.Read(shpReader, offset, contentLength);
            }

            return new ShapeCollection<EsriPoint>(geometries);
        }

        private static IShapeCollection ExtractMultiPoints(System.IO.BinaryReader shpReader, ShxReader shxReader, List<Indexing.ShpIndex> indexes)
        {
            List<MultiPoint> geometries = new List<MultiPoint>(indexes.Count);

            for (int i = 0; i < indexes.Count; i++)
            {
                int offset, contentLength;

                shxReader.GetRecord(indexes[i].RecordNumber, out offset, out contentLength);

                geometries[i] = MultiPointReader.Read(shpReader, offset, contentLength);
            }

            return new ShapeCollection<MultiPoint>(geometries);
        }

        private static IShapeCollection ExtractPolyLines(System.IO.BinaryReader shpReader, ShxReader shxReader, List<Indexing.ShpIndex> indexes)
        {
            List<PolyLine> geometries = new List<PolyLine>(indexes.Count);

            for (int i = 0; i < indexes.Count; i++)
            {
                int offset, contentLength;

                shxReader.GetRecord(indexes[i].RecordNumber, out offset, out contentLength);

                geometries[i] = PolyLineReader.Read(shpReader, offset, contentLength);
            }

            return new ShapeCollection<PolyLine>(geometries);
        }

        private static IShapeCollection ExtractPolygons(System.IO.BinaryReader shpReader, ShxReader shxReader, List<Indexing.ShpIndex> indexes)
        {
            List<Polygon> geometries = new List<Polygon>(indexes.Count);

            for (int i = 0; i < indexes.Count; i++)
            {
                int offset, contentLength;

                shxReader.GetRecord(indexes[i].RecordNumber, out offset, out contentLength);

                geometries[i] = PolygonReader.Read(shpReader, offset, contentLength);
            }

            return new ShapeCollection<Polygon>(geometries);
        }

        #endregion

    }
}
