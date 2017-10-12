using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IRI.Ket.DataManagement;
//using IRI.NGO.GonbadNegar.Model;
using IRI.Ket.SqlServerSpatialExtension;
//using IRI.NGO.GonbadNegar.Data;

namespace MainProject.Forms
{
    public partial class Shapefile2SqlServer : Form
    {
        public Shapefile2SqlServer()
        {
            InitializeComponent();
        }

        private void Shapefile2SqlServer_Load(object sender, EventArgs e)
        {


        }

        private void browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "shapefile|*.shp" };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            this.fileName.Text = dialog.FileName;

            this.tableName.Text = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName);
        }

        private void Do()
        {
            //IRI.Ket.DataManagement.PostGisDataSource postgis =
            //    new PostGisDataSource("localhost", "postgres", "sa123456", "FarsiDatabase", "5432", "park", "geom");

            //var result = postgis.GetGeometries();

            //IRI.Ket.DataManagement.Infrastructure.PostGisInfrastructure post =
            //    new IRI.Ket.DataManagement.Infrastructure.PostGisInfrastructure("localhost", "postgres", "sa123456", "FarsiDatabase", "5432");



            //var result = post.Exists("park");


            //new IRI.Ket.DataManagement.Infrastructure.PostgreSqlInfrastructure("localhost", "postgres", "sa123456", "FarsiDatabase", "5432");
        }

        private void go_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(this.fileName.Text) && !string.IsNullOrEmpty(this.tableName.Text))
            {
                var shapes = IRI.Ket.ShapefileFormat.Shapefile.Read(this.fileName.Text);

                var rows = IRI.Ket.ShapefileFormat.Dbf.DbfFile.Read(IRI.Ket.ShapefileFormat.Shapefile.GetBbfFileName(this.fileName.Text), "temp")
                    .Rows.Cast<System.Data.DataRow>().ToList();

                string kml = IRI.Ket.KmlFormat.KmlDecorator.DecorateWithExtendedData(shapes.Select(i => i.AsPlacemark(j => (IRI.Ket.SpatialBase.Point)IRI.Ket.CoordinateSystem.Projection.UTMToGeodetic(j, 39))).ToList(),
                       rows,
                       new List<string>() { "نام", "نوع", "وضعیت" },
                       new List<Func<DataRow, string>>() {
                        i=>i["Name"].ToString(),
                        i=>i["Kind"].ToString(),
                        i=>i["Operation_"].ToString()
                    });

                System.IO.File.WriteAllText(@"C:\Users\Hossein\Desktop\education.kml", kml);


                //IRI.Ket.DataManagement.SpatialSqlDataProvider provider =
                //    new IRI.Ket.DataManagement.SpatialSqlDataProvider(this.connectionString.Text);


                //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

                //watch.Reset();
                //watch.Start();
                //provider.ImportDataFromShapefile(this.fileName.Text, this.tableName.Text);
                //watch.Stop();

                //foreach (string item in result)
                //{
                //    MessageBox.Show(item);
                //}


                MessageBox.Show("Finished Successfully");
            }
        }

        private void goSdfForVillages()
        {
            if (System.IO.File.Exists(this.fileName.Text) && !string.IsNullOrEmpty(this.tableName.Text) && System.IO.File.Exists(this.sdfFileLocation.Text))
            {
                IRI.Ket.DataManagement.Infrastructure.SqlServerCeInfrastructure provider =
                    new IRI.Ket.DataManagement.Infrastructure.SqlServerCeInfrastructure(this.sdfFileLocation.Text);

                IRI.Ket.DataManagement.ShapefileDataSource<object> dataSource =
                    new IRI.Ket.DataManagement.ShapefileDataSource<object>(this.fileName.Text, "WkbPosition", 4326, false);

                DataTable table = dataSource.GetEntireFeature();

                string createTable =
                    "CREATE TABLE " + this.tableName.Text + " (Id INT IDENTITY(1,1) PRIMARY KEY, Column1 IMAGE, Column2 IMAGE)";

                provider.CreateTable(createTable);

                provider.Insert(
                    this.tableName.Text,
                    table,
                    new List<string>() 
                    { 
                        "Column1", "Column2"
                    },
                    new List<Func<DataRow, object>>() 
                    { 
                        i => ((Microsoft.SqlServer.Types.SqlGeography)i["WkbPosition"]).GeodeticToMercator().STAsBinary().Buffer,
                        i=>IRI.Ket.IO.BinaryStream.StructureToByteArray( 
                            new OldVillage(
                                ArabicToFarsi(i["ابادي"].ToString()),
                                ArabicToFarsi(i["شهرستان"].ToString()),
                                ArabicToFarsi(i["استان"].ToString()),
                                int.Parse(i["وضعيت_طبيع"].ToString()),
                                int.Parse( i["نوع_راه"].ToString()),
                                int.Parse( i["جمعيت_كل"].ToString()),
                                !string.IsNullOrEmpty( i["پاسگاه_انت"].ToString()),
                                !string.IsNullOrEmpty(i["برق"].ToString()),
                                !string.IsNullOrEmpty(i["تلفن"].ToString()),
                                !string.IsNullOrEmpty( i["موج_FM"].ToString()),
                                !string.IsNullOrEmpty(i["پزشك"].ToString()),
                                !string.IsNullOrEmpty( i["داروخانه"].ToString())))}
                        );
                MessageBox.Show("Done Sucessfully");
            }

        }

        private void goSdfForNewVillages()
        {
            //if (System.IO.File.Exists(this.fileName.Text) && !string.IsNullOrEmpty(this.tableName.Text) && System.IO.File.Exists(this.sdfFileLocation.Text))
            //{
            //    IRI.Ket.DataManagement.Infrastructure.SqlServerCeInfrastructure provider =
            //        new IRI.Ket.DataManagement.Infrastructure.SqlServerCeInfrastructure(this.sdfFileLocation.Text);

            //    IRI.Ket.DataManagement.ShapefileDataSource<object> dataSource =
            //        new IRI.Ket.DataManagement.ShapefileDataSource<object>(this.fileName.Text, "WkbPosition", 4326, false);

            //    DataTable table = dataSource.GetEntireFeature();

            //    string createTable =
            //        "CREATE TABLE " + this.tableName.Text + " (Id INT IDENTITY(1,1) PRIMARY KEY, Column1 IMAGE, Column2 IMAGE)";

            //    provider.CreateTable(createTable);

            //    provider.Insert(
            //        this.tableName.Text,
            //        table,
            //        new List<string>() 
            //        { 
            //            "Column1", "Column2"
            //        },
            //        new List<Func<DataRow, object>>() 
            //        { 
            //            i => ((Microsoft.SqlServer.Types.SqlGeography)i["WkbPosition"]).GeodeticToMercator().STAsBinary().Buffer,
            //            i=>IRI.Ket.IO.BinaryStream.StructureToByteArray( 
            //                new Village(
            //                    ArabicToFarsi(i["ابادي"].ToString()),
            //                    ArabicToFarsi(i["دهستان"].ToString()),
            //                    ArabicToFarsi(i["بخش"].ToString()),
            //                    ArabicToFarsi(i["شهرستان"].ToString()),
            //                    ArabicToFarsi(i["استان"].ToString()),
            //                    int.Parse(i["وضعيت_طبيع"].ToString()),
            //                    int.Parse( i["نوع_راه"].ToString()),
            //                    int.Parse( i["جمعيت_كل"].ToString()),
            //                    int.Parse(i["تعداد_خانو"].ToString())))}
            //            );
            //    MessageBox.Show("Done Sucessfully");
            //}

        }

        private void goSdfForMountains()
        {
            if (System.IO.File.Exists(this.fileName.Text) && !string.IsNullOrEmpty(this.tableName.Text) && System.IO.File.Exists(this.sdfFileLocation.Text))
            {
                IRI.Ket.DataManagement.Infrastructure.SqlServerCeInfrastructure provider =
                    new IRI.Ket.DataManagement.Infrastructure.SqlServerCeInfrastructure(this.sdfFileLocation.Text);

                IRI.Ket.DataManagement.ShapefileDataSource<object> dataSource =
                    new IRI.Ket.DataManagement.ShapefileDataSource<object>(this.fileName.Text, "Geo", 4326, false);

                DataTable table = dataSource.GetEntireFeature();

                string createTable =
                    "CREATE TABLE " + this.tableName.Text + " (Id INT IDENTITY(1,1) PRIMARY KEY, Column1 IMAGE, Column2 IMAGE)";

                provider.CreateTable(createTable);

                provider.Insert(this.tableName.Text,
                    table,
                    new List<string>() { "Column1", "Column2" },
                    new List<Func<DataRow, object>>()
                    {
                        i=>((Microsoft.SqlServer.Types.SqlGeography)i["Geo"]).GeodeticToMercator().STAsBinary().Buffer,
                        i=>IRI.Ket.IO.BinaryStream.StructureToByteArray(
                            new MountainStructure()
                            {
                             Name = ArabicToFarsi(i["نام_كوه"].ToString()),
                             SecondaryName = ArabicToFarsi(i["نام_ديگر"].ToString()),
                             Longitude = double.Parse(i["طول"].ToString()),
                             Latitude = double.Parse(i["عرض"].ToString()),
                             Height = double.Parse(i["ارتفاع"].ToString()),
                             Orientation = ArabicToFarsi(i["جهت_كوه"].ToString()),
                             NearestCity = ArabicToFarsi(i["نزديكترين"].ToString()),
                             DistanceToCity = double.Parse(i["فاصله_تانز"].ToString()),
                             CityRelatedPosition = ArabicToFarsi(i["موقعيت_نسب"].ToString()),
                             Mountains = ArabicToFarsi(i["ازرشته_كوه"].ToString()),
                             OriginatingRiver = ArabicToFarsi(i["سرچشمه_رود"].ToString()),
                             CrossingRiver = ArabicToFarsi(i["مسيرعبوررو"].ToString()),
                             Note = ArabicToFarsi(i["توضيحات"].ToString()),
                             Province = ArabicToFarsi(i["استان"].ToString()),
                             IsEdgy = i["کوه_مرزي_2"].ToString().Length>0,
                            })
                    });

                MessageBox.Show("Done Sucessfully");
            }

        }

        private void goSdfForProvinceCenter()
        {
            if (System.IO.File.Exists(this.fileName.Text) && !string.IsNullOrEmpty(this.tableName.Text) && System.IO.File.Exists(this.sdfFileLocation.Text))
            {
                IRI.Ket.DataManagement.Infrastructure.SqlServerCeInfrastructure provider =
                    new IRI.Ket.DataManagement.Infrastructure.SqlServerCeInfrastructure(this.sdfFileLocation.Text);

                IRI.Ket.DataManagement.ShapefileDataSource<object> dataSource =
                    new IRI.Ket.DataManagement.ShapefileDataSource<object>(this.fileName.Text, "Geo", 4326, false);

                DataTable table = dataSource.GetEntireFeature();

                string createTable =
                    "CREATE TABLE IRIProvinceCenter (Province NVARCHAR(25) NOT NULL, ProvinceCenter NVARCHAR(25) NOT NULL, EstablishYear INT NOT NULL, Code INT NOT NULL, Latitude FLOAT NOT NULL, Longitude FLOAT NOT NULL, Geo IMAGE NOT NULL)";

                provider.CreateTable(createTable);

                provider.Insert(this.tableName.Text,
                    table,
                    new List<string>() { "Geo", "Province", "ProvinceCenter", "EstablishYear", "Code", "Latitude", "Longitude" },
                    new List<Func<DataRow, object>>()
                    {
                        i=>((Microsoft.SqlServer.Types.SqlGeography)i["Geo"]).GeodeticToMercator().STAsBinary().Buffer,
                        i=> ArabicToFarsi(i[1].ToString()),
                        i=> ArabicToFarsi(i[2].ToString()),
                        i=> int.Parse(i[3].ToString()),
                        i=> int.Parse(i[4].ToString()),
                        i=> double.Parse(i[5].ToString()),
                        i=> double.Parse(i[6].ToString()),
                    });

                MessageBox.Show("Done Sucessfully");
            }

        }

        private void goSdfForCountyCenter()
        {
            if (System.IO.File.Exists(this.fileName.Text) && !string.IsNullOrEmpty(this.tableName.Text) && System.IO.File.Exists(this.sdfFileLocation.Text))
            {
                IRI.Ket.DataManagement.Infrastructure.SqlServerCeInfrastructure provider =
                    new IRI.Ket.DataManagement.Infrastructure.SqlServerCeInfrastructure(this.sdfFileLocation.Text);

                IRI.Ket.DataManagement.ShapefileDataSource<object> dataSource =
                    new IRI.Ket.DataManagement.ShapefileDataSource<object>(this.fileName.Text, "Geo", 4326, false);

                DataTable table = dataSource.GetEntireFeature();

                string createTable =
                    "CREATE TABLE IRICountyCenter (County NVARCHAR(25) NOT NULL, CountyCenter NVARCHAR(25) NOT NULL, EstablishYear INT NOT NULL, Code INT NOT NULL, Province NVARCHAR(25) NOT NULL, Latitude FLOAT NOT NULL, Longitude FLOAT NOT NULL, Geo IMAGE NOT NULL)";

                provider.CreateTable(createTable);

                provider.Insert(this.tableName.Text,
                    table,
                    new List<string>() { "Geo", "County", "CountyCenter", "EstablishYear", "Code", "Province", "Latitude", "Longitude" },
                    new List<Func<DataRow, object>>()
                    {
                        i=>((Microsoft.SqlServer.Types.SqlGeography)i["Geo"]).GeodeticToMercator().STAsBinary().Buffer,
                        i=> ArabicToFarsi(i[0].ToString()),
                        i=> ArabicToFarsi(i[1].ToString()),
                        i=> int.Parse(i[2].ToString()),
                        i=> int.Parse(i[3].ToString()),
                        i=> ArabicToFarsi(i[4].ToString()),
                        i=> double.Parse(i[5].ToString()),
                        i=> double.Parse(i[6].ToString()),
                    });

                MessageBox.Show("Done Sucessfully");
            }

        }

        private void goPostgres()
        {
            if (System.IO.File.Exists(this.fileName.Text) && !string.IsNullOrEmpty(this.tableName.Text))
            {
                IRI.Ket.DataManagement.Infrastructure.PostgreSqlInfrastructure provider =
                    new IRI.Ket.DataManagement.Infrastructure.PostgreSqlInfrastructure("localhost", "postgres", "sa123456", "FarsiDatabase", "5432");

                IRI.Ket.DataManagement.ShapefileDataSource<object> dataSource =
                    new IRI.Ket.DataManagement.ShapefileDataSource<object>(this.fileName.Text, "Geo", 4326, false);

                DataTable table = dataSource.GetEntireFeature();

                //provider.CreateTable("CREATE TABLE " + this.tableName.Text + "(Id INT IDENTITY(1,1), Name NVARCHAR(100), Longitude FLOAT, Latitude FLOAT, Height FLOAT, Note NVARCHAR(500), WkbPosition IMAGE, Province NVARCHAR(50))");

                //D:\NGO\Data\Export_Output_2.shp

                string createTable =
                    "CREATE TABLE " + this.tableName.Text + " (WkbPosition VARBINARY, X FLOAT, Y FLOAT, name VARCHAR(250), county VARCHAR(200), province VARCHAR(100), natureType BOOLEAN, roadType BOOLEAN, population BOOLEAN, police BOOLEAN, electricity BOOLEAN, phone BOOLEAN, FM BOOLEAN, doctor BOOLEAN, pharmacy BOOLEAN)";

                provider.CreateTable(createTable);

                //provider.CreateTable("CREATE TABLE " + this.tableName.Text + "(Name NVARCHAR(300), WkbPosition IMAGE, X FLOAT, Y FLOAT)");

                //provider.ExecuteNonQuery("CREATE INDEX spatialIndex ON "+ this.tableName.Text + " (\"Y\" ASC, \"X\" ASC)");
                //provider.Insert(this.tableName.Text,

                //                    table,
                //                    new List<string>() { "Name", "WkbPosition" },
                //                    new List<Func<DataRow, object>>() { 
                //                        i=>i["ابادي"],
                //                        i => ((Microsoft.SqlServer.Types.SqlGeography)i["Geo"]).GeodeticToMercator().STAsBinary().Buffer
                //                    });

                provider.Insert(
                    this.tableName.Text,
                    table,
                    new List<string>() 
                    { 
                        "WkbPosition", "X", "Y", "name", "county", "province", "natureType", "roadType", "population", "police", "electricity", "phone", "FM", "doctor", "pharmacy" 
                    },
                    new List<DbType>() {DbType.AnsiString, DbType.Double,DbType.Double,DbType.String,
                                        DbType.String,DbType.String,DbType.Int32,DbType.Int32,DbType.Double,
                                        DbType.Boolean ,DbType.Boolean,DbType.Boolean,DbType.Boolean,DbType.Boolean,DbType.Boolean}
                    ,
                    new List<Func<DataRow, object>>() 
                    { 
                        i => ((Microsoft.SqlServer.Types.SqlGeography)i["Geo"]).GeodeticToMercator().STAsText().Value,
                        i=>((Microsoft.SqlServer.Types.SqlGeography)i["Geo"]).GeodeticToMercator().STX.Value,
                        i=>((Microsoft.SqlServer.Types.SqlGeography)i["Geo"]).GeodeticToMercator().STY.Value,
                        i=>i["ابادي"],i=>i["شهرستان"],i=>i["استان"],i=>int.Parse(i["وضعيت_طبيع"].ToString()),
                        i=>int.Parse( i["نوع_راه"].ToString()),i=>i[5],i=>!string.IsNullOrEmpty( i["پاسگاه_انت"].ToString()),
                        i=>!string.IsNullOrEmpty(i["برق"].ToString()),i=>!string.IsNullOrEmpty( i["داروخانه"].ToString()),
                        i=>!string.IsNullOrEmpty(i[9].ToString()),i=>!string.IsNullOrEmpty( i["موج_FM"].ToString()),
                        i=>!string.IsNullOrEmpty(i["تلفن"].ToString())});

                MessageBox.Show("Done Sucessfully");
            }

        }

        private void ExportDuplicatedMountains()
        {
            //var temp = this.Presenter.LookForMountains(null, null, null, null, 0, 6000)
            //        .GroupBy(i => new { x = i.Geometry.STX.Value, y = i.Geometry.STY.Value })
            //        .Where(i => i.Count() > 1)
            //        .SelectMany(i => i.Select(j => j))
            //        .ToList();

            //IRI.Ket.ShapefileFormat.Writer.ShpWriter.Write(
            //    @"C:\Users\gisad\Desktop\newMountains",
            //    new IRI.Ket.ShapefileFormat.ShapeTypes.ShapeCollection<IRI.Ket.ShapefileFormat.ShapeTypes.Point>(
            //    temp.Select(i => new IRI.Ket.ShapefileFormat.ShapeTypes.Point(i.Longitude, i.Latitude)).ToList()),
            //    Ket.ShapefileFormat.ShapeTypes.ShapeType.Point);

            //IRI.Ket.ShapefileFormat.Dbf.DbfFile.Write<Model.Mountain>(
            //    @"C:\Users\gisad\Desktop\newMountains.dbf",
            //    temp,
            //    new List<Func<Model.Mountain, object>>() 
            //        {
            //            i=>i.Name,
            //            i=>i.SecondaryName,
            //            i=>i.Longitude,
            //            i=>i.Latitude,
            //            i=>i.Height,
            //            i=>i.Orientation,
            //            i=>i.NearestCity,
            //            i=>i.DistanceToCity,
            //            i=>i.CityRelatedPosition,
            //            i=>i.Mountains,
            //            i=>i.OriginatingRiver,
            //            i=>i.CrossingRiver,
            //            i=>i.Note,
            //            i=>i.Province,
            //            i=>i.IsEdgy?"*":string.Empty,
            //        },
            //    new List<Ket.ShapefileFormat.Dbf.DbfFieldDescriptor>() 
            //        {
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetStringField("Name",50),
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetStringField("SecondaryName",50),
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetDoubleField("Longitude"),
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetDoubleField("Latitude"),
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetDoubleField("Height"),
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetStringField("Orientation",20),
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetStringField("NearestCity",50),
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetDoubleField("DistanceToCity"),
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetStringField("CityRelatedPosition",10),
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetStringField("Mountains",20),
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetStringField("OriginatingRiver",20),
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetStringField("CrossingRiver",20),
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetStringField("Note",254),
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetStringField("Province",50),
            //            Ket.ShapefileFormat.Dbf.DbfFieldDescriptors.GetStringField("IsEdgy",2),
            //        },
            //    Encoding.GetEncoding(1256));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "sql compact database|*.sdf" };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            this.sdfFileLocation.Text = dialog.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            goSdfForNewVillages();
            //goSdfForVillages();
            //goSdfForProvinceCenter();
            //goSdfForCountyCenter();
            //goPostgres();
        }

        private void insertRandom_Click(object sender, EventArgs e)
        {
            //System.Data.SqlServerCe.SqlCeConnection connection
            //    = new System.Data.SqlServerCe.SqlCeConnection(string.Format("Data Source = {0}; max database size = 4091", sdfFileLocation.Text));

            //System.Data.SqlServerCe.SqlCeCommand command =
            //    new System.Data.SqlServerCe.SqlCeCommand();

            //command.Connection = connection;

            //connection.Open();

            //command.CommandText = "DELETE FROM MountainsEncrypted";

            //command.ExecuteNonQuery();

            //command.CommandText = "INSERT INTO MountainsEncrypted(Column1, Column2) VALUES(@column1, @column2)";

            //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            //watch.Start();

            //for (int i = 0; i < 10000; i++)
            //{
            //    command.Parameters.Clear();

            //    var temp = MountainStructure.GetRandom();

            //    //var column1 = temp.Geometry.STAsBinary().Buffer;
            //    System.Threading.Thread.Sleep(10);
            //    IRI.Ket.SpatialBase.Point point = new IRI.Ket.SpatialBase.Point(new Random(100).Next(43, 63), new Random().Next(24, 40));
            //    var mercatorPoint = IRI.Ket.CoordinateSystem.Projection.GeodeticToMercator(point, IRI.Ket.CoordinateSystem.Ellipsoids.WGS84);

            //    var geometry = Microsoft.SqlServer.Types.SqlGeometry.Parse(new System.Data.SqlTypes.SqlString(
            //        string.Format("POINT({0} {1})", mercatorPoint.X, mercatorPoint.Y))).STAsBinary().Buffer;

            //    var column2 = IRI.Ket.IO.BinaryStream.StructureToByteArray<MountainStructure>(temp);

            //    command.Parameters.AddWithValue("@column1", geometry);

            //    command.Parameters.AddWithValue("@column2", column2);

            //    command.ExecuteNonQuery();
            //}

            //connection.Close();

            //watch.Stop();
            //var t = watch.ElapsedMilliseconds;
        }

        private void read_Click(object sender, EventArgs e)
        {
            //var temp = Encoding.GetEncodings().Select(i => new Tuple<string, int>(i.DisplayName, i.CodePage)).ToList();
            ReadEncryptedMountains();
        }

        private void ChangeArabicToFarsi()
        {
            //IRI.Ket.DataManagement.ShapefileDataSource source =
            //    new ShapefileDataSource(this.fileName.Text, "Geo", 4326, true);

            string fileName = this.fileName.Text.Replace(".shp", ".dbf");

            var table = IRI.Ket.ShapefileFormat.Dbf.DbfFile.Read(fileName, "mytable");

            //MakeFarsi(ref table);
            foreach (var item in table.Columns)
            {
                if (((System.Data.DataColumn)item).DataType != typeof(string))
                    continue;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string temp = (string)table.Rows[i][(System.Data.DataColumn)item];
                    temp = temp.Replace('ي', 'ی');
                    temp = temp.Replace('ك', 'ک');
                    table.Rows[i][(System.Data.DataColumn)item] = temp;
                }
            }

            IRI.Ket.ShapefileFormat.Dbf.DbfFile.Write(this.fileName.Text + ".dbf",
                table.Select().ToList(),
                new List<Func<DataRow, object>>()
                {
                    i=>int.Parse( i[0].ToString()),
                    i=>i[1].ToString(),
                    i=>i[2].ToString(),
                    i=>double.Parse( i[3].ToString()),
                    i=>double.Parse( i[4].ToString()),
                    i=>double.Parse( i[5].ToString()),
                    i=>i[6].ToString(),
                    i=>i[7].ToString(),
                    i=>int.Parse( i[8].ToString()),
                    i=>i[9].ToString(),
                    i=>i[10].ToString(),
                    i=>i[11].ToString(),
                    i=>i[12].ToString(),
                    i=>i[13].ToString(),
                    i=>i[14].ToString(),
                    i=>i[15].ToString(),
                    i=>int.Parse( i[16].ToString()),
                },
                new List<IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor>()
                {
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("id",'N',9,0),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("نام_کوه",'C',50,0),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("نام_دیگر",'C',50,0),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("طول",'F',13,11),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("عرض",'F',13,11),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("ارتفاع",'F',13,11),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("جهت_کوه",'C',20,0),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("نزدیکترین",'C',50,0),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("فاصله_تانز",'N',9,0),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("موقعیت_نسب",'C',10,0),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("ازرشته_کوه",'C',20,0),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("سرچشمه_رود",'C',20,0),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("مسیرعبوررو",'C',20,0),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("توضیحات",'C',254,0),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("استان",'C',50,0),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("کوه_مرزی_2",'C',25,0),
                    new IRI.Ket.ShapefileFormat.Dbf.DbfFieldDescriptor("tasvir",'N',4,0)
                },
                Encoding.GetEncoding(1256));
        }

        private void ReadEncryptedMountains()
        {
            System.Data.SqlServerCe.SqlCeConnection connection
              = new System.Data.SqlServerCe.SqlCeConnection(string.Format("Data Source = {0}; max database size = 4091", sdfFileLocation.Text));

            System.Data.SqlServerCe.SqlCeCommand command =
                new System.Data.SqlServerCe.SqlCeCommand();

            command.Connection = connection;

            command.CommandText = "SELECT * FROM IRIMountain1";

            connection.Open();

            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            var reader = command.ExecuteReader();

            List<MountainStructure> results = new List<MountainStructure>();

            List<Microsoft.SqlServer.Types.SqlGeometry> geometries = new List<Microsoft.SqlServer.Types.SqlGeometry>();

            while (reader.Read())
            {
                int id = (int)reader[0];

                byte[] geometry = (byte[])reader[1];

                MountainStructure temp = IRI.Ket.IO.BinaryStream.ByteArrayToStructure<MountainStructure>((byte[])reader[2]);

                geometries.Add(Microsoft.SqlServer.Types.SqlGeometry.STGeomFromWKB(
                    new System.Data.SqlTypes.SqlBytes(geometry), 0));

                results.Add(temp);
            }
            connection.Close();
            watch.Stop();
            var t = watch.ElapsedMilliseconds;
        }

        private void goToPostgres_Click(object sender, EventArgs e)
        {

        }

        private string ArabicToFarsi(string value)
        {
            var temp = value.Replace('ي', 'ی');
            return temp.Replace('ك', 'ک');
        }
    }
}
