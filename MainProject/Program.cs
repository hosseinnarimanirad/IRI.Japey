// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using IRI.Sta.Algebra;
using IRI.Ket.Geometry;
using IRI.Sta.Statistics;
using IRI.Ket.DigitalTerrainModeling;
using IRI.Ket.Graph.GraphRepresentation;
using IRI.Ket.Graph;
using Drawing = System.Drawing;
using IRI.Ket.GsmGprsCommunication.AddressField;
using IRI.Ket.GsmGprsCommunication1;
using IRI.Ket.Spatial.Primitives;
using Sfc = IRI.Ket.Spatial;
using System.Diagnostics;
using IRI.Sta.Common.Mapping;
using spatial = IRI.Sta.Common.Primitives;
using IRI.Ket.SpatialExtensions;
using IRI.Ket.Common.Helpers;
using Microsoft.SqlServer.Types;
using System.Data;
using Newtonsoft.Json;
using IRI.Ket.Common.Extensions;
using IRI.Ket.Common.Model;
using IRI.Sta.Common.Model.GeoJson;
using IRI.Ket.SqlServerSpatialExtension.Extensions;
using System.Xml;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using IRI.Ket.ShapefileFormat.EsriType;
using IRI.Jab.Common.Model.Spatialable;
using IRI.Ket.SqlServerSpatialExtension.Helpers;

namespace MainProject
{

    class Program
    {

        [STAThread()]
        static void Main()
        {
            //TestGeolocation();
            GetTiles18();
            ////////var jsonString = System.IO.File.ReadAllText(@"E:\Data\Iran\Railway\networkblock.json");
            ////////// var jsonStringOne = System.IO.File.ReadAllText(@"C:\Users\Hossein\Desktop\New Text Document.txt");

            ////////var jsonStations = System.IO.File.ReadAllText(@"E:\Data\Iran\Railway\stations.json");

            ////////// var one = Newtonsoft.Json.JsonConvert.DeserializeObject<GeoJsonFeature>(jsonStringOne);
            //////////GeoJsonGeometry geo = Newtonsoft.Json.JsonConvert.DeserializeObject<GeoJsonGeometry>();

            ////////var parsedObject = Newtonsoft.Json.Linq.JObject.Parse(jsonStations);

            ////////var temp = parsedObject["features"].Select(f => JsonConvert.DeserializeObject<GeoJsonFeature>(f.ToString())).ToList();

            ////////GeoJsonExtensions.WriteAsShapefile(temp, @"C:\Users\Hossein\Desktop\stations.shp", true);

            ////////var c = temp.Count;



            //TestGeolocation();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
            //var temp = IRI.Ket.Common.Extensions.DateTimeExtensions._midValidDateTime.ToPersianAlphabeticDate();
            //new DateTime().ToPersianAlphabeticDate();

            ////////var wgsGeodeticPoint = new spatial.Point(51 + 22.0 / 60.0 + 12.72 / 3600.0, 29 + 14.0 / 60.0 + 14.68 / 3600.0);
            ////////var nahrawanGeodeticPoint = new spatial.Point(51 + 22.0 / 60.0 + 09.42 / 3600.0, 29 + 14.0 / 60.0 + 08.65 / 3600.0);

            ////////var lccNahrawanPoint = new spatial.Point(2119090.03, 823058.15);
            ////////var utmWgs84Point = new spatial.Point(535975, 3234346);


            ////////var result000 = IRI.Sta.CoordinateSystem.MapProjection.MapProjections.LccNiocWithClarcke1880Rgs.FromGeodetic(nahrawanGeodeticPoint);
            ////////var result0000 = IRI.Sta.CoordinateSystem.MapProjection.MapProjections.LccNiocWithClarcke1880Rgs.FromLocalGeodetic(nahrawanGeodeticPoint);

            ////////var result00 = IRI.Sta.CoordinateSystem.MapProjection.MapProjections.LccNahrawan.FromWgs84Geodetic(wgsGeodeticPoint);
            ////////var result01 = IRI.Sta.CoordinateSystem.MapProjection.MapProjections.LccNahrawan.FromGeodetic(nahrawanGeodeticPoint);
            ////////var result001 = IRI.Sta.CoordinateSystem.MapProjection.MapProjections.LccNahrawan.FromLocalGeodetic(nahrawanGeodeticPoint);



            ////////var result3 = IRI.Sta.CoordinateSystem.MapProjection.MapProjections.LccNahrawanIraq.FromGeodetic(nahrawanGeodeticPoint);
            ////////var result003 = IRI.Sta.CoordinateSystem.MapProjection.MapProjections.LccNahrawanIraq.FromLocalGeodetic(nahrawanGeodeticPoint);

            ////////var result4 = IRI.Sta.CoordinateSystem.MapProjection.MapProjections.LccFd58.FromGeodetic(nahrawanGeodeticPoint);
            ////////var result004 = IRI.Sta.CoordinateSystem.MapProjection.MapProjections.LccFd58.FromLocalGeodetic(nahrawanGeodeticPoint);

            ////////var temp = IRI.Sta.CoordinateSystem.MapProjection.MapProjects.UTMToGeodetic(utmWgs84Point, 39);


            ////////var result30 = IRI.Sta.CoordinateSystem.Transformation.ChangeDatum(wgsGeodeticPoint,
            ////////                IRI.Sta.CoordinateSystem.Ellipsoids.WGS84,
            ////////                IRI.Sta.CoordinateSystem.Ellipsoids.Clarke1880Rgs);

            ////////var nahrawanCalculatedClarke = $"{DegreeHelper.ToDms(result30.X)} - {DegreeHelper.ToDms(result30.Y)}";

            ////////var result20 = IRI.Sta.CoordinateSystem.Transformation.ChangeDatum(wgsGeodeticPoint,
            ////////                IRI.Sta.CoordinateSystem.Ellipsoids.WGS84,
            ////////                IRI.Sta.CoordinateSystem.Ellipsoids.Nahrawan);

            ////////var nahrawanCalculatedNahrawan = $"{DegreeHelper.ToDms(result20.X)} - {DegreeHelper.ToDms(result20.Y)}";

            ////////var result40 = IRI.Sta.CoordinateSystem.Transformation.ChangeDatum(wgsGeodeticPoint,
            ////////                IRI.Sta.CoordinateSystem.Ellipsoids.WGS84,
            ////////                IRI.Sta.CoordinateSystem.Ellipsoids.NahrawanIraq);

            ////////var nahrawanCalculatedNahrawanIraq = $"{DegreeHelper.ToDms(result40.X)} - {DegreeHelper.ToDms(result40.Y)}";

            ////////var result50 = IRI.Sta.CoordinateSystem.Transformation.ChangeDatum(wgsGeodeticPoint,
            ////////                IRI.Sta.CoordinateSystem.Ellipsoids.WGS84,
            ////////                IRI.Sta.CoordinateSystem.Ellipsoids.FD58);

            ////////var nahrawanCalculatedFD58 = $"{DegreeHelper.ToDms(result50.X)} - {DegreeHelper.ToDms(result50.Y)}";


            //var shapes = IRI.Ket.ShapefileFormat.Shapefile.Read(@"C:\Users\YAMORTAZAALI\Desktop\Export_Output.shp").Select(i => i.AsSqlGeometry(0).MakeValid().ParseToPathMarkup(4)).ToList();

            //var shapes = IRI.Ket.ShapefileFormat.Shapefile.Read(@"C:\Users\Hossein\Desktop\93.PolygonGeo\Province.shp").Select(i => i.AsSqlGeometry(0).MakeValid().ParseToPathMarkup(4)).ToList();
            //var dbf = IRI.Ket.ShapefileFormat.Dbf.DbfFile.Read(@"C:\Users\Hossein\Desktop\93.PolygonGeo\Province.dbf", "t", Encoding.UTF8, Encoding.Unicode, false);
            //var data = shapes.Zip(dbf.Select().Select(i => i[0].ToString()), (s1, s2) => Tuple.Create(s1, s2)).ToList();
            //List<string> result = new List<string>();
            //for (int i = 0; i < data.Count; i++)
            //{
            //    result.Add($"public string {data[i].Item2} {{get{{ return \"{data[i].Item1}\";}} }}");
            //}
            //System.IO.File.WriteAllLines(@"C:\Users\Hossein\Desktop\93.PolygonGeo\markup.txt", result);

            ////////var utmPoint = IRI.Sta.CoordinateSystem.MapProjection.MapProjects.GeodeticToUTM(new spatial.Point(52.34, 35.0));
            ////////var geodeticPoint0 = IRI.Sta.CoordinateSystem.MapProjection.MapProjects.UTMToGeodetic(utmPoint, 39);

            //ReadAdmin();
            //IRI.Ket.Data.Feeds.AdminFeeder.GetProvinces();
            var p1 = GetP(new spatial.Point(2702697, 777753), 39);
            var p2 = GetP(new spatial.Point(1695799, 1548481), 39);
            var p3 = GetP(new spatial.Point(2377182, 351233), 39);
            var p4 = GetP(new spatial.Point(1369727, 1120365), 39);

            Debug.WriteLine($"{p1.X},{p1.Y}");
            Debug.WriteLine($"{p2.X},{p2.Y}");
            Debug.WriteLine($"{p3.X},{p3.Y}");
            Debug.WriteLine($"{p4.X},{p4.Y}");

            Console.Read();
        }


        static void GetTiles18()
        {

            //var wm = IRI.Jab.Common.Model.Spatialable.EnvelopeMarkupLabelTriple.GetProvinces93Wm(null).First(i => i.Label == "اصفهان").GetBoundingBox(3857);

            //IRI.Sta.Common.Mapping.WebMercatorUtility.WriteWebMercatorBoundingBoxToGoogleTileRegions(@"C:\Users\Hossein\Desktop\New Text Document.txt", wm, 18);


            WebMercatorHelper.WriteWebMercatorBoundingBoxToGoogleTileRegionsAsShapefile(
                @"C:\Users\Hossein\Desktop\Iran13v3.shp",
                IRI.Sta.Common.Primitives.BoundingBoxes.IranWebMercatorBoundingBox, 13);
             
        }

        class MetroStation
        {
            public string Title { get; set; }

            public double Latitude { get; set; }

            public double Longitude { get; set; }

            public IEsriShape Point
            {
                get { return new IRI.Ket.ShapefileFormat.EsriType.EsriPoint(Longitude, Latitude); }
            }
        }

        static async void GoForStations()
        {
            var firstArray = GetMetros(@"C:\Users\Hossein\Desktop\Input.txt", "AIzaSyCGpzS-UhqBW3n2MJbqfPalJs1qD3-DnKE");


            System.IO.File.WriteAllLines("stations1.txt", firstArray.Select(r => $"{r.Longitude}; {r.Latitude}; {r.Title}"));

            var secondArray = GetMetros(@"C:\Users\Hossein\Desktop\Input2.txt", "AIzaSyAw46PA2twfhyid0R5Vm8igszi56V36enQ");

            var resultArray = firstArray.Union(secondArray).ToList();

            System.IO.File.WriteAllLines("stations.txt", resultArray.Select(r => $"{r.Longitude}; {r.Latitude}; {r.Title}"));
            IRI.Ket.ShapefileFormat.Shapefile.Save("stations.shp", resultArray.Select(r => r.Point), false, true);
            IRI.Ket.ShapefileFormat.Dbf.DbfFile.Write<MetroStation>(
                "stations.dbf",
                resultArray,
                new List<Func<MetroStation, object>>()
                {
                    m=>m.Title
                },
                new List<IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor>()
                {
                    IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetStringField("station title",255)
                },
                Encoding.UTF8,
                true);
        }

        static List<MetroStation> GetMetros(string fileName, string key)
        {
            var lines = System.IO.File.ReadAllLines(fileName).Select(l => l.Split('\t')).ToList();

            List<MetroStation> resultArray = new List<MetroStation>();

            foreach (var item in lines)
            {
                var id = item[0]?.ToString();
                var field1 = item[1]?.ToString();
                var field2 = item[2]?.ToString();
                var field3 = item[3]?.ToString();
                //var field4 = item[4]?.ToString();
                //var field5 = item[5]?.ToString();

                var fields = (new List<string>() { field1, field2, field3 }).Where(f => !string.IsNullOrWhiteSpace(f));

                var query = string.Join(",", fields);

                double Longitude, Latitude, XWebMercator, YWebMercator;

                try
                {
                    var result = IRI.Ket.Common.Service.Google.GooglePlacesService.Search(query, key);

                    if (!result.IsFailed && result.Result.Results.FirstOrDefault() != null)
                    {
                        var wgs84 = result.Result?.Results?.FirstOrDefault()?.AsGeodeticPoint();

                        var webMercator = result.Result.Results.First().AsWebMercatorPoint();

                        Longitude = wgs84.X;

                        Latitude = wgs84.Y;

                        XWebMercator = webMercator.X;

                        YWebMercator = webMercator.Y;

                        resultArray.Add(new MetroStation() { Latitude = Latitude, Longitude = Longitude, Title = field3 });
                        //newRow["LocationWgs84Wkb"] = SqlGeography.Point(wgs84.Y, wgs84.X, IRI.Sta.Common.CoordinateSystems.MapProjection.SridHelper.GeodeticWGS84).STAsBinary().Value;

                        //newRow["LocationWmWkb"] = SqlGeometry.Point(webMercator.X, webMercator.Y, IRI.Sta.Common.CoordinateSystems.MapProjection.SridHelper.WebMercator).STAsBinary().Value;
                    }
                    else
                    {
                        resultArray.Add(new MetroStation() { Latitude = 20, Longitude = 20, Title = field3 });
                    }

                }
                catch
                {

                }
            }

            return resultArray;
        }

        static async void GoForZamin()
        {
            //var max = 1860;

            WebClient client = new WebClient();

            //client.Headers.Add(HttpRequestHeader.ContentType, contentType);
            client.Headers.Add(HttpRequestHeader.UserAgent, "application!");

            client.Encoding = Encoding.UTF8;

            //Regex aTagRegex = new Regex("<(a|link).*?href=(\"|')(.+?)(\"|').*?>");
            //Regex aTagRegex = new Regex("href=['\"]([^ '\"]+?)['\"]");
            Regex aTagRegex = new Regex("href=\"(.*)\">");


            for (int i = 0; i < 125; i++)
            {
                //i = 111;

                System.IO.File.AppendAllText(@"E:\New folder\log2.txt", $"{System.Environment.NewLine}processing page {i}");

                var temp = 15 * i;

                var url = $"http://gsi.ir/fa/Maps/ListView/1/{temp}/All/All/All/All";


                var stringResult = client.DownloadString(url);


                //Regex r = new Regex("<(a|link).*?href=(\"|')(.+?)(\"|').*?>");

                //Regex r2 = new Regex(@"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)");
                //Regex r2 = new Regex("http://([\\w+?\\.\\w+])+([ا-یa-zA-Z0-9آء\\s\\~\\!\\@\\#\\$\\|\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);
                Regex r2 = new Regex("ShowDetail(.*)html'");


                var downloadPages = r2.Matches(stringResult).Cast<Match>().Where(a => a.Value.Contains("http://gsi.ir/fa/Maps/MoreInfo"))
                    .Select(a => a.Value.Replace("ShowDetail(", string.Empty).Replace("\'", string.Empty))
                    .ToList();


                foreach (var item in downloadPages)
                {
                    try
                    {

                        System.Threading.Thread.Sleep(500);

                        System.IO.File.AppendAllText(@"E:\New folder\log2.txt", $"{System.Environment.NewLine}processing item {i} {temp} {item}");

                        var page = client.DownloadString(item);

                        var matches = aTagRegex.Matches(page).Cast<Match>()
                            .Where(a => a.Value?.Contains("http://gsi.ir/Files/Maps-PackFile") == true)
                            .Select(a => a.Value?.ToString().Replace("href=", string.Empty).Replace("\"", string.Empty).Replace("class=HrefPackFile>", string.Empty)?.Trim())
                            .ToList();

                        if (matches.Count < 1)
                        {
                            continue;
                        }

                        if (matches.Count > 1)
                        {
                            System.IO.File.AppendAllText(@"E:\New folder\log2.txt", $"{System.Environment.NewLine}MORE MATCHES AT {i} {temp} {item}");
                        }

                        var index = item.LastIndexOf('/');

                        var name = item.Substring(index).Replace("/", "").Replace(".html", "");

                        var directory = CleanDirectory($@"E:\New folder\{name}").Replace(":", ".");

                        var fileNameIndex = matches.First().LastIndexOf('/');

                        var fileName = matches.First().Substring(fileNameIndex).Replace("/", "");

                        var fullFileName = CleanFileName(System.IO.Path.Combine(directory, fileName));

                        if (System.IO.File.Exists(fullFileName))
                        {
                            continue;
                        }

                        System.Threading.Thread.Sleep(3000);

                        var downloadFile = client.DownloadData(matches.First());

                        if (!System.IO.Directory.Exists(directory))
                        {
                            System.IO.Directory.CreateDirectory(directory);


                            var di = new DirectoryInfo(directory);
                            di.Attributes &= ~FileAttributes.ReadOnly;


                        }
                        System.IO.File.WriteAllBytes((fullFileName), downloadFile);

                        var lengthCount = matches.Count;
                    }
                    catch (Exception ex)
                    {
                        System.IO.File.AppendAllText(@"E:\New folder\log2.txt", $"{System.Environment.NewLine}Exception at {i} {i * 15} {item}");
                        var exc = ex.Message;
                    }
                }



                var length = downloadPages.Count;
            }
        }

        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        private static string CleanDirectory(string directory)
        {
            return Path.GetInvalidPathChars().Aggregate(directory, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        //private List<string> ExtractAllAHrefTags(HtmlDocument htmlSnippet)
        //{
        //    List<string> hrefTags = new List<string>();

        //    foreach (var link in htmlSnippet.DocumentNode.SelectNodes("//a[@href]"))
        //    {
        //        HtmlAttribute att = link.Attributes["href"];
        //        hrefTags.Add(att.Value);
        //    }

        //    return hrefTags;
        //}


        static async void LoadLive()
        {
            //var geo = fields.Select(f => f.AsSqlGeometry()).ToList();

            //var count = geo.Count;
        }

        static async void TestGeolocation()
        {
            var location = await IRI.Ket.Common.Service.Google.GoogleMapsGeolocationService.GetLocationAsync("AIzaSyCC43GEQQv3cqK6Aea7XlHQb0bhjt3FQpE");
        }

        static spatial.Point GetP(spatial.Point point, int zone)
        {
            var geo = IRI.Sta.CoordinateSystem.MapProjection.DefaultMapProjections.LccNiocWithClarcke1880Rgs.ToWgs84Geodetic(point);

            return IRI.Sta.CoordinateSystem.MapProjection.MapProjects.GeodeticToUTM(geo, IRI.Sta.CoordinateSystem.Ellipsoids.WGS84, zone, true);
        }


        static void ReadAdmin()
        {
            var connectionString = "data source=DESKTOP-C459ACH;integrated security=true;initial catalog = MyGeodatabase";
            IRI.Ket.DataManagement.DataSource.SqlServerDataSource s = new IRI.Ket.DataManagement.DataSource.SqlServerDataSource(connectionString, "TAGHSIMAT93WM", "[Shape]");
            var table = s.GetEntireFeature();

            List<prov> result = new List<prov>();

            foreach (DataRow item in table.Rows)
            {
                result.Add(new prov()
                {
                    Ostan = item["Ostan"].ToString(),
                    Shahrestan = item["Shahrestan"].ToString(),
                    Bakhsh = item["Bakhsh"].ToString(),
                    Markaz = item["Markaz"].ToString(),
                    BorderGeo = (SqlGeometry)item["Shape"],
                    //Border = Convert.ToBase64String(((SqlGeometry)item["Shape"]).AsWkb()),
                    Envelope = Convert.ToBase64String(((SqlGeometry)item["Shape"]).STEnvelope().AsWkb()),

                });
            }

            var ostanGroups = result.GroupBy(i => i.Ostan);

            List<prov> ostanha = new List<prov>();

            foreach (var ostan in ostanGroups)
            {
                var geo = IRI.Ket.SqlServerSpatialExtension.Utility.Union(ostan.Select(j => j.BorderGeo).ToList());
                //ostanha.Add(new prov() { Ostan = ostan.Key, Border = Convert.ToBase64String(geo.AsWkb()), BorderGeo = geo });
                ostanha.Add(new prov() { Ostan = ostan.Key, Envelope = Convert.ToBase64String(geo.STEnvelope().AsWkb()), BorderGeo = geo });
            }

            //var shahrestanGroups = result.GroupBy(i => new { i.Shahrestan, i.Ostan });

            //List<prov> shahrestanha = new List<prov>();

            //foreach (var shahrestan in shahrestanGroups)
            //{
            //    var geo = IRI.Ket.SqlServerSpatialExtension.Utility.Union(shahrestan.Select(j => j.BorderGeo).ToList());
            //    shahrestanha.Add(new prov() { Ostan = shahrestan.Key.Ostan, Shahrestan = shahrestan.Key.Shahrestan, Border = Convert.ToBase64String(geo.AsWkb()), BorderGeo = geo });
            //}

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(ostanha);
            System.IO.File.WriteAllText("ostanEnvelope93Wm.txt", json);
        }

        public class prov
        {
            public string Ostan { get; set; }

            public string Shahrestan { get; set; }

            public string Bakhsh { get; set; }

            public string Markaz { get; set; }

            public string Border { get; set; }

            public string Envelope { get; set; }

            [JsonIgnore]
            public SqlGeometry BorderGeo { get; set; }
        }

        private static void GetCapabilities()
        {
            //IRI.Standards.OGC.WMS.WMSCapabilities value = new IRI.Standards.OGC.WMS.WMSCapabilities();
            //XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //ns.Add("xlink", "http://www.w3.org/1999/xlink");
            //ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");


            //value.Service.OnlineResource = new IRI.Standards.OGC.WMS.OnlineResource() { href = "http://mesonet.agron.iastate.edu/cgi-bin/wms/iowa/rainfall.cgi?" };
            //XmlSerializer serializer = new XmlSerializer(typeof(IRI.Standards.OGC.WMS.WMSCapabilities));
            //serializer.Serialize(System.IO.File.Open(@"C:\Users\Hossein\Desktop\N.txt", System.IO.FileMode.Create), value, ns);


            //var tempResult = serializer.Deserialize(System.IO.File.Open(@"C:\Users\Hossein\Desktop\sample3.txt", FileMode.Open));

            //var fileName = @"C:\Users\Hossein\Desktop\sample2.txt";
            //XmlDocument xdoc;
            //WMSCapabilities result;

            //using (var xmlString = new StringReader(System.IO.File.ReadAllText(fileName).Replace("&", "&amp;")))
            //{
            //    var settings = new XmlReaderSettings();
            //    settings.NameTable = new NameTable();

            //    var manager = new XmlNamespaceManager(settings.NameTable);
            //    //xmlns: xsi = "http://www.w3.org/2001/XMLSchema-instance" 
            //    //    xmlns: xsd = "http://www.w3.org/2001/XMLSchema"
            //    manager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            //    manager.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
            //    manager.AddNamespace("sld", "http://www.opengis.net/sld");
            //    manager.AddNamespace("ms", "http://mapserver.gis.umn.edu/mapserver");
            //    manager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
            //    manager.AddNamespace("ns2", "http://www.w3.org/1999/xlink");
            //    XmlParserContext context = new XmlParserContext(null, manager, null, XmlSpace.Default);
            //    using (var xmlReader = XmlReader.Create(xmlString, settings, context))
            //    {
            //        xdoc = new XmlDocument();
            //        xdoc.Load(xmlReader);

            //        var xml = Do(xdoc);

            //        //result = (WMSCapabilities)new XmlSerializer(typeof(WMSCapabilities)).Deserialize(new XmlNodeReader(xdoc.DocumentElement));

            //        using (var reader = new StringReader(xml))
            //        {
            //            XmlRootAttribute xRoot = new XmlRootAttribute();
            //            //xdoc.Save(@"C:\Users\Hossein\Desktop\N2.txt");
            //            //xRoot.ElementName = "WMS_Capabilities";
            //            //xRoot.Namespace = "http://www.opengis.net/wms";
            //            //xRoot.IsNullable = false;
            //            //serializer = new XmlSerializer(typeof(WMSCapabilities), xRoot);
            //            result = (WMSCapabilities)serializer.Deserialize(reader);
            //        }
            //    }
            //}


        }

        const string outputFolder = @"E:\NIOC\Joint Study\GeoPhotos";

        private static void ProcessFolder(string folder)
        {
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(folder);

            var directories = info.GetDirectories();

            foreach (var directory in directories)
            {
                ProcessFolder(directory.FullName);
            }

            var files = info.EnumerateFiles("*.*", System.IO.SearchOption.TopDirectoryOnly)
                            .Where(s => s.Name.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase) || s.Name.EndsWith(".jpeg", StringComparison.InvariantCultureIgnoreCase));

            foreach (var file in files)
            {
                var bitmap = new System.Drawing.Bitmap(file.FullName);
                var latitude = IRI.Ket.Common.Helpers.ImageHelper.GetLatitude(bitmap);

                if (latitude.HasValue)
                {
                    System.IO.File.Copy(file.FullName, $"{outputFolder}\\{file.Name}.{DateTime.Now.Ticks}.jpg");
                }
                bitmap.Dispose();
            }

            Console.WriteLine($"{ folder} processed");
        }

        private static void GoForDbf()
        {
            //string file = @"C:\Users\Hossein\Desktop\pnt940721\pnt940721\error_index_contour.dbf";
            string file = @"C:\Users\Hossein\Desktop\pnt940721\pnt940721\error_water_reserv.dbf";
            var result = IRI.Ket.ShapefileFormat.Dbf.DbfFile.Read(file, "tableName", Encoding.Unicode, Encoding.GetEncoding(1256), true);
        }

        public class Item
        {
            public override string ToString()
            {
                return $"Id: {Id}";
            }
            public Item()
            {
                this.Children = new List<Item>();
            }

            public int Id { get; set; }
            public string ParentId { get; set; }
            public string Name { get; set; }
            public DateTime Created { get; set; }
            public string Content { get; set; }
            public string UserId { get; set; }
            public List<Item> Children { get; set; }

            public IEnumerable<Item> GetItems()
            {

                var result = Children.SelectMany(i => i.GetItems()).ToList();

                result.Add(this);

                return result;
            }
        }

        private static void TestGoogleMap()
        {
            Debug.Print("level \t number of tiles \t mapsize \t resolution Tab \t resolution Teh \t resolution Ban \t resolution Eq \t tile size Tab \t tile size Teh \t tile size Ban \t tile size Eq \t scale Tab \t scale Teh \t scale Ban \t scale Eq");

            for (int i = 0; i < 24; i++)
            {
                var mapSize = WebMercatorUtility.CalculateScreenSize(i);

                var resolutionEq = WebMercatorUtility.CalculateGroundResolution(i, 0);

                var resolutionTab = WebMercatorUtility.CalculateGroundResolution(i, 38.05);

                var resolutionTeh = WebMercatorUtility.CalculateGroundResolution(i, 35.72);

                var resolutionBan = WebMercatorUtility.CalculateGroundResolution(i, 27.2);

                var scaleEq = WebMercatorUtility.CalculateMapScale(i, 0);

                var scaleTab = WebMercatorUtility.CalculateMapScale(i, 38.05);

                var scaleTeh = WebMercatorUtility.CalculateMapScale(i, 35.72);

                var scaleBan = WebMercatorUtility.CalculateMapScale(i, 27.2);

                Debug.Print("{0} \t {1} \t {2} \t {3} \t {4} \t {5} \t {6} \t {7} \t {8} \t {9} \t {10} \t {11} \t {12} \t {13} \t {14}",
                    i,
                    Math.Pow(2, 2 * i),
                    mapSize,
                    resolutionTab, resolutionTeh, resolutionBan, resolutionEq,
                    256 * resolutionTab, 256 * resolutionTeh, 256 * resolutionBan, 256 * resolutionEq,
                    1.0 / scaleTab, 1.0 / scaleTeh, 1.0 / scaleBan, 1.0 / scaleEq);
            }
        }

        #region STANFORDClass-Design & Analysis of Algorithms II

        //*****************************WEEK 6 ***************************************//

        static void Do2SAT()
        {
            //string fileName = @"D:\Online Courses\STANFORD.D&AOfAlgorithmsII\ProblemAssignments\WEEK6.2sat5.txt";
            ////string fileName = @"C:\Users\Hossein\Desktop\notSExample5.txt";
            ////string fileName = @"C:\Users\Hossein\Desktop\sExample4.txt";
            string fileName = @"C:\Users\Hossein\Desktop\notSExample7.txt";

            string[] lines = System.IO.File.ReadAllLines(fileName);

            long n = long.Parse(lines[0]);

            IRI.Ket.Graph.GraphRepresentation.AdjacencyList<int, int> graph = new AdjacencyList<int, int>(2 * (int)n);

            //328507
            //start: 1:04
            for (int i = 1; i <= n; i++)
            {
                graph.AddNode(i);
                graph.AddNode(-i);
            }
            //end: 1h:56':00" !!
            DateTime t00 = DateTime.Now;

            for (int i = 0; i < n; i++)
            {
                string[] tempS = lines[i + 1].Split(' ');

                int first = int.Parse(tempS[0]);

                int second = int.Parse(tempS[1]);

                graph.AddDirectedEdgeUnsafly(-first, second, 1);

                graph.AddDirectedEdgeUnsafly(-second, first, 1);
            }
            //5"
            TimeSpan dt0 = DateTime.Now - t00;

            int nE = graph.NumberOfEdges;

            int nN = graph.NumberOfNodes;

            DateTime t0 = DateTime.Now;
            //IRI.Ket.Graph.Search.FastDepthFirstSearch<int, int> dfs = new IRI.Ket.Graph.Search.FastDepthFirstSearch<int, int>(graph, graph[0]);
            List<LinkedList<int>> result = IRI.Ket.Graph.Search.FastDepthFirstSearch<int, int>.GetStronglyConnectedComponents(graph);
            //22"
            TimeSpan dt1 = DateTime.Now - t0;

            t0 = DateTime.Now;

            bool isSatisfied = IsSatisfied(result);

            TimeSpan dt2 = DateTime.Now - t0;
        }

        private static bool IsSatisfied(List<LinkedList<int>> values)
        {
            foreach (LinkedList<int> item in values)
            {
                foreach (int item2 in item)
                {
                    if (item.Contains(-item2))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //*****************************WEEK 4 ***************************************//

        private static void TestFloydWarshall()
        {
            string fileName = @"C:\Users\Hossein\Desktop\g3.txt";

            string[] lines = System.IO.File.ReadAllLines(fileName);

            int n = int.Parse(lines[0].Split(' ')[0]);

            double[,] matrix = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        matrix[i, j] = 0;
                    }
                    else
                    {
                        matrix[i, j] = double.PositiveInfinity;
                    }
                }
            }

            for (int i = 1; i < lines.Length; i++)
            {
                List<int> edge = lines[i].Split(' ').ToList().ConvertAll<int>(input => int.Parse(input));

                matrix[edge[0] - 1, edge[1] - 1] = edge[2];
            }

            IRI.Ket.Graph.ShortestPaths.AllPairs.FloydWarshallProblem fwp = new IRI.Ket.Graph.ShortestPaths.AllPairs.FloydWarshallProblem(matrix);

            double shortest = IRI.Sta.Statistics.Statistics.GetMin(new Matrix(fwp.shortestPaths));
            double biggest = IRI.Sta.Statistics.Statistics.GetMax(new Matrix(fwp.shortestPaths));
            double mean = IRI.Sta.Statistics.Statistics.CalculateMean(new Matrix(fwp.shortestPaths));
        }

        private static void FloydWarshall()
        {
            double[,] matrix = {
                              {0,3,8,double.PositiveInfinity,-4},
                              {double.PositiveInfinity,0,double.PositiveInfinity,1,7},
                              {double.PositiveInfinity,4,0,double.PositiveInfinity,double.PositiveInfinity},
                              {2,double.PositiveInfinity,-5,0,double.PositiveInfinity},
                              {double.PositiveInfinity,double.PositiveInfinity,double.PositiveInfinity,6,0}
                              };

            IRI.Ket.Graph.ShortestPaths.AllPairs.FloydWarshallProblem fwp = new IRI.Ket.Graph.ShortestPaths.AllPairs.FloydWarshallProblem(matrix);


        }

        //*****************************WEEK 3 ***************************************//

        struct temp
        {
            public double value;
            public double weight;

            public temp(double value, double weight)
            {
                this.value = value;

                this.weight = weight;
            }
        }

        private static void KNAPSACK2()
        {
            string fileName = @"C:\Users\Hossein\Desktop\knapsack2.txt";

            string[] lines = System.IO.File.ReadAllLines(fileName);

            List<temp> temps = new List<temp>();

            for (int i = 1; i < lines.Length; i++)
            {
                temps.Add(new temp(double.Parse(lines[i].Split(' ')[0]), double.Parse(lines[i].Split(' ')[1])));
            }

            double value = IRI.Ket.CombinatorialOptimization.KnapsackProblemFast.Calculate(temps, double.Parse(lines[0].Split(' ')[0]),
                    val => val.value, val => val.weight);

            //javabe sahih: 2595819
        }

        private static void KNAPSACK1()
        {
            string fileName = @"C:\Users\Hossein\Desktop\knapsack1.txt";

            string[] lines = System.IO.File.ReadAllLines(fileName);

            List<temp> temps = new List<temp>();

            for (int i = 1; i < lines.Length; i++)
            {
                temps.Add(new temp(double.Parse(lines[i].Split(' ')[0]), double.Parse(lines[i].Split(' ')[1])));
            }

            double value = IRI.Ket.CombinatorialOptimization.KnapsackProblem.Calculate(temps, double.Parse(lines[0].Split(' ')[0]),
                    val => val.value, val => val.weight);

            //javabe sahih: 2493893


        }

        //*****************************WEEK 2 ***************************************//

        private static void CLUSTRING2()
        {
            string fileName = @"C:\Users\Hossein\Desktop\clustering2.txt";

            string[] lines = System.IO.File.ReadAllLines(fileName);

            IRI.Ket.Graph.GraphRepresentation.AdjacencyList<int, double> graph = new AdjacencyList<int, double>();

            List<int[]> nodes = new List<int[]>();

            DateTime t0 = DateTime.Now;

            for (int i = 1; i < lines.Length; i++)
            {
                string[] temp = lines[i].Split(' ').Take(24).ToArray();

                nodes.Add(Array.ConvertAll(temp, int.Parse));
                graph.AddNode(i - 1);
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = i + 1; j < nodes.Count; j++)
                {
                    int distance = CalculateHammingDistance(nodes[i], nodes[j]);

                    if (distance < 3)
                    {
                        graph.AddUndirectedEdge(i, j, distance);
                    }

                    //graph.AddDirectedEdge(i, j, CalculateHammingDistance(nodes[i], nodes[j]));
                }
            }


            IRI.Ket.Graph.Search.DepthFirstSearch<int, double> dfs = new IRI.Ket.Graph.Search.DepthFirstSearch<int, double>(graph, 0);

            TimeSpan dt = DateTime.Now - t0;
            //7'30"
            List<List<int>> result = dfs.GetComponents();

            //IRI.Ket.Graph.Clustring.GreedyClustring<int, double> clustringProblem =
            //    new IRI.Ket.Graph.Clustring.GreedyClustring<int, double>(graph, (e1, e2) => e2.Connection.Weight.CompareTo(e1.Connection.Weight));

            //clustringProblem.Cluster(3, (d1, d2) => d1 > d2);

            //double result = clustringProblem.ClusterSpacing;
        }


        private static int CalculateHammingDistance(int[] first, int[] second)
        {
            if (first.Length != second.Length)
            {
                throw new NotImplementedException();
            }

            int result = 0;

            for (int i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i])
                {
                    result++;
                }
            }

            return result;
        }

        private static void CLUSTRING1()
        {
            string fileName = @"C:\Users\Hossein\Desktop\clustering1.txt";

            string[] lines = System.IO.File.ReadAllLines(fileName);

            IRI.Ket.Graph.GraphRepresentation.AdjacencyList<int, double> graph = new AdjacencyList<int, double>();

            for (int i = 1; i < lines.Length; i++)
            {
                string[] temp = lines[i].Split(' ');

                graph.AddUndirectedEdge(int.Parse(temp[0]), int.Parse(temp[1]), double.Parse(temp[2]));
            }

            IRI.Ket.Graph.Clustring.GreedyClustring<int, double> clustringProblem =
                new IRI.Ket.Graph.Clustring.GreedyClustring<int, double>(graph, (e1, e2) => e2.Connection.Weight.CompareTo(e1.Connection.Weight));

            clustringProblem.Cluster(4);

            double result = clustringProblem.ClusterSpacing;
        }

        //*****************************WEEK 1 ***************************************//
        private struct Job
        {
            public double weight, length;

            public Job(double weight, double length)
            {
                this.weight = weight;

                this.length = length;
            }

            public double Diff
            {
                get { return weight - length; }
            }

            public double Ratio
            {
                get { return weight / length; }
            }

        }

        private static int DiffCompare(Job j1, Job j2)
        {
            if (j1.Diff == j2.Diff)
            {
                return j2.weight.CompareTo(j1.weight);
            }
            else
            {
                return j2.Diff.CompareTo(j1.Diff);
            }
        }

        private static int RatioCompare(Job j1, Job j2)
        {
            return j2.Ratio.CompareTo(j1.Ratio);
        }

        private static double Calculate(Job[] jobs)
        {
            double length = 0;

            double result = 0;

            for (int i = 0; i < jobs.Length; i++)
            {
                length += jobs[i].length;

                result += length * jobs[i].weight;
            }

            return result;
        }

        public static void Do1()
        {
            string fileName = @"C:\Users\Hossein\Desktop\jobs.txt";

            Job[] jobs = new Job[10000];

            string[] lines = System.IO.File.ReadAllLines(fileName);

            for (int i = 1; i < lines.Length; i++)
            {
                string[] temp = lines[i].Split(' ');
                jobs[i - 1] = new Job(double.Parse(temp[0]), double.Parse(temp[1]));
            }

            IRI.Ket.DataStructure.SortAlgorithm.QuickSort(jobs, DiffCompare);

            double time1 = Calculate(jobs);


            jobs = new Job[10000];

            for (int i = 1; i < lines.Length; i++)
            {
                string[] temp = lines[i].Split(' ');

                jobs[i - 1] = new Job(double.Parse(temp[0]), double.Parse(temp[1]));
            }

            IRI.Ket.DataStructure.SortAlgorithm.QuickSort(jobs, RatioCompare);

            double time2 = Calculate(jobs);

            double valu = time1 - time2;
        }

        public static void Do2()
        {
            string fileName = @"C:\Users\Hossein\Desktop\edges.txt";

            string[] lines = System.IO.File.ReadAllLines(fileName);

            int numNodes = int.Parse(lines[0].Split(' ')[0]);
            int numEdges = int.Parse(lines[0].Split(' ')[1]);

            IRI.Ket.Graph.GraphRepresentation.AdjacencyList<int, double> graph = new AdjacencyList<int, double>(numNodes);

            for (int i = 1; i < lines.Length; i++)
            {
                string[] temp = lines[i].Split(' ');

                graph.AddUndirectedEdge(int.Parse(temp[0]), int.Parse(temp[1]), double.Parse(temp[2]));
            }

            IRI.Ket.Graph.MinimumSpanningTree2.PrimAlgorithm<int, double> p =
                new IRI.Ket.Graph.MinimumSpanningTree2.PrimAlgorithm<int, double>(graph);

            IRI.Ket.Graph.GraphRepresentation.AdjacencyList<int, double> p2 = p.GetMinimumSpanningTree();

            double result = p2.AggregateWeights(0, (d1, d2) => d1 + d2);
        }

        #endregion


        #region ShapefileReaderFarsiIssue

        static void TestDbfReader()
        {
            string filePath = @"D:\Data\Aja\SHP_Map\City_Poly.dbf";

            OpenFileDialog dialog = new OpenFileDialog() { Filter = "dBase IV|*.dbf" };
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            filePath = dialog.FileName;

            System.Data.DataTable table = IRI.Ket.ShapefileFormat.Dbf.DbfFile.Read(filePath, "myTable");
        }

        static void TestdbfReaderForFarsiColumns2()
        {
            string filePath = @"D:\Data\Aja\SHP_Map\City_Poly.dbf";

            OpenFileDialog dialog = new OpenFileDialog() { Filter = "dBase IV|*.dbf" };
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            filePath = dialog.FileName;

            System.IO.Stream s = new System.IO.FileStream(filePath, System.IO.FileMode.Open);

            System.IO.BinaryReader reader = new System.IO.BinaryReader(s);

            byte version = reader.ReadByte();
            string date = BitConverter.ToString(reader.ReadBytes(3));

            Int32 nOrecords = reader.ReadInt32();
            UInt16 headerLength = reader.ReadUInt16();
            UInt16 recordLength = reader.ReadUInt16();
            reader.ReadInt16();
            reader.ReadByte();
            byte flag = reader.ReadByte();
            reader.ReadInt32();
            reader.ReadBytes(8);
            byte mdx = reader.ReadByte();
            byte languageDriver = reader.ReadByte();

            reader.ReadBytes(2);

            if (((headerLength - 33) % 32 != 0))
            {
                throw new NotImplementedException();
            }

            int numberOfFields = (headerLength - 33) / 32;

            string[] fieldName = new string[numberOfFields];
            string[] fieldType = new string[numberOfFields];
            string[] fieldDataAddress = new string[numberOfFields];
            string[] fieldLength = new string[numberOfFields];
            string[] fieldDecimalCount = new string[numberOfFields];
            string[] fieldWorkAreaId = new string[numberOfFields];
            string[] flagForSetFields = new string[numberOfFields];
            string[] indexFieldFlag = new string[numberOfFields];

            for (int i = 0; i < numberOfFields; i++)
            {
                fieldName[i] = new string(Encoding.ASCII.GetChars(reader.ReadBytes(11)));
                fieldType[i] = new string(Encoding.ASCII.GetChars(reader.ReadBytes(1)));
                fieldDataAddress[i] = new string(Encoding.ASCII.GetChars(reader.ReadBytes(4)));
                fieldLength[i] = reader.ReadByte().ToString();
                fieldDecimalCount[i] = reader.ReadByte().ToString();
                reader.ReadBytes(2);
                fieldWorkAreaId[i] = reader.ReadByte().ToString();
                reader.ReadBytes(2);
                flagForSetFields[i] = reader.ReadByte().ToString();
                reader.ReadBytes(7);
                indexFieldFlag[i] = reader.ReadByte().ToString();
            }

            string terminator = reader.ReadByte().ToString("X");

            StringBuilder report = new StringBuilder();

            for (int i = 0; i < nOrecords; i++)
            {
                string recordFlag = reader.ReadByte().ToString("X");

                string[] items = new string[numberOfFields];

                for (int j = 0; j < numberOfFields; j++)
                {
                    Encoding faIR = Encoding.GetEncoding(1256);

                    switch (fieldType[j])
                    {
                        case "B":
                            throw new NotImplementedException();

                        case "C":
                            items[j] = new string(faIR.GetChars(reader.ReadBytes(int.Parse(fieldLength[j])))).Trim();
                            break;

                        default:
                            items[j] = new string(Encoding.ASCII.GetChars(reader.ReadBytes(int.Parse(fieldLength[j])))).Trim();
                            break;
                    }

                }

                report.AppendLine(string.Join(",", items));
                //Console.WriteLine(string.Join(" , ", items));
            }



            //string value = val.ToString("X2");
            reader.Close();
            s.Close();
            System.IO.File.WriteAllText(@"C:\Users\Hossein\Desktop\" + DateTime.Now.ToLongTimeString().Replace(':', '-') + ".txt", report.ToString());
            //string fileDirectory = System.IO.Path.GetDirectoryName(filePath);


            //string unicodeString = row.ItemArray.GetValue(3).ToString();
            //Console.Read();
        }

        static void TestdbfReaderForFarsiColumns()
        {
            string filePath = @"D:\Data\Aja\SHP_Map\City_Poly.dbf";


            string fileDirectory = System.IO.Path.GetDirectoryName(filePath);

            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

            string connectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0; Data Source=" + fileDirectory + " ; Extended Properties=\"dBASE IV; \"";
            //string connectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0; Data Source=" + fileDirectory  + " ; Extended Properties=\"dBASE IV\"";

            string commandString = string.Format("SELECT * FROM [{0}];", GetShortFileName(fileDirectory, fileName + ".dbf"));

            System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(connectionString);

            connection.Open();

            System.Data.OleDb.OleDbCommand command = new System.Data.OleDb.OleDbCommand(commandString, connection);

            System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(command);

            System.Data.DataTable table = new System.Data.DataTable();

            table.Locale = System.Globalization.CultureInfo.GetCultureInfo("fa-IR");

            adapter.Fill(table);

            System.Data.DataTable table2 = connection.GetSchema();


            connection.Close();

            System.Data.DataRow row = table.Rows[0];

            string unicodeString = row.ItemArray.GetValue(3).ToString();

            object o = row.ItemArray.GetValue(3);

            System.Text.Encoding srcEncoding = System.Text.Encoding.ASCII;
            System.Text.Encoding midEncoding = Encoding.GetEncoding(1256);
            System.Text.Encoding dstEncoding = System.Text.Encoding.GetEncoding(850);


            string fieldValue = row.ItemArray.GetValue(3).ToString();

            string temp = new string(midEncoding.GetChars(
                                                Encoding.Convert(Encoding.UTF8,
                                                midEncoding,
                                                Encoding.UTF8.GetBytes(fieldValue))));
            //string UniTo1252 = changeEncoding(Encoding.Unicode, Encoding.GetEncoding(1252), fieldValue);
            //string E1252To850 = changeEncoding(Encoding.GetEncoding(850), Encoding.Unicode, UniTo1252);
            //string E1252ToUnicode = changeEncoding(Encoding.GetEncoding(1252), Encoding.UTF8, E1252To850);
            string UniTo1252 = changeEncoding(Encoding.ASCII, Encoding.GetEncoding(1256), fieldValue);
            string UniTo12 = changeEncoding(Encoding.Unicode, Encoding.GetEncoding(1256), fieldValue);



            string ttt = "s";
            //// Create two different encodings.
            //Encoding ascii = Encoding.ASCII;
            //Encoding unicode = Encoding.Unicode;

            //Encoding utf8 = Encoding.Unicode;

            //// Convert the string into a byte array. 
            ////byte[] unicodeBytes = unicode.GetBytes(unicodeString);
            //byte[] unicodeBytes = utf8.GetBytes(unicodeString);

            //// Perform the conversion from one encoding to the other. 
            ////byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

            //// Perform the conversion from one encoding to the other. 
            //byte[] utf8Bytes = Encoding.Convert(utf8, unicode, unicodeBytes);

            //// Convert the new byte[] into a char[] and then into a string. 
            //char[] utf8Chars = new char[utf8.GetCharCount(utf8Bytes, 0, utf8Bytes.Length)];
            //utf8.GetChars(utf8Bytes, 0, utf8Bytes.Length, utf8Chars, 0);
            //string utf8String = new string(utf8Chars);

            // Display the strings created before and after the conversion.
            //Console.WriteLine("Original string: {0}", unicodeString);
            //Console.WriteLine("Ascii converted string: {0}", asciiString);

        }

        static string changeEncoding(System.Text.Encoding first, Encoding second, string text)
        {
            byte[] srcBytes = first.GetBytes(text);
            char[] dstChars = new char[second.GetCharCount(srcBytes, 0, srcBytes.Length)];
            second.GetChars(srcBytes, 0, srcBytes.Length, dstChars, 0);
            return new string(dstChars);

        }

        public static string GetShortFileName(string fileDirectory, string fileNameWithExtension)
        {
            StringBuilder temp = new StringBuilder(255);

            string path = System.IO.Path.Combine(fileDirectory, fileNameWithExtension);

            int n = GetShortPathName(path, temp, 255);

            if (n == 0)
                throw new NotImplementedException();

            string extension = System.IO.Path.GetExtension(path);

            return ((temp.ToString().Split('\\')).Last()).ToLower();
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int GetShortPathName(
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)]
                    string path,
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)]
                    StringBuilder shortPath,
            int shortPathLength);


        #endregion


        #region STANFORDClass-Design & Analysis of Algorithms

        static void QuizMergeSort()
        {
            int[] array = new int[] { 5, 3, 8, 9, 1, 7, 0, 2, 6, 4 };

            IRI.Ket.DataStructure.SortAlgorithm.MergeSort<int>(array, (i, j) => i.CompareTo(j));
        }

        static void testHashtable()
        {
            string path = @"D:\Tutorial\STANFORD.Design&AnalysisOfAlgorithmsI\Problem Sets\hashtable\HashInt.txt";

            string[] lines = System.IO.File.ReadAllLines(path);

            System.Collections.Hashtable table = new System.Collections.Hashtable(100000);

            int i = 0;

            foreach (string item in lines)
            {
                int value = int.Parse(item);

                table.Add(value, value % 20);
            }

            DateTime t1 = DateTime.Now;
            bool result1 = LookUp(231552, table);
            TimeSpan dt1 = DateTime.Now - t1;

            DateTime t2 = DateTime.Now;
            bool result2 = LookUp(234756, table);
            TimeSpan dt2 = DateTime.Now - t2;

            DateTime t3 = DateTime.Now;
            bool result3 = LookUp(596873, table);
            TimeSpan dt3 = DateTime.Now - t3;

            DateTime t4 = DateTime.Now;
            bool result4 = LookUp(648219, table);
            TimeSpan dt4 = DateTime.Now - t4;


            DateTime t5 = DateTime.Now;
            bool result5 = LookUp(726312, table);
            TimeSpan dt5 = DateTime.Now - t5;

            DateTime t6 = DateTime.Now;
            bool result6 = LookUp(981237, table);
            TimeSpan dt6 = DateTime.Now - t6;

            DateTime t7 = DateTime.Now;
            bool result7 = LookUp(988331, table);
            TimeSpan dt7 = DateTime.Now - t7;

            DateTime t8 = DateTime.Now;
            bool result8 = LookUp(1277361, table);
            TimeSpan dt8 = DateTime.Now - t8;

            DateTime t9 = DateTime.Now;
            bool result9 = LookUp(1283379, table);
            TimeSpan dt9 = DateTime.Now - t9;

        }

        static bool LookUp(int number, System.Collections.Hashtable table)
        {
            foreach (int item in table.Values)
            {
                if (table.ContainsKey(number - item))
                {
                    return true;
                }
            }

            return false;
        }

        static void testFastDFS()
        {
            //string[] b = new string[8];
            //b[0] = string.Format("{0},{1}", 1, 3);
            //b[1] = string.Format("{0},{1}", 1, 2);
            //b[2] = string.Format("{0},{1}", 2, 3);
            //b[3] = string.Format("{0},{1}", 4, 2);
            //b[4] = string.Format("{0},{1}", 3, 4);
            //b[5] = string.Format("{0},{1}", 5, 4);
            //b[6] = string.Format("{0},{1}", 5, 6);
            //b[7] = string.Format("{0},{1}", 6, 6);
            //IRI.Ket.Graph.FastImplementation.Graph f = new IRI.Ket.Graph.FastImplementation.Graph(b, ',', 6);

            //string path = @"D:\Tutorial\STANFORD.Design&AnalysisOfAlgorithmsI\Problem Sets\SCC\New Text Document15.txt";
            //string[] lines = System.IO.File.ReadAllLines(path);
            //IRI.Ket.Graph.FastImplementation.Graph f = new IRI.Ket.Graph.FastImplementation.Graph(lines, ',', 15);


            string path = @"D:\Online Courses\STANFORD.Design&AnalysisOfAlgorithmsI\Programming Questions\PQ4.SCC.txt";
            string[] lines = System.IO.File.ReadAllLines(path);
            //IRI.Ket.Graph.Search.Graph f = new IRI.Ket.Graph.Search.Graph(lines, ' ', 875714);


            //List<LinkedList<int>> result = IRI.Ket.Graph.FastImplementation.DFS.GetStronglyConnectedComponents(f);

            //List<int> counts = new List<int>();

            //for (int i = 0; i < result.Count; i++)
            //{
            //    counts.Add(result[i].Count);
            //}

            //counts.Sort();
            //counts.Reverse();
        }



        static void testSCC()
        {
            string path = @"D:\Online Courses\STANFORD.Design&AnalysisOfAlgorithmsI\Programming Questions\PQ4.SCC.txt";
            string[] lines = System.IO.File.ReadAllLines(path);
            //IRI.Ket.Graph.Search.Graph f = new IRI.Ket.Graph.Search.Graph(lines, ' ', 875714);


            //DateTime t0 = DateTime.Now;
            //IRI.Ket.Graph.Search.FastDepthFirstSearch d = new IRI.Ket.Graph.Search.FastDepthFirstSearch(f);
            //TimeSpan dt = DateTime.Now - t0;
            //List<List<int>> result = IRI.Ket.Graph.Graph.GetStronglyConnectedComponents(graph);
        }

        static void testMinCut()
        {
            //IRI.Ket.Graph.GraphRepresentation.AdjacencyList<int, int> graph = new AdjacencyList<int, int>();

            //for (int i = 0; i < 5; i++)
            //{
            //    graph.AddNode(i);
            //}

            //graph.Connect(0, new Connection<int, int>(1, 0), false);
            //graph.Connect(0, new Connection<int, int>(4, 0), false);
            //graph.Connect(1, new Connection<int, int>(2, 0), false);
            //graph.Connect(1, new Connection<int, int>(4, 0), false);
            //graph.Connect(2, new Connection<int, int>(3, 0), false);
            //graph.Connect(3, new Connection<int, int>(4, 0), false);

            //int n = IRI.Ket.Graph.MinCut.MinimumCut.GetMinCut(graph);

            string path = @"C:\Users\Hossein\Desktop\kargerAdj.CSV.txt";

            IRI.Ket.Graph.GraphRepresentation.AdjacencyList<string, int> graph = ReadFile(path, 1);

            int temp = 1000;

            for (int i = 0; i < 1000; i++)
            {
                int n = IRI.Ket.Graph.MinCut.MinimumCut.GetMinCut(graph);

                temp = Math.Min(temp, n);
            }

        }



        private static T PartitionWithMedianElement<T>(T[] array, Func<T, T, int> comparer, int startIndex, int endIndex)
        {
            T first = array[startIndex];
            T middle = array[(int)Math.Floor((endIndex + startIndex) / 2.0)];
            T last = array[endIndex];

            int index;

            if (comparer(first, middle) <= 0 && comparer(middle, last) <= 0)
            {
                index = (int)Math.Floor((endIndex + startIndex) / 2.0);
            }
            else if (comparer(first, last) <= 0 && comparer(last, middle) <= 0)
            {
                index = endIndex;
            }
            else if (comparer(middle, first) <= 0 && comparer(first, last) <= 0)
            {
                index = startIndex;
            }
            else if (comparer(middle, last) <= 0 && comparer(last, first) <= 0)
            {
                index = endIndex;
            }
            else if (comparer(last, first) <= 0 && comparer(first, middle) <= 0)
            {
                index = startIndex;
            }
            else if (comparer(last, middle) <= 0 && comparer(middle, first) <= 0)
            {
                index = (int)Math.Floor((endIndex + startIndex) / 2.0);
            }
            else
            {
                throw new NotImplementedException();
            }

            return array[index];
        }

        static void testMergeSort()
        {
            string fileName = @"D:\Tutorial\STANFORD.Design&AnalysisOfAlgorithmsI\Problem Sets\problem set-1\IntegerArray2.txt";

            DateTime t0 = DateTime.Now;
            string[] lines = System.IO.File.ReadAllLines(fileName);
            TimeSpan dtIO = DateTime.Now - t0;
            //

            int[] values = new int[lines.Length];

            t0 = DateTime.Now;
            for (int i = 0; i < lines.Length; i++)
            {
                values[i] = int.Parse(lines[i]);
            }
            TimeSpan dtParse = DateTime.Now - t0;


            t0 = DateTime.Now;
            int[] result = IRI.Ket.DataStructure.SortAlgorithm.MergeSort<int>(values, (i, j) => i.CompareTo(j));
            TimeSpan dtAlgo = DateTime.Now - t0;

            //long count = 0;
            //int[] result = IRI.Ket.DataStructure.SortAlgorithm.CountInversionAndSort<int>(values, (i, j) => i.CompareTo(j), ref count);

            t0 = DateTime.Now;
            int[] result2 = IRI.Ket.DataStructure.SortAlgorithm.Heapsort<int>(values, (i, j) => i.CompareTo(j));
            TimeSpan dtAlgo2 = DateTime.Now - t0;

            t0 = DateTime.Now;
            Array.Sort(values);
            TimeSpan dtAlgo4 = DateTime.Now - t0;

            t0 = DateTime.Now;
            IRI.Ket.DataStructure.SortAlgorithm.QuickSort<int>(values, (i, j) => i.CompareTo(j));
            TimeSpan dtAlgo3 = DateTime.Now - t0;


            StringBuilder b = new StringBuilder();

            foreach (int item in result)
            {
                b.AppendLine(item.ToString());
            }

            System.IO.File.WriteAllText(@"C:\Users\Hossein\Desktop\New folder\IntegerArray2.txt", b.ToString());
        }

        static void testQuickSort()
        {
            string fileName = @"C:\Users\Hossein\Desktop\QuickSort.txt";

            DateTime t0 = DateTime.Now;
            string[] lines = System.IO.File.ReadAllLines(fileName);
            TimeSpan dtIO = DateTime.Now - t0;
            //

            int[] values = new int[lines.Length];

            t0 = DateTime.Now;
            for (int i = 0; i < lines.Length; i++)
            {
                values[i] = int.Parse(lines[i]);
            }
            TimeSpan dtParse = DateTime.Now - t0;

            //values = new int[] { 1, 3, 5, 2, 4, 6 };

            long count = 0;
            IRI.Ket.DataStructure.SortAlgorithm.QuickSort<int>(values, (i, j) => i.CompareTo(j));

            StringBuilder b = new StringBuilder();

            foreach (int item in values)
            {
                b.AppendLine(item.ToString());
            }

            System.IO.File.WriteAllText(@"C:\Users\Hossein\Desktop\QuickSort.txt", b.ToString());
        }
        #endregion


        //^^^^^^^^^^^**********AFTER UNIVERSITY ERA**********^^^^^^^^^^^
        //**************************************************************


        #region DataStructures

        static void TestBinaryTree()
        {
            int[] vals = new int[] { 7, 4, 11, 3, 2, 6, 9, 18, 14, 12, 17, 19, 22, 20 };

            IRI.Ket.DataStructure.Trees.BinarySearchTree<int> t = new IRI.Ket.DataStructure.Trees.BinarySearchTree<int>(vals);

            IRI.Ket.DataStructure.Trees.BinarySearchNode<int> val = t.Search(2);

            IRI.Ket.DataStructure.Trees.BinarySearchNode<int> val02 = t.Minimum;

            //IRI.Ket.DataStructure.Trees.BinarySearchTreeNode<int> val03 = t.GetSuccessor(t.parent.LeftChild.RigthChild);

            //IRI.Ket.DataStructure.Trees.BinarySearchTreeNode<int> val04 = t.GetPredecessor(t.parent);

            //t.Insert(new IRI.Ket.DataStructure.Trees.BinarySearchTreeNode<int>(13));
            //t.LeftRotate(t.Parent.RigthChild);
            //t.RigthRotate(t.Parent.RigthChild);
            Console.WriteLine();

            //foreach (int item in t.InorderTreeWalk)
            //{
            //    Console.Write(item.ToString() + ", ");
            //}

            //Console.WriteLine();

            //foreach (int item in t.PreorderTreeWalk)
            //{
            //    Console.Write(item.ToString() + ", ");
            //}

            //Console.WriteLine();

            //foreach (int item in t.PostorderTreeWalk)
            //{
            //    Console.Write(item.ToString() + ", ");
            //}




        }

        static void TestRedBlackTree()
        {
            //int[] vals = new int[] { 11, 2, 14, 1, 7, 5, 8, 15 };
            int[] vals =
               new int[] { 26, 17, 41, 14, 21, 30, 47, 10, 16, 19, 21, 28, 38, 7, 12, 14, 20, 35, 39, 3 };

            IRI.Ket.DataStructure.Trees.RedBlackTree<int> t =
                new IRI.Ket.DataStructure.Trees.RedBlackTree<int>(vals);

            t.Insert(4);

            Console.Read();
        }

        static void TestOrderStatisticsTree()
        {
            int[] vals =
                new int[] { 26, 17, 41, 14, 21, 30, 47, 10, 16, 19, 21, 28, 38, 7, 12, 14, 20, 35, 39, 3 };

            IRI.Ket.DataStructure.Trees.OrderStatisticTree<int> t =
                new IRI.Ket.DataStructure.Trees.OrderStatisticTree<int>(vals);


        }

        static void TestIntervalTree()
        {
            IRI.Ket.DataStructure.Trees.IntervalTree<int> tree =
                new IRI.Ket.DataStructure.Trees.IntervalTree<int>(16, 21);

            tree.Insert(8, 9);
            tree.Insert(25, 30);
            tree.Insert(15, 23);
            tree.Insert(5, 8);
            tree.Insert(17, 19);
            tree.Insert(26, 26);

            tree.Insert(6, 10);
            tree.Insert(0, 3);

            tree.Insert(19, 20);

            IRI.Ket.DataStructure.Trees.IntervalTreeNode<int> result = tree.Search(11, 14);
            Console.Read();
        }

        static void TestBTree01()
        {
            IRI.Ket.DataStructure.Trees.BTreeNode<char> rootNode = new IRI.Ket.DataStructure.Trees.BTreeNode<char>();

            IRI.Ket.DataStructure.Trees.BTree<char> tree =
                new IRI.Ket.DataStructure.Trees.BTree<char>(rootNode, 2);

            tree.Insert('F');
            tree.Insert('S');
            tree.Insert('Q');
            tree.Insert('K');
            tree.Insert('C');
            tree.Insert('L');
            tree.Insert('H');
            tree.Insert('T');
            tree.Insert('V');
            tree.Insert('W');
            tree.Insert('M');
            tree.Insert('R');
            tree.Insert('N');
            tree.Insert('P');
            tree.Insert('A');
            tree.Insert('B');
            tree.Insert('X');
            tree.Insert('Y');
            tree.Insert('D');
            tree.Insert('Z');
            tree.Insert('E');

            Console.Read();
        }

        static void TestBTree02()
        {
            IRI.Ket.DataStructure.Trees.BTreeNode<int> rootNode =
                new IRI.Ket.DataStructure.Trees.BTreeNode<int>();

            IRI.Ket.DataStructure.Trees.BTree<int> tree =
                new IRI.Ket.DataStructure.Trees.BTree<int>(rootNode, 2);

            tree.Insert(8);
            tree.Insert(5);
            tree.Insert(1);
            tree.Insert(7);
            tree.Insert(3);
            tree.Insert(12);
            tree.Insert(9);
            tree.Insert(6);

            Console.Read();
        }

        #endregion


        #region MyThesis


        #region ThesisInverseFunction

        static void TestInverseFunction()
        {
            spatial.Point p0 = new spatial.Point(0, 0);
            spatial.Point p1 = new spatial.Point(1, 2);

            //spatial.Point p001 = Transforms.Rotate180(Moves.North)(p0, 2);
            //spatial.Point p002 = Transforms.InverseTransform(Transforms.Rotate180)(Transforms.Rotate180(Moves.SouthEast))(p1, 2);
            //spatial.Point p003 = Transforms.InverseTransform(Transforms.Rotate90CCW)(Transforms.Rotate90CCW(Moves.SouthEast))(p1, 2);
            //spatial.Point p004 = Transforms.InverseTransform(Transforms.HorizontalReflection)(Transforms.HorizontalReflection(Moves.SouthEast))(p1, 2);
            //spatial.Point p005 = Transforms.InverseTransform(Transforms.Rotate90CW)(Transforms.Rotate90CW(Moves.SouthEast))(p1, 2);
            //spatial.Point p006 = Transforms.InverseTransform(Transforms.VerticalReflection)(Transforms.VerticalReflection(Moves.SouthEast))(p1, 2);

        }

        #endregion

        #region ThesisOverlay

        private static void TestOverlay()
        {

            double[,] first = new double[8, 8];
            double[,] second = new double[8, 8];

            first[0, 0] = 1; first[0, 1] = 4.0; first[0, 2] = 3; first[0, 3] = 4; first[0, 4] = 3; first[0, 5] = 3; first[0, 6] = 1; first[0, 7] = 4.5;
            first[1, 0] = 5; first[1, 1] = 2.5; first[1, 2] = 1; first[1, 3] = 2; first[1, 4] = 5; first[1, 5] = 5; first[1, 6] = 1; first[1, 7] = 4;
            first[2, 0] = 3; first[2, 1] = 9.0; first[2, 2] = 5; first[2, 3] = 2; first[2, 4] = 1; first[2, 5] = 2; first[2, 6] = 3; first[2, 7] = 4;
            first[3, 0] = 6; first[3, 1] = 1.0; first[3, 2] = 5; first[3, 3] = 2; first[3, 4] = 2; first[3, 5] = 1; first[3, 6] = 3; first[3, 7] = 2;
            first[4, 0] = 3; first[4, 1] = 5.0; first[4, 2] = 3; first[4, 3] = 1; first[4, 4] = 2; first[4, 5] = 3; first[4, 6] = 2; first[4, 7] = 1;
            first[5, 0] = 1; first[5, 1] = 1.0; first[5, 2] = 2; first[5, 3] = 1; first[5, 4] = 1; first[5, 5] = 3; first[5, 6] = 2; first[5, 7] = 1;
            first[6, 0] = 1; first[6, 1] = 1.0; first[6, 2] = 1; first[6, 3] = 1; first[6, 4] = 1; first[6, 5] = 3; first[6, 6] = 1; first[6, 7] = 4;
            first[7, 0] = 1; first[7, 1] = 1.0; first[7, 2] = 3; first[7, 3] = 2; first[7, 4] = 4; first[7, 5] = 1; first[7, 6] = 4; first[7, 7] = 3;


            second[0, 0] = 1; second[0, 1] = 3; second[0, 2] = 4; second[0, 3] = 4; second[0, 4] = 2; second[0, 5] = 2; second[0, 6] = 3; second[0, 7] = 6;
            second[1, 0] = 2; second[1, 1] = 1; second[1, 2] = 4; second[1, 3] = 2; second[1, 4] = 3; second[1, 5] = 2; second[1, 6] = 2; second[1, 7] = 1;
            second[2, 0] = 2; second[2, 1] = 3; second[2, 2] = 2; second[2, 3] = 1; second[2, 4] = 5; second[2, 5] = 1; second[2, 6] = 3; second[2, 7] = 6;
            second[3, 0] = 2; second[3, 1] = 3; second[3, 2] = 3; second[3, 3] = 2; second[3, 4] = 5; second[3, 5] = 2; second[3, 6] = 3; second[3, 7] = 1;
            second[4, 0] = 4; second[4, 1] = 1; second[4, 2] = 3; second[4, 3] = 2; second[4, 4] = 1; second[4, 5] = 3; second[4, 6] = 2; second[4, 7] = 5;
            second[5, 0] = 3; second[5, 1] = 2; second[5, 2] = 1; second[5, 3] = 4; second[5, 4] = 1; second[5, 5] = 2; second[5, 6] = 3; second[5, 7] = 3;
            second[6, 0] = 1; second[6, 1] = 2; second[6, 2] = 2; second[6, 3] = 1; second[6, 4] = 2; second[6, 5] = 1; second[6, 6] = 2; second[6, 7] = 5;
            second[7, 0] = 6; second[7, 1] = 1; second[7, 2] = 1; second[7, 3] = 5; second[7, 4] = 5; second[7, 5] = 5; second[7, 6] = 1; second[7, 7] = 5;

            Sfc.Primitives.SpaceFillingCurve firstPath = SpaceFillingCurves.Hilbert(Moves.North, Moves.East);

            //List<spatial.Point> points01 = IRI.Ket.Spatial. SpaceFillingCurve.Construct(8, 8, firstPath);

            Sfc.Primitives.SpaceFillingCurve secondPath = SpaceFillingCurves.Gray(Moves.West, Moves.South, Moves.East);

            Sfc.Primitives.SpaceFillingCurve thirdPath = SpaceFillingCurves.NOrdering(Moves.North, Moves.SouthWest);
            //List<spatial.Point> points02 = IRI.Ket.Spatial. SpaceFillingCurve.Construct(8, 8, firstPath);

            //List<spatial.Point> points03 = IRI.Ket.Spatial. SpaceFillingCurve.Construct(8, 8, BasicPaths.Hilbert(Moves.North, Moves.East));


            //List<spatial.Point> points00 = IRI.Ket.Spatial. SpaceFillingCurve.Construct(8, 8, secondPath);
            //List<spatial.Point> points000 = IRI.Ket.Spatial. SpaceFillingCurve.Construct(8, 8, BasicPaths.Gray(Moves.West, Moves.South, Moves.East));


            List<double> hilbertSort = IRI.Ket.Spatial.Primitives.SpaceFillingCurve.Project2DDataTo1D(first, firstPath);
            List<double> graySort = IRI.Ket.Spatial.Primitives.SpaceFillingCurve.Project2DDataTo1D(second, secondPath);
            List<double> mortonSort = IRI.Ket.Spatial.Primitives.SpaceFillingCurve.Project2DDataTo1D(second, thirdPath);

            //double[,] result = IRI.Ket.Spatial. Primitives.SpaceFillingCurve.Overlay(hilbertSort, firstPath, graySort, secondPath, 8, 8, (a, b) => a + b);
            //double[,] result02 = IRI.Ket.Spatial. Primitives.SpaceFillingCurve.Overlay(hilbertSort, firstPath, mortonSort, thirdPath, 8, 8, (a, b) => a + b);

        }

        static void TestIndexToPoint()
        {
            Sfc.Primitives.SpaceFillingCurve path = SpaceFillingCurves.Hilbert(Moves.East, Moves.North);

            int n = (int)Math.Pow(2, 8);

            for (int i = 0; i < n * n; i++)
            {
                int index01 = i;

                spatial.Point point = IRI.Ket.Spatial.Primitives.SpaceFillingCurve.TransformIndexToPoint(path, index01, n);

                int index02 = IRI.Ket.Spatial.Primitives.SpaceFillingCurve.TransformPointToIndex(path, point, n);

                if (index01 != index02)
                {
                    throw new NotImplementedException();
                }
            }

        }

        static void TestBaseConversion()
        {
            List<int> result;

            //result = IRI.Ket.Spatial. SpaceFillingCurve.ChangeBase(73, 2);
            //result = IRI.Ket.Spatial. SpaceFillingCurve.ChangeBase(73, 3);
            //result = IRI.Ket.Spatial. SpaceFillingCurve.ChangeBase(73, 4);
            //result = IRI.Ket.Spatial. SpaceFillingCurve.ChangeBase(73, 5);
            //result = IRI.Ket.Spatial. SpaceFillingCurve.ChangeBase(73, 6);
            //result = IRI.Ket.Spatial. SpaceFillingCurve.ChangeBase(73, 7);

        }

        static void TestCompareOverlay()
        {
            int level = 10;

            int n = (int)Math.Pow(2, level);

            double[] first = new double[n * n];
            double[] second = new double[n * n];

            List<double> firstValues = new List<double>(first);
            List<double> secondValues = new List<double>(second);

            Sfc.Primitives.SpaceFillingCurve firstSfc = SpaceFillingCurves.Hilbert(Moves.East, Moves.North);
            Sfc.Primitives.SpaceFillingCurve secondSfc = SpaceFillingCurves.Gray(Moves.South, Moves.East, Moves.North);

            DateTime t0 = DateTime.Now;

            IRI.Ket.Spatial.Primitives.SpaceFillingCurve.SpeedEfficientOverlay(firstValues, firstSfc, secondValues, secondSfc, n, n, (a, b) => a + b + 1);

            TimeSpan dt1 = DateTime.Now - t0;


            t0 = DateTime.Now;

            //IRI.Ket.Spatial. SpaceFillingCurve.Overlay02(firstValues, firstSfc, secondValues, secondSfc, n, n, (a, b) => a + b + 1);

            TimeSpan dt2 = DateTime.Now - t0;

        }

        #endregion

        #region Thesis02


        static void TestComparePoint()
        {
            List<spatial.Point> array = new List<spatial.Point>(100);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int x = i * 50 + 50;
                    int y = j * 50 + 50;

                    array.Add(new spatial.Point(x, y));
                }
            }

            Boundary boundary = IRI.Ket.Spatial.PointSorting.PointOrdering.GetBoundary(array.ToArray(), 5);

            int result = IRI.Ket.Spatial.PointSorting.PointOrdering.HilbertComparer(new spatial.Point(50, 50), new spatial.Point(50, 100), boundary);



        }

        static void TestSparsePointOrderingNew()
        {
            List<spatial.Point> array = new List<spatial.Point>(100);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int x = i * 50 + 50;
                    int y = j * 50 + 50;

                    array.Add(new spatial.Point(x, y));
                }
            }

            spatial.Point[] result = IRI.Ket.Spatial.PointSorting.PointOrdering.HilbertSorter(array.ToArray());

            DrawSparsePoints("G", new List<spatial.Point>(result), 450);
        }

        static void TestKdTreeNormal()
        {
            List<spatial.Point> points = new List<spatial.Point>();
            points.Add(new spatial.Point(30, 40));
            points.Add(new spatial.Point(10, 30));
            points.Add(new spatial.Point(45, 5));

            points.Add(new spatial.Point(20, -10));
            points.Add(new spatial.Point(0, 0));

            points.Add(new spatial.Point(1, 50));

            points.Add(new spatial.Point(50, 45));
            points.Add(new spatial.Point(55, 35));

            points.Add(new spatial.Point(60, -20));
            points.Add(new spatial.Point(70, -6));

            points.Add(new spatial.Point(40, -5));
            points.Add(new spatial.Point(50, -7));
            points.Add(new spatial.Point(51, 1));

            Func<spatial.Point, spatial.Point, int> xWise = (p1, p2) => p1.X.CompareTo(p2.X);
            Func<spatial.Point, spatial.Point, int> yWise = (p1, p2) => p1.Y.CompareTo(p2.Y);

            List<Func<spatial.Point, spatial.Point, int>> funcs = new List<Func<spatial.Point, spatial.Point, int>>();

            funcs.Add(xWise); funcs.Add(yWise);

            //IRI.Ket.DataStructure.AdvancedStructures.KdTree<Point> kdtree =
            //    new IRI.Ket.DataStructure.AdvancedStructures.KdTree<Point>(points.ToArray(), funcs);

            //IRI.Ket.DataStructure.AdvancedStructures.BalancedKdTreeNode<spatial.Point>.SetNilNode(new spatial.Point(double.NaN, double.NaN));

            IRI.Ket.DataStructure.AdvancedStructures.BalancedKdTree<spatial.Point> kdtree =
                new IRI.Ket.DataStructure.AdvancedStructures.BalancedKdTree<spatial.Point>(points.ToArray(), funcs, spatial.Point.NaN, i => i);

            Boundary b = Sfc.PointSorting.PointOrdering.GetBoundary(points.ToArray(), 2);

            Func<spatial.Point, spatial.Point, int> myFunc = (p1, p2) => Sfc.PointSorting.PointOrdering.HilbertComparer(new spatial.Point(p1.X, p1.Y), new spatial.Point(p2.X, p2.Y), b);

            IRI.Ket.DataStructure.AdvancedStructures.BalancedKdTree<spatial.Point> kdtree02 =
                new IRI.Ket.DataStructure.AdvancedStructures.BalancedKdTree<spatial.Point>(points.ToArray(), new List<Func<spatial.Point, spatial.Point, int>>() { myFunc }, spatial.Point.NaN, i => i);
        }

        static void TestPointOrdering()
        {

            spatial.Point p1 = new spatial.Point(63, 62);

            spatial.Point p2 = new spatial.Point(62, 63);

            Sfc.Primitives.SpaceFillingCurve c = Sfc.Primitives.SpaceFillingCurves.NOrdering(Moves.West, Moves.SouthEast);
            //Sfc.SpaceFillingCurve c = new Sfc.SpaceFillingCurve(Sfc.Primitives.BasicPaths.Hilbert(Moves.West, Moves.South));

            int result = IRI.Ket.Spatial.PointSorting.PointOrdering.HilbertComparer(p1, p2, new Boundary(new spatial.Point(0, 0), new spatial.Point(100, 100)));

            string s = "d";

            s.Any(cu => true);
            //System.Linq.Enumerable.Sum<

        }

        static void TestPointSorting()
        {
            List<spatial.Point> array = new List<spatial.Point>(100);

            Random r1 = new Random(10);

            Random r2 = new Random(20);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int x = r1.Next(500);

                    int y = r2.Next(500);

                    array.Add(new spatial.Point(x, y));
                }
            }

            int jj = 0;

            int ii = array.IndexOf(new spatial.Point(-jj, -jj));

            //spatial.Point[] result = IRI.Ket.Spatial.PointSorting.PointOrdering.HilbertSorter(array.ToArray());
            spatial.Point[] result = IRI.Ket.Spatial.PointSorting.PointOrdering.HosseinSorter(array.ToArray());

            DrawSparsePoints("HO", new List<spatial.Point>(result), 500);
        }

        static void TestSFCSegmentTypeAnalysis()
        {
            Sfc.Primitives.SpaceFillingCurve path = SpaceFillingCurves.Hilbert(Moves.North, Moves.East);

            int level = 2;

            int N = (int)Math.Pow(path.BaseSize, level + 1);

            IRI.Ket.Spatial.Analysis.RegionMoves rm =
                IRI.Ket.Spatial.Analysis.SegmentMeasure.Measure(path, level, SpaceFillingCurves.GetHilbert1stToOtherSubregionTransforms);

        }

        public static bool Foo(spatial.Point first, spatial.Point second)
        {
            return first.X - second.X == 1.0;
        }

        #endregion

        #region Spatial Sort

        static void TestMeasure()
        {
            //int n = IRI.Ket.Spatial. Analysis.SegmentMeasure.MeasureJumps(
            //         IRI.Ket.Spatial. Primitives.BasicPaths.Hilbert(Moves.North, Moves.East),
            //         IRI.Ket.Spatial. Primitives.Jumps.HilbertJump,
            //         8);

            //n = IRI.Ket.Spatial.Version05.Analysis.SegmentMeasure.MeasureJumps(
            //         IRI.Ket.Spatial.Version05.Primitives.BasicPaths.Gray(Moves.North, Moves.East, Moves.South),
            //         IRI.Ket.Spatial.Version05.Primitives.Jumps.GrayJump,
            //         8);

            //n = IRI.Ket.Spatial.Version05.Analysis.SegmentMeasure.MeasureJumps(
            //         IRI.Ket.Spatial.Version05.Primitives.BasicPaths.NOrdering(Moves.North, Moves.SouthEast),
            //         IRI.Ket.Spatial.Version05.Primitives.Jumps.ZigzagJump,
            //         8);

            //n = IRI.Ket.Spatial.Version05.Analysis.SegmentMeasure.MeasureJumps(
            //         IRI.Ket.Spatial.Version05.Primitives.BasicPaths.(Moves.North, Moves.SouthEast),
            //         IRI.Ket.Spatial.Version05.Primitives.Jumps.ZigzagJump,
            //         8);



        }

        static TimeSpan TestHilbert(int level)
        {
            int size = (int)Math.Pow((double)2, (double)level);

            DateTime t0 = DateTime.Now;

            List<spatial.Point> result =
                                        Sfc.Primitives.SpaceFillingCurve.ConstructRasterSFC(size, size,
                                                Sfc.Primitives.SpaceFillingCurves.Hilbert(
                                                                Sfc.Primitives.Moves.North,
                                                                Sfc.Primitives.Moves.East));

            TimeSpan time = DateTime.Now - t0;

            Draw("Hilbert", result, size);

            return time;
        }

        static TimeSpan TestGray(int level)
        {
            int size = (int)Math.Pow((double)2, (double)level);

            DateTime t0 = DateTime.Now;

            List<spatial.Point> result =
                Sfc.Primitives.SpaceFillingCurve.ConstructRasterSFC(size, size,
                        Sfc.Primitives.SpaceFillingCurves.Gray(Sfc.Primitives.Moves.North,
                                                        Sfc.Primitives.Moves.East,
                                                        Sfc.Primitives.Moves.South));

            TimeSpan time = DateTime.Now - t0;

            DrawGray(result, size);

            return time;
        }

        static TimeSpan TestZigZag(int level)
        {
            int size = (int)Math.Pow((double)2, (double)level);

            DateTime t0 = DateTime.Now;

            List<spatial.Point> result =
                Sfc.Primitives.SpaceFillingCurve.ConstructRasterSFC(
                        size, size,
                        Sfc.Primitives.SpaceFillingCurves.NOrdering(Sfc.Primitives.Moves.East, Sfc.Primitives.Moves.SouthWest));

            TimeSpan time = DateTime.Now - t0;

            Draw("zigzag", result, size);

            return time;
        }

        static TimeSpan TestLebesgueDiagonal(int level)
        {
            int size = (int)Math.Pow((double)2, (double)level);

            DateTime t0 = DateTime.Now;

            List<spatial.Point> result =
                Sfc.Primitives.SpaceFillingCurve.ConstructRasterSFC(size, size,
                        Sfc.Primitives.SpaceFillingCurves.LebesgueDiagonal(
                                                        Sfc.Primitives.Moves.NorthEast,
                                                        Sfc.Primitives.Moves.South));

            TimeSpan time = DateTime.Now - t0;

            Draw("LebesgueDiagonal", result, size);

            return time;

        }

        static TimeSpan TestLebesgueSquare(int level)
        {
            int size = (int)Math.Pow((double)2, (double)level);

            DateTime t0 = DateTime.Now;

            List<spatial.Point> result =
                Sfc.Primitives.SpaceFillingCurve.ConstructRasterSFC(size, size,
                        Sfc.Primitives.SpaceFillingCurves.UOrderOrLebesgueSquare(
                                                        Sfc.Primitives.Moves.North,
                                                        Sfc.Primitives.Moves.East));

            TimeSpan time = DateTime.Now - t0;

            Draw("LebesgueSquare", result, size);

            return time;
        }

        static TimeSpan TestPeano(int level)
        {
            int size = (int)Math.Pow((double)3, (double)level);

            DateTime t0 = DateTime.Now;

            List<spatial.Point> result =
                Sfc.Primitives.SpaceFillingCurve.ConstructRasterSFC(size, size,
                        Sfc.Primitives.SpaceFillingCurves.Peano(Sfc.Primitives.Moves.North,
                                                                            Sfc.Primitives.Moves.East));

            TimeSpan time = DateTime.Now - t0;

            Draw("Peano", result, size);

            return time;
        }

        static TimeSpan TestPeano03(int level)
        {
            int size = (int)Math.Pow((double)3, (double)level);

            DateTime t0 = DateTime.Now;

            List<spatial.Point> result =
                Sfc.Primitives.SpaceFillingCurve.ConstructRasterSFC(size, size,
                        Sfc.Primitives.SpaceFillingCurves.Peano03(Sfc.Primitives.Moves.South,
                                                            Sfc.Primitives.Moves.West));

            TimeSpan time = DateTime.Now - t0;

            Draw("Peano", result, size);

            return time;
        }


        static void printResult(int bSize, string name, List<TimeSpan> results)
        {

            string result = string.Empty;

            for (int i = 0; i < results.Count; i++)
            {
                result += Math.Pow(bSize, i + 1).ToString() + " " + results[i].ToString() + System.Environment.NewLine;
            }

            System.IO.File.WriteAllText(
                string.Format(
                    @"D:\{0}{1}.txt", name,
                    System.DateTime.Now.ToLongTimeString().Replace(':', '-'))
                    , result);
        }

        static void DrawSparsePoints(string name, List<spatial.Point> points, int size)
        {
            System.Drawing.Bitmap image = new Drawing.Bitmap(size, size);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);

            for (int i = 0; i < points.Count - 1; i++)
            {
                g.DrawLine(System.Drawing.Pens.Green,
                    new System.Drawing.Point((int)points[i].X, image.Height - (int)points[i].Y),
                    new System.Drawing.Point((int)points[i + 1].X, image.Height - (int)points[i + 1].Y));

                g.DrawEllipse(System.Drawing.Pens.Red,
                        new Drawing.Rectangle((int)points[i].X - 2, image.Height - ((int)points[i].Y + 2), 4, 4));
            }

            g.DrawEllipse(System.Drawing.Pens.Red,
                    new Drawing.Rectangle((int)points[points.Count - 1].X - 2, image.Height - ((int)points[points.Count - 1].Y + 2), 4, 4));

            image.Save(string.Format(@"C:\Users\Hossein\Desktop\New folder\{0}{1}.bmp", name, System.DateTime.Now.ToLongTimeString().Replace(':', '-')));
        }

        static void Draw(string name, List<spatial.Point> points, int size)
        {

            int scaleFactor = size;
            //int scaleFactor = 1;

            System.Drawing.Bitmap image = new Drawing.Bitmap(scaleFactor * size, scaleFactor * size);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);

            for (int i = 0; i < points.Count - 1; i++)
            {
                g.DrawLine(System.Drawing.Pens.Green,
                    new System.Drawing.Point(scaleFactor * (int)points[i].X, image.Height - size - scaleFactor * (int)points[i].Y),
                    new System.Drawing.Point(scaleFactor * (int)points[i + 1].X, image.Height - size - scaleFactor * (int)points[i + 1].Y));

                g.DrawEllipse(System.Drawing.Pens.Red,
                        new Drawing.Rectangle(scaleFactor * (int)points[i].X - 2, image.Height - size - (scaleFactor * (int)points[i].Y + 2), 4, 4));

            }

            g.DrawEllipse(System.Drawing.Pens.Red,
                    new Drawing.Rectangle(scaleFactor * (int)points[points.Count - 1].X - 2, image.Height - size - (scaleFactor * (int)points[points.Count - 1].Y + 2), 4, 4));

            image.Save(string.Format(@"C:\Users\Hossein\Desktop\New folder\{0}{1}.bmp", name, System.DateTime.Now.ToLongTimeString().Replace(':', '-')));
        }

        static void DrawGray(List<spatial.Point> points, int size)
        {
            int scaleFactor = 5 * size;

            System.Drawing.Bitmap image = new Drawing.Bitmap(scaleFactor * size, scaleFactor * size);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);

            for (int i = 0; i < points.Count - 1; i = i + 4)
            {
                int a = (int)(i * 200.0 / points.Count + 55);
                //System.Drawing.Pen pen1 = new Drawing.Pen(System.Drawing.Color.FromArgb(a, 170, 170, 170));
                //System.Drawing.Pen pen2 = new Drawing.Pen(System.Drawing.Color.FromArgb(a, 70, 70, 70));
                //System.Drawing.Pen pen3 = new Drawing.Pen(System.Drawing.Color.FromArgb(a, 0, 0, 0));
                System.Drawing.Pen pen1 = new Drawing.Pen(System.Drawing.Color.FromArgb(255, a, a, a), 3);

                g.DrawLine(pen1,
                    new System.Drawing.Point((int)scaleFactor * (int)points[i].X, image.Height - size - scaleFactor * (int)points[i].Y),
                    new System.Drawing.Point((int)scaleFactor * (int)points[i + 1].X, image.Height - size - scaleFactor * (int)points[i + 1].Y));

                g.DrawLine(pen1,
                    new System.Drawing.Point((int)scaleFactor * (int)points[i + 1].X, image.Height - size - scaleFactor * (int)points[i + 1].Y),
                    new System.Drawing.Point((int)scaleFactor * (int)points[i + 2].X, image.Height - size - scaleFactor * (int)points[i + 2].Y));

                g.DrawLine(pen1,
                    new System.Drawing.Point((int)scaleFactor * (int)points[i + 2].X, image.Height - size - scaleFactor * (int)points[i + 2].Y),
                    new System.Drawing.Point((int)scaleFactor * (int)points[i + 3].X, image.Height - size - scaleFactor * (int)points[i + 3].Y));


                g.DrawEllipse(System.Drawing.Pens.Red,
                        new Drawing.Rectangle((int)scaleFactor * (int)points[i].X - 2, image.Height - size - (int)(scaleFactor * points[i].Y + 2), 4, 4));

                g.DrawEllipse(System.Drawing.Pens.Red,
                        new Drawing.Rectangle((int)scaleFactor * (int)points[i + 1].X - 2, image.Height - size - (int)(scaleFactor * points[i + 1].Y + 2), 4, 4));

                g.DrawEllipse(System.Drawing.Pens.Red,
                        new Drawing.Rectangle((int)scaleFactor * (int)points[i + 2].X - 2, image.Height - size - (int)(scaleFactor * points[i + 2].Y + 2), 4, 4));

                g.DrawEllipse(System.Drawing.Pens.Red,
                        new Drawing.Rectangle((int)scaleFactor * (int)points[i + 3].X - 2, image.Height - size - (int)(scaleFactor * points[i + 3].Y + 2), 4, 4));

            }

            image.Save(string.Format(@"C:\Users\Hossein\Desktop\New folder\gray{0}.bmp", System.DateTime.Now.ToLongTimeString().Replace(':', '-')));
        }

        #endregion

        #endregion


        #region Formating

        static string getString(int[] value)
        {
            Array.Reverse(value);

            string result = string.Empty;

            for (int i = 0; i < value.Length; i++)
            {
                result += value[i];
            }

            return result;
        }

        private static void ShowFormatting()
        {
            Console.WriteLine("{0,10}| {1,20}", "Hossein", "Narimani Rad");
            Console.WriteLine("{0,10}| {1,20}", "Pooya", "Soofbaf");
            //   Hossein|        Narimani Rad
            //     Pooya|             Soofbaf

            Console.WriteLine("{0,-10}| {1,-20}", "Hossein", "Narimani Rad");
            Console.WriteLine("{0,-10}| {1,-20}", "Pooya", "Soofbaf");
            //Hossein   | Narimani Rad
            //Pooya     | Soofbaf

            Console.WriteLine("{0:C2}", 123.1);
            //$123.10

            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("fa-IR");

            Console.WriteLine("{0:C}", 123.1);
            //???? 123.10


            //This format is supported for integral types only.
            Console.WriteLine("{0:D8}", 123);
            //000000123

            Console.WriteLine("{0:E8}", 123.1);
            //1.23100000E+002

            Console.WriteLine("{0:F5}", 123456.1);
            //123456.10000

            Console.WriteLine("{0:F5}", 123456);
            //123456.00000

            Console.WriteLine("{0:G8}", 123.1);
            //123.1

            Console.WriteLine("{0:N5}", 123456.1);
            //123,456.10000

            Console.WriteLine("{0:N5}", 123456);
            //123,456.00000

            Console.WriteLine("{0:P2}", 123.1);
            //12,310.00 %

            //This format is supported for integral types only.
            Console.WriteLine("{0:X4}", 123);
            //007B

            Console.WriteLine("{0:x2}", 123);
            //7b
        }

        #endregion


        #region GSM

        private static void SendSms()
        {
            string pduCode = testSMS();

            //PduDecoding decoding = new PduDecoding(pduCode);

            //serialPort1.Write("AT+CMGF=0" + System.Environment.NewLine);

            //serialPort1.Write("AT+CMGS=\"" + "09123976634" + "\"" + (char)13);


        }

        private static string testSMS()
        {

            ShortTextMessage mes = new ShortTextMessage();
            mes.ServiceCenterNumber = "+9891100500";
            mes.DataCodingSheme = DataCodingSheme.UCS2;
            mes.Report = StatusReportRequest.NoReport;
            mes.DestinationNumber = "+989123976634";
            mes.DestinationNumberFormat = SmscNumberFormat.International;
            mes.ServiceCenterNumberFormat = SmscNumberFormat.International;
            mes.TP_MR = 0;
            mes.Period = ValidPeriod.Maximum;
            mes.message = @". (تست 1)، تست 2

تست 3";
            string result = mes.GetPDUCode();
            string v1 = "0691891901500011000C918919327966430008FF1606330644062706450020062806310020063406450627";

            bool t = result == v1;

            return result;
        }

        private static void LastTest()
        {
            Address address = new Address(+9891100500);
            string message = @"تست 01";

            IRI.Ket.GsmGprsCommunication.SmsSubmit sms = new IRI.Ket.GsmGprsCommunication.SmsSubmit(+9891100500, new Address(+989123976634), message);

            string pdu = sms.PduCode;
            string val2 = "0691891901500011000C918919327966430008FF38062806470020064606270645002006270648000D000A0633064406270645000D000A062A0633062A0031060C0020062A0633062A00200032";
            bool v = val2 == pdu;

        }

        private static void pduSend()
        {
            IRI.Ket.GsmGprsCommunication.AddressField.Address address = new Address(+9891100500);

            string message = @"تست 01";

            IRI.Ket.GsmGprsCommunication.SmsSubmit sms = new IRI.Ket.GsmGprsCommunication.SmsSubmit(+9891100500, new Address(+989123976634), message);

            string pduText = sms.PduCode;

            string[] ports = System.IO.Ports.SerialPort.GetPortNames();

            //string txt = "0011000B916407281553F80000AA0AE8329BFD4697D9EC37";
            System.IO.Ports.SerialPort serialPort1 = new System.IO.Ports.SerialPort()
            {
                PortName = "COM11",
                ReadBufferSize = 10000,
                ReadTimeout = 1000,
                RtsEnable = true,
                WriteBufferSize = 10000,
                WriteTimeout = 1000
            };

            if (serialPort1.IsOpen == false)
            {
                serialPort1.Open();
            }

            //serialPort1.Write("AT+CSCA=\"" + "+9891100500" + "\"" + (char)13);

            serialPort1.Write("AT+CMGF=0" + (char)13);

            System.Threading.Thread.Sleep(1000);

            int length = int.Parse(pduText.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);

            serialPort1.Write("AT+CMGS=" + (pduText.Length - 2 - length * 2) / 2 + (char)13);

            serialPort1.Write(pduText + (char)26);

            serialPort1.Close();
        }

        #endregion


        #region SIFT

        //private static void TestSerialization()
        //{
        //    string path = @"I:\University.M.Sc\106. Articles\1. SIFT\Test\01.xml";

        //    Exterma e1 = new Exterma(1, 2, 3, 4, .5); Exterma e2 = new Exterma(11, 22, 33, 44, .55);

        //    double[] v1 = new double[128]; v1[0] = 1; double[] v2 = new double[128]; v2[0] = 2;

        //    KeyPoint k1 = new KeyPoint(1, Math.PI / 2, 10); KeyPoint k2 = new KeyPoint(2, Math.PI, 20);

        //    Descriptor d1 = new Descriptor(e1, k1, v1); Descriptor d2 = new Descriptor(e2, k2, v2);

        //    List<Descriptor> ds = new List<Descriptor>(); ds.Add(d1); ds.Add(d2);

        //    ImageDescriptors id = new ImageDescriptors(ds);

        //    id.Serialize(path);
        //    ImageDescriptors id2 = ImageDescriptors.Deserialize(path);

        //    //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Exterma));

        //}

        //private static void SIFT()
        //{
        //    DateTime t1 = DateTime.Now;

        //    string path = @"I:\University.M.Sc\106. Articles\1. SIFT\Test\01.jpg";

        //    System.Drawing.Bitmap image = new System.Drawing.Bitmap(path);

        //    Matrix imageMatrix = IRI.Ket.DigitalImageProcessing.Conversion.GrayscaleImageToMatrix(image);

        //    ScaleInvariantFeatureTransform sift = new ScaleInvariantFeatureTransform(imageMatrix, 2, 3, 1, 20);

        //    sift.ConstructScaleSpace();

        //    sift.CalculateDifferenceOfGaussians();

        //    sift.FindExtermas();

        //    sift.FindExactExtermas();

        //    sift.RemoveEdgeResponses();

        //    sift.AssignOrientations();

        //    System.Drawing.Bitmap result = new System.Drawing.Bitmap(image);

        //    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(result);

        //    sift.CreateDescriptors();

        //    TimeSpan t = DateTime.Now - t1;

        //    for (int k = 0; k < sift.keypoints.Count; k++)
        //    {
        //        int i = sift.keypoints[k].ExtermaIndex;

        //        g.DrawEllipse(System.Drawing.Pens.Red, new System.Drawing.RectangleF((float)sift.extermas[i].Column - 1, (float)sift.extermas[i].Row - 1, 2, 2));

        //        float y2 = (float)(sift.extermas[i].Row + sift.keypoints[k].Magnitude * Math.Sin(sift.keypoints[k].Orientation));

        //        float x2 = (float)(sift.extermas[i].Column + sift.keypoints[k].Magnitude * Math.Cos(sift.keypoints[k].Orientation));

        //        g.DrawLine(System.Drawing.Pens.Green, (float)sift.extermas[i].Column, (float)sift.extermas[i].Row, x2, y2);
        //    }

        //    g.Save();

        //    result.Save(string.Format(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\{0}.jpg", (System.DateTime.Now.ToShortTimeString()).Replace(':', '-')));
        //    //sift.CreateDescriptors();
        //}

        //private static void Match()
        //{
        //    List<string> path = new List<string>();

        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1184).jpg");//0
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1185).jpg");//1
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1193).jpg");//2
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1194).jpg");//3
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1196).jpg");//4
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1197).jpg");//5
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1203).jpg");//6
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1204).jpg");//7
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1210).jpg");//8
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1211).jpg");//9
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1225).jpg");//10
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1226).jpg");//11
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1232).jpg");//12
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1171).jpg");//13
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1172).jpg");//14

        //    path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\01.jpg");//15
        //    path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\02.jpg");//16
        //    path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Bird01.jpg");//17
        //    path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Bird02.jpg");//18
        //    path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Kitchen01.jpg");//19
        //    path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Kitchen02.jpg");//20
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Boof01.jpg");//21
        //    //path.Add(@"I:\University.M.Sc\106. Articles\1. SIFT\Test\Boof02.jpg");//22


        //    List<ImageDescriptors> database = new List<ImageDescriptors>();

        //    for (int i = 0; i < path.Count; i++)
        //    {
        //        database.Add(GetDescriptors(path[i]));
        //    }

        //    DateTime t1 = DateTime.Now;

        //    //string targetStrig = @"I:\University.M.Sc\106. Articles\1. SIFT\Test\Image(1196).jpg";

        //    //ImageDescriptors target = GetDescriptors(targetStrig);

        //    SiftImageMatching matching = new SiftImageMatching(database);

        //    List<IndexValue<double>> result = matching.CalculateSimilariry(database[5], .8);

        //    TimeSpan elapsedTime = DateTime.Now - t1;
        //}

        //private static ImageDescriptors GetDescriptors(string path)
        //{
        //    System.Drawing.Bitmap image = new System.Drawing.Bitmap(path);

        //    Matrix imageMatrix = IRI.Ket.DigitalImageProcessing.Conversion.GrayscaleImageToMatrix(image);

        //    ScaleInvariantFeatureTransform sift = new ScaleInvariantFeatureTransform(imageMatrix, 2, 3, 0.5, 20);

        //    sift.ConstructScaleSpace();

        //    sift.CalculateDifferenceOfGaussians();

        //    sift.FindExtermas();

        //    sift.FindExactExtermas();

        //    sift.RemoveEdgeResponses();

        //    sift.AssignOrientations();

        //    sift.CreateDescriptors();

        //    return new ImageDescriptors(sift.descriptors);
        //}

        //private static void TestGetPortion()
        //{
        //    string path = @"I:\University.M.Sc\106. Articles\1. SIFT\Test\01.jpg";

        //    System.Drawing.Bitmap image = new System.Drawing.Bitmap(path);

        //    Matrix imageMatrix = IRI.Ket.DigitalImageProcessing.Conversion.GrayscaleImageToMatrix(image);

        //    Matrix resultMatrix = IRI.Ket.DigitalImageProcessing.GeometricEnhancement.GetImagePortion(imageMatrix, 100, 100, 30, Math.PI / 2);

        //    System.Drawing.Bitmap resultImage = IRI.Ket.DigitalImageProcessing.Conversion.MatrixToGrayscaleImage(resultMatrix);

        //    string url = string.Format("I:\\University.M.Sc\\106. Articles\\1. SIFT\\Test\\{0}.jpg", DateTime.Now.ToString().Replace(':', '-').Replace('/', '-'));

        //    resultImage.Save(url);
        //}

        #endregion


        #region Graph

        private static void testDFSBIG()
        {
            string path = @"D:\Online Courses\STANFORD.Design&AnalysisOfAlgorithmsI\Programming Questions\PQ4.SCC.txt";
            string[] lines = System.IO.File.ReadAllLines(path);

            IRI.Ket.Graph.GraphRepresentation.AdjacencyList<int, int> graph = new AdjacencyList<int, int>(875714);

            for (int i = 0; i < 875714; i++)
            {
                graph.AddNode(i);
            }

            for (int i = 0; i < lines.Length; i++)
            {
                //graph.AddNode(i);

                string[] temp = lines[i].Split(' ');

                for (int j = 1; j < temp.Length; j++)
                {
                    if (!string.IsNullOrEmpty(temp[j]))
                    {
                        graph.AddDirectedEdge(int.Parse(temp[0]) - 1, int.Parse(temp[j]) - 1, 1);
                    }
                }
            }

            DateTime t0 = DateTime.Now;

            //IRI.Ket.Graph.Search.DepthFirstSearch<int, int> dfs = new IRI.Ket.Graph.Search.DepthFirstSearch<int, int>(graph, graph[0]);
            //IRI.Ket.Graph.Search.FastDepthFirstSearch<int, int> dfs = new IRI.Ket.Graph.Search.FastDepthFirstSearch<int, int>(graph);
            List<LinkedList<int>> result = IRI.Ket.Graph.Search.FastDepthFirstSearch<int, int>.GetStronglyConnectedComponents(graph);
            TimeSpan dt = DateTime.Now - t0;



            int[] size = new int[result.Count];

            for (int i = 0; i < result.Count; i++)
            {
                size[i] = result[i].Count;
            }

            Array.Sort(size);

            Array.Reverse(size);


            Console.WriteLine(dt.ToString());
        }

        private static AdjacencyList<string, int> ReadFile2(string path, int weight, int numberOfNodes)
        {
            string[] lines = System.IO.File.ReadAllLines(path);

            AdjacencyList<string, int> result = new AdjacencyList<string, int>(numberOfNodes);

            for (int i = 1; i <= numberOfNodes; i++)
            {
                result.AddNode(i.ToString());
                //int j = 5;
            }

            for (int i = 0; i < lines.Length; i++)
            {
                string[] temp = lines[i].Split(' ');

                //result.Connect(temp[0], new Connection<string, int>(temp[1], weight), true);
                result.AddDirectedEdge((int.Parse(temp[0]) - 1).ToString(), temp[1], weight);
            }

            //foreach (string item in lines)
            //{
            //    string[] temp = item.Split(' ');
            //    //if (temp.Length != 2)
            //    //{
            //    //    throw new NotImplementedException();
            //    //}
            //    //for (int i = 1; i < temp.Length; i++)
            //    //{
            //    result.Connect(temp[0], new Connection<string, int>(temp[1], weight), true);
            //    //}
            //}

            return result;
        }

        private static AdjacencyList<string, int> ReadFile(string path, int weight)
        {
            string[] lines = System.IO.File.ReadAllLines(path);

            AdjacencyList<string, int> result = new AdjacencyList<string, int>();

            foreach (string item in lines)
            {
                string[] temp = item.Split(',');

                for (int i = 1; i < temp.Length; i++)
                {
                    result.AddDirectedEdge(temp[0], temp[i], weight);
                }
            }

            return result;
        }

        private static void Tests001()
        {
            IRI.Sta.Algebra.Matrix m = new IRI.Sta.Algebra.Matrix(4, 4);
            m[2, 2] = 4;
            m[1, 0] = 2;
            m[2, 0] = 5;
            m[3, 0] = 4;
            m[0, 0] = 3;
            m[0, 2] = 8;
            m[1, 3] = 10;


            IRI.Ket.Graph.ShortestPaths.DijkstraProblem p = new IRI.Ket.Graph.ShortestPaths.DijkstraProblem(m);

            List<int> result = p.FindShortestPath(1, 3);
        }

        private static void TestTehranGraph()
        {
            string fileName = @"E:\Personal\University.M.Sc\1. First Semester\5. Geocomputation\Navigation Project\Data\AdjacencyList.txt";

            Matrix list = Matrix.Ones(87).Multiply(double.PositiveInfinity);

            for (int i = 0; i < list.NumberOfRows; i++)
            {
                list[i, i] = 0;
            }

            System.IO.StreamReader reader = new System.IO.StreamReader(fileName);

            int row = 0;

            while (!reader.EndOfStream)
            {
                string[] temp = (reader.ReadLine()).Split(';');

                for (int i = 0; i < temp.Length - 1; i++)
                {
                    string[] temp2 = temp[i].Trim(new char[] { ')', '(' }).Split(',');

                    list[row, int.Parse(temp2[0])] = double.Parse(temp2[1]);
                }

                row++;
            }
            IRI.Ket.Graph.ShortestPaths.DijkstraProblem d = new IRI.Ket.Graph.ShortestPaths.DijkstraProblem(list);


            List<int> path = d.FindShortestPath(43, 36);

        }

        private static void CreateGraph()
        {
            string fileName = @"E:\Personal\University.M.Sc\1. First Semester\5. Geocomputation\Navigation Project\Data\OriginalNetworkCleaned.txt";

            System.IO.StreamReader reader = new System.IO.StreamReader(fileName);

            string output = @"E:\Personal\University.M.Sc\1. First Semester\5. Geocomputation\Navigation Project\Data\AdjacencyList[1].txt";

            System.IO.StreamWriter writer = new System.IO.StreamWriter(output);

            StringBuilder[] list = new StringBuilder[300];

            for (int i = 0; i < list.Length; i++)
            {
                list[i] = new StringBuilder();
            }

            while (!reader.EndOfStream)
            {
                string[] temp = reader.ReadLine().Split(',');

                if (temp[0].Equals("0"))
                {
                    int index1 = int.Parse(temp[3]);

                    int index2 = int.Parse(temp[4]);

                    list[index1].Append(string.Format("({0},{1},{2});", index2, temp[2], temp[1]));

                    list[index2].Append(string.Format("({0},{1},{2});", index1, temp[2], temp[1]));
                }
                else if (temp[0].Equals("1"))
                {
                    int index1 = int.Parse(temp[3]);

                    list[index1].Append(string.Format("({0},{1},{2});", temp[4], temp[2], temp[1]));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            for (int i = 0; i < list.Length; i++)
            {
                writer.WriteLine(list[i].ToString());
            }

            reader.Close();

            writer.Close();

        }

        private static void TestBFS()
        {
            List<string> nodes = new List<string>(new string[] { "r", "s", "v", "w", "t", "x", "u", "y" });

            AdjacencyList<string, int> graph = new AdjacencyList<string, int>(nodes);

            graph.AddUndirectedEdge("v", "r", 1);

            graph.AddUndirectedEdge("r", "s", 1);

            graph.AddUndirectedEdge("s", "w", 1);

            graph.AddUndirectedEdge("w", "t", 1);

            graph.AddUndirectedEdge("w", "x", 1);

            graph.AddUndirectedEdge("t", "x", 1);

            graph.AddUndirectedEdge("t", "u", 1);

            graph.AddUndirectedEdge("x", "u", 1);

            graph.AddUndirectedEdge("x", "y", 1);

            graph.AddUndirectedEdge("u", "y", 1);

            IRI.Ket.Graph.Search.BreadthFirstSearch<string, int> bfs = new IRI.Ket.Graph.Search.BreadthFirstSearch<string, int>(graph, "s");

            AdjacencyList<string, int> result = bfs.searchResult;

            List<string> one = bfs.GetPathTo("u");
        }

        private static void TestDFSNEW()
        {
            AdjacencyList<string, int> graph = new AdjacencyList<string, int>(new List<string>(new string[] { "u", "v", "w", "z", "y", "x" }));

            graph.AddDirectedEdge("u", "v", 1);

            graph.AddDirectedEdge("u", "x", 1);

            graph.AddDirectedEdge("v", "y", 1);

            graph.AddDirectedEdge("y", "x", 1);

            graph.AddDirectedEdge("x", "v", 1);

            graph.AddDirectedEdge("w", "y", 1);

            graph.AddDirectedEdge("w", "z", 1);

            graph.AddDirectedEdge("z", "z", 1);

            IRI.Ket.Graph.Search.FastDepthFirstSearch<string, int> dfs = new IRI.Ket.Graph.Search.FastDepthFirstSearch<string, int>(graph);

        }

        private static void TestDFS()
        {
            AdjacencyList<string, int> graph = new AdjacencyList<string, int>(new List<string>(new string[] { "u", "v", "w", "z", "y", "x" }));

            graph.AddDirectedEdge("u", "v", 1);

            graph.AddDirectedEdge("u", "x", 1);

            graph.AddDirectedEdge("v", "y", 1);

            graph.AddDirectedEdge("y", "x", 1);

            graph.AddDirectedEdge("x", "v", 1);

            graph.AddDirectedEdge("w", "y", 1);

            graph.AddDirectedEdge("w", "z", 1);

            graph.AddDirectedEdge("z", "z", 1);

            IRI.Ket.Graph.Search.DepthFirstSearch<string, int> dfs = new IRI.Ket.Graph.Search.DepthFirstSearch<string, int>(graph, "u");

        }

        private static void TestTopologicalSort()
        {
            //List<string> nodes = new List<string>(new string[] { "r", "s", "v", "w", "t", "x", "u", "y" });

            AdjacencyList<string, int> graph = new AdjacencyList<string, int>(new List<string>(new string[] { "s", "t", "u", "v", "w", "z", "y", "x" }));

            graph.AddDirectedEdge("z", "y", 1);

            graph.AddDirectedEdge("y", "x", 1);

            graph.AddDirectedEdge("s", "z", 1);

            graph.AddDirectedEdge("x", "z", 1);

            graph.AddDirectedEdge("z", "w", 1);

            graph.AddDirectedEdge("w", "x", 1);

            graph.AddDirectedEdge("s", "w", 1);

            graph.AddDirectedEdge("t", "v", 1);

            graph.AddDirectedEdge("v", "w", 1);

            graph.AddDirectedEdge("v", "s", 1);

            graph.AddDirectedEdge("t", "u", 1);

            graph.AddDirectedEdge("u", "t", 1);

            graph.AddDirectedEdge("u", "v", 1);

            IRI.Ket.Graph.Search.DepthFirstSearch<string, int> bfs = new IRI.Ket.Graph.Search.DepthFirstSearch<string, int>(graph, "s");

            List<string> sorted = bfs.CalculateTopologiacalSort();
        }

        private static void TestSort()
        {
            AdjacencyList<string, int> graph = new AdjacencyList<string, int>();

            graph.AddDirectedEdge("z", "y", 1);

            graph.AddDirectedEdge("y", "x", 1);

            graph.AddDirectedEdge("s", "z", 1);

            graph.AddDirectedEdge("x", "z", 1);

            graph.AddDirectedEdge("z", "w", 1);

            graph.AddDirectedEdge("w", "x", 1);

            graph.AddDirectedEdge("s", "w", 1);

            graph.AddDirectedEdge("t", "v", 1);

            graph.AddDirectedEdge("v", "w", 1);

            graph.AddDirectedEdge("v", "s", 1);

            graph.AddDirectedEdge("t", "u", 1);

            graph.AddDirectedEdge("u", "t", 1);

            graph.AddDirectedEdge("u", "v", 1);

            IRI.Ket.Graph.Search.DepthFirstSearch<string, int> bfs = new IRI.Ket.Graph.Search.DepthFirstSearch<string, int>(graph, "s");

            List<string> sorted = bfs.GetSortedNodes(IRI.Ket.Graph.Search.SortType.BasedOnFinishTime);
        }

        private static void TestDFS2()
        {
            AdjacencyList<string, int> graph = new AdjacencyList<string, int>();

            graph.AddDirectedEdge("undershorts", "pants", 1);

            graph.AddDirectedEdge("pants", "belt", 1);

            graph.AddDirectedEdge("belt", "jacket", 1);

            graph.AddDirectedEdge("undershorts", "shoes", 1);

            graph.AddDirectedEdge("pants", "shoes", 1);

            graph.AddDirectedEdge("shirt", "belt", 1);

            graph.AddDirectedEdge("shirt", "tie", 1);

            graph.AddDirectedEdge("tie", "jacket", 1);

            graph.AddDirectedEdge("socks", "shoes", 1);

            graph.AddNode("watch");

            IRI.Ket.Graph.Search.DepthFirstSearch<string, int> bfs = new IRI.Ket.Graph.Search.DepthFirstSearch<string, int>(graph, "socks");

            List<string> sorted = bfs.CalculateTopologiacalSort();
        }

        private static void TestSCC()
        {
            AdjacencyList<string, int> graph = new AdjacencyList<string, int>();

            graph.AddDirectedEdge("z", "y", 1);

            graph.AddDirectedEdge("y", "x", 1);

            graph.AddDirectedEdge("s", "z", 1);

            graph.AddDirectedEdge("x", "z", 1);

            graph.AddDirectedEdge("z", "w", 1);

            graph.AddDirectedEdge("w", "x", 1);

            graph.AddDirectedEdge("s", "w", 1);

            graph.AddDirectedEdge("t", "v", 1);

            graph.AddDirectedEdge("v", "w", 1);

            graph.AddDirectedEdge("v", "s", 1);

            graph.AddDirectedEdge("t", "u", 1);

            graph.AddDirectedEdge("u", "t", 1);

            graph.AddDirectedEdge("u", "v", 1);

            List<List<string>> result = IRI.Ket.Graph.Graph.GetStronglyConnectedComponents<string, int>(graph);
        }

        private static void TestKruskal()
        {
            AdjacencyList<string, int> graph = new AdjacencyList<string, int>();

            //graph.Connect("a", new Connection<string, int>("b", 4), false);

            //graph.Connect("a", new Connection<string, int>("h", 8), false);

            //graph.Connect("b", new Connection<string, int>("h", 11), false);

            //graph.Connect("b", new Connection<string, int>("c", 8), false);

            //graph.Connect("h", new Connection<string, int>("i", 7), false);

            //graph.Connect("h", new Connection<string, int>("g", 1), false);

            //graph.Connect("c", new Connection<string, int>("i", 2), false);

            //graph.Connect("c", new Connection<string, int>("f", 4), false);

            //graph.Connect("c", new Connection<string, int>("d", 7), false);

            //graph.Connect("i", new Connection<string, int>("g", 6), false);

            //graph.Connect("g", new Connection<string, int>("f", 2), false);

            //graph.Connect("d", new Connection<string, int>("f", 14), false);

            //graph.Connect("d", new Connection<string, int>("e", 9), false);

            //graph.Connect("e", new Connection<string, int>("f", 10), false);

            AdjacencyList<string, int> result = MinimumSpanningTree.CalculateByKruskal<string, int>(graph);

        }


        #endregion


        #region DTM

        private static void Merge()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "*.txt|*.txt"; dialog.Multiselect = true;

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            string[] fileNames = dialog.FileNames;

            string path = @"E:\Personal\University.M.Sc\1. First Semester\1. GIS\Major Project\li.txt";

            System.IO.StreamWriter writer = new System.IO.StreamWriter(path);

            foreach (string item in fileNames)
            {
                writer.Write(System.IO.File.ReadAllText(item));
            }

            writer.Close();
        }

        private static void MajorTest()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "*.grd|*.grd"; dialog.Multiselect = true;

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            string[] fileNames = dialog.FileNames;

            GRDFileFormat[] files = new GRDFileFormat[fileNames.Length];

            RegularDtm[] dtm = new RegularDtm[fileNames.Length];

            for (int i = 0; i < fileNames.Length; i++)
            {
                files[i] = new GRDFileFormat(fileNames[i]);

                dtm[i] = new RegularDtm(files[i]);
            }

            double[] threshold = new double[] { 20, 16, 12, 10, 8 };

            double cellSize = 10;

            for (int i = 0; i < fileNames.Length; i++)
            {
                IRI.Ket.DigitalTerrainModeling.GRDFileFormat file = new IRI.Ket.DigitalTerrainModeling.GRDFileFormat(fileNames[i]);

                string folderName = System.IO.Path.GetFileNameWithoutExtension(fileNames[i]);

                string path = System.IO.Path.GetDirectoryName(fileNames[i]);

                string outputPath = string.Format("{0}\\{1}", path, folderName);

                if (!System.IO.Directory.Exists(outputPath))
                {
                    System.IO.Directory.CreateDirectory(outputPath);
                }

                StringBuilder info = new StringBuilder();

                int n = file.NumberOfColumns * file.NumberOfRows;

                for (int j = 50; j < 1000; j = j + 200)
                {
                    TestCAG(file, outputPath, n / j, cellSize, ref info);
                }

                System.IO.File.WriteAllText(string.Format("{0}\\{1}.CAG.txt", path, folderName), info.ToString());

            }

        }

        private static void TestLi(GRDFileFormat file, string outputPath, double threshold, double cellSize, ref StringBuilder info)
        {

            Matrix values = file.Values;

            IRI.Ket.DigitalTerrainModeling.RegularDtm d = new IRI.Ket.DigitalTerrainModeling.RegularDtm(values, cellSize, new Point(file.LowerLeftX, file.LowerLeftY));

            DateTime t0 = DateTime.Now;

            IRI.Ket.DigitalTerrainModeling.IrregularDtm irdtm = d.ToIrregularDtmBasedOnLi(threshold);

            TimeSpan dt2 = DateTime.Now - t0;

            IRI.Ket.DigitalTerrainModeling.RegularDtm rdtm = irdtm.ToRegularDtm(cellSize);

            TimeSpan dt3 = DateTime.Now - t0;

            Matrix analysis = d.Differece(rdtm);

            double min = Statistics.GetMin(analysis);

            double max = Statistics.GetMax(analysis);

            double mean = Statistics.CalculateMean(analysis);

            double standarDeviation = Statistics.CalculateStandardDeviation(analysis);

            double var = Statistics.CalculateVariance(analysis);

            string filePath = string.Format(@"{0}\{1}Li(threshold{2}).grd",
                                            outputPath,
                                            System.IO.Path.GetFileNameWithoutExtension(file.Path),
                                            threshold);

            rdtm.SaveAsGRD(filePath, -999);

            string text = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}",
                            System.IO.Path.GetFileNameWithoutExtension(file.Path),
                            string.Format("{0}*{1}", values.NumberOfRows, values.NumberOfColumns),
                            threshold, irdtm.NumberOfPoints, min, max,
                            mean, standarDeviation, var, dt2, dt3);

            info.AppendLine(text);

            System.Drawing.Bitmap image = DoubleBitmap.DoubleToBitmapLinear(rdtm.Values);

            image.Save(string.Format(@"{0}\{1}Li(threshold{2}).bmp",
                                            outputPath,
                                            System.IO.Path.GetFileNameWithoutExtension(file.Path),
                                            threshold));
        }

        private static void TestCAG(GRDFileFormat file, string outputPath, int numberOfPoints, double cellSize, ref StringBuilder info)
        {

            //
            Matrix values = file.Values;

            IRI.Ket.DigitalTerrainModeling.RegularDtm d = new IRI.Ket.DigitalTerrainModeling.RegularDtm(values, cellSize, new Point(file.LowerLeftX, file.LowerLeftY));

            DateTime t0 = DateTime.Now;

            IRI.Ket.DigitalTerrainModeling.IrregularDtm irdtm = d.ToIrregularDtmBaesdOnCAG(numberOfPoints);

            TimeSpan dt2 = DateTime.Now - t0;

            IRI.Ket.DigitalTerrainModeling.RegularDtm rdtm = irdtm.ToRegularDtm(cellSize);

            TimeSpan dt3 = DateTime.Now - t0;

            Matrix analysis = d.Differece(rdtm);

            double min = Statistics.GetMin(analysis); double max = Statistics.GetMax(analysis);

            double mean = Statistics.CalculateMean(analysis);

            double standarDeviation = Statistics.CalculateStandardDeviation(analysis);

            double var = Statistics.CalculateVariance(analysis);

            string filePath = string.Format(@"{0}\{1}.CAG(count{2}).grd",
                                            outputPath,
                                            System.IO.Path.GetFileNameWithoutExtension(file.Path),
                                            numberOfPoints);
            rdtm.SaveAsGRD(filePath, -999);

            string text = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}",
                            System.IO.Path.GetFileNameWithoutExtension(file.Path),
                            string.Format("{0}*{1}", values.NumberOfRows, values.NumberOfColumns),
                            numberOfPoints, irdtm.NumberOfPoints, min, max,
                            mean, standarDeviation, var, dt2, dt3);

            info.AppendLine(text);

            System.Drawing.Bitmap image = DoubleBitmap.DoubleToBitmapLinear(rdtm.Values);

            image.Save(string.Format(@"{0}\{1}.CAG(count{2}).bmp",
                                            outputPath,
                                            System.IO.Path.GetFileNameWithoutExtension(file.Path),
                                            numberOfPoints));
        }

        private static void testFiner()
        {
            Matrix course = new Matrix(5, 5);

            Matrix fine = new Matrix(10, 10);

            IRI.Ket.DigitalTerrainModeling.RegularDtm courseDtm = new IRI.Ket.DigitalTerrainModeling.RegularDtm(course, 10, new Point(10, 35));

            IRI.Ket.DigitalTerrainModeling.RegularDtm fineDtm = new IRI.Ket.DigitalTerrainModeling.RegularDtm(fine, 5, new Point(7.5, 32.5));

            Matrix result = courseDtm.DifferenceFromFinerDtm(fineDtm);

        }

        private static void manipulate()
        {
            string[] fileNames =
                new string[] {
                    @"E:\Personal\University.M.Sc\1. First Semester\1. GIS\Major Project\OriginalData\1.1. UL.UL.grd",
                    @"E:\Personal\University.M.Sc\1. First Semester\1. GIS\Major Project\OriginalData\1.2. UL.UR.grd",
                    @"E:\Personal\University.M.Sc\1. First Semester\1. GIS\Major Project\OriginalData\2.1. UR.UL.grd",
                    @"E:\Personal\University.M.Sc\1. First Semester\1. GIS\Major Project\OriginalData\2.2. UR.UR.grd",
                    @"E:\Personal\University.M.Sc\1. First Semester\1. GIS\Major Project\OriginalData\2.3. UR.LL.grd",
                    @"E:\Personal\University.M.Sc\1. First Semester\1. GIS\Major Project\OriginalData\2.4. UR.LR.grd",
                    @"E:\Personal\University.M.Sc\1. First Semester\1. GIS\Major Project\OriginalData\3.3. LL.LL.grd",
                    @"E:\Personal\University.M.Sc\1. First Semester\1. GIS\Major Project\OriginalData\3.4. LL.LR.grd",
                    @"E:\Personal\University.M.Sc\1. First Semester\1. GIS\Major Project\OriginalData\4.1. LR.UL.grd",
                    @"E:\Personal\University.M.Sc\1. First Semester\1. GIS\Major Project\OriginalData\4.2. LR.UR.grd",
                    @"E:\Personal\University.M.Sc\1. First Semester\1. GIS\Major Project\OriginalData\4.3. LR.LL.grd",
                    @"E:\Personal\University.M.Sc\1. First Semester\1. GIS\Major Project\OriginalData\4.4. LR.LR.grd"};

            string[] tempEx = new string[] { "UL", "UR", "LL", "LR" };

            for (int i = 0; i < fileNames.Length; i++)
            {
                GRDFileFormat file = new GRDFileFormat(fileNames[i]);

                for (int j = 0; j < 4; j++)
                {
                    Matrix values = file.ReadQuarter((QuarterPart)j);

                    string[] temp = (System.IO.Path.GetFileNameWithoutExtension(fileNames[i])).Split(new char[] { ' ' });

                    string path = System.IO.Path.GetDirectoryName(fileNames[i]);

                    string output = string.Format("{0}\\{1}. {2}.{3}", path, temp[0] + (j + 1).ToString(), temp[1], tempEx[j]);

                    file.SaveQuarter(output + ".grd", (QuarterPart)j);

                    System.Drawing.Bitmap image = DoubleBitmap.DoubleToBitmapLinear(values);

                    image.Save(output + ".bmp");
                }
            }
        }

        private static void TestPartitioning()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "*.grd|*.grd";

            dialog.Multiselect = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string[] fileNames = dialog.FileNames;

                GRDFileFormat[] files = new GRDFileFormat[fileNames.Length];

                RegularDtm[] dtm = new RegularDtm[fileNames.Length];

                for (int i = 0; i < fileNames.Length; i++)
                {
                    files[i] = new GRDFileFormat(fileNames[i]);

                    dtm[i] = new RegularDtm(files[i]);
                }
            }
        }

        private static void TestDelaunay()
        {
            PointCollection collection = new PointCollection();

            for (int i = 0; i < 1000; i++)
            {

            }
        }


        #endregion

    }
}