﻿using Microsoft.SqlServer.Types;
using System;
using System.Data.SqlTypes;
using System.IO;
using System.Windows;
using IRI.Maptor.Sta.Common.Helpers;
using IRI.Maptor.Sta.Ogc.SLD;
using IRI.Maptor.Sta.Common.Contracts.Google;
using IRI.Maptor.Sta.Spatial.Helpers;
using System.Data.Common;
using IRI.Maptor.Sta.Spatial.Model;

namespace IRI.Maptor.MasterProjectWPF;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void loaded_l(object sender, RoutedEventArgs e)
    {
        //SqlServerTypes.Utilities.LoadNativeAssembliesv14(AppDomain.CurrentDomain.BaseDirectory);

        var polygon = SqlGeometry.Parse(new SqlString("POLYGON( (0 0 9, 30 0 9, 30 30 9, 0 30 9, 0 0 9) )"));
        var temp = polygon.AsGml();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        //TestSld(@"C:\Users\Hossein\Downloads\point_pointasgraphic.sld");
        var t1 = new TileInfo(1338158, 837168, 21);
        var t2 = new TileInfo(1338158, 837169, 21);
         

        var result1_1 = t1.WebMercatorExtent;
        var result1_2 = t2.WebMercatorExtent;

        
    }


    private void TestSld(string fileName)
    {
        var sld = XmlHelper.Deserialize<StyledLayerDescriptor>(fileName);

        string modifiedPath = Path.Combine(
                            Path.GetDirectoryName(fileName),
                            Path.GetFileNameWithoutExtension(fileName) + "_m" + Path.GetExtension(fileName));

        XmlHelper.Serialize(modifiedPath, sld, true);
    }

}