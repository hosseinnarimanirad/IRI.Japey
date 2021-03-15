using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Model.Map;
using IRI.Jab.Common.Presenter.Map;
using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using IRI.Msh.Common.Extensions;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model
{
    public static class FeatureTableCommands
    {
        #region Defaults

        public static FeatureTableCommand Create(Action action, string markup, string tooltip)
        {
            var result = new FeatureTableCommand()
            {
                PathMarkup = markup,
                Command = new RelayCommand(param => action()),
                ToolTip = tooltip,
            };

            result.Command = new RelayCommand(param => action());

            return result;
        }


        //public static Func<MapPresenter, FeatureTableCommand> CreateZoomToExtentCommandFunc = (presenter) => CreateZoomToExtentCommand(presenter);
        public static FeatureTableCommand CreateZoomToExtentCommand(MapPresenter map)
        {
            var result = new FeatureTableCommand()
            {
                PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarMagnify,
                //Layer = layer.AssociatedLayer,
                ToolTip = "محدودهٔ عوارض"
            };

            result.Command = new RelayCommand((param) =>
            {
                var layer = param as ISelectedLayer;

                if (layer == null || map == null)
                    return;

                var features = layer.GetHighlightedFeatures();

                var extent = BoundingBox.GetMergedBoundingBox(features.Select(f => f.TheSqlGeometry.GetBoundingBox()));

                map.ZoomToExtent(extent, false, () => { TryFlashPoint(map, features); });
            });

            return result;
        }

        private static void TryFlashPoint(MapPresenter map, IEnumerable<ISqlGeometryAware> point)
        {
            if (point?.Count() == 1 && point.First().TheSqlGeometry.GetOpenGisType() == Microsoft.SqlServer.Types.OpenGisGeometryType.Point)
            {
                map.FlashHighlightedFeatures(point.First());
            }
        }

        #endregion

        #region Export Excel

        public static FeatureTableCommand CreateExportToExcelCommand(MapPresenter map)
        {
            var result = new FeatureTableCommand()
            {
                PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarPageExcel,
                //Layer = layer.AssociatedLayer,
                ToolTip = "خروجی اکسل"
            };

            result.Command = new RelayCommand((param) =>
            {
                var layer = param as ISelectedLayer;

                if (layer == null || map == null)
                    return;

                var features = layer.GetSelectedFeatures();

                //
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

                foreach (var item in features)
                {
                    if (item is SqlFeature feature)
                    {
                        rows.Add(feature.Attributes);
                    }
                }

                //گرفتن مسیر فایل
                var fileName = map.SaveFile("*.xlsx|*.xlsx");

                if (string.IsNullOrWhiteSpace(fileName))
                    return;

                Ket.OfficeFormat.ExcelHelper.WriteDictionary(rows, fileName, "Sheet1", null, null);

            });

            return result;
        }

        #endregion

        #region Export As Drawing Item

        public static FeatureTableCommand CreateExportAsDrawingLayersCommand(MapPresenter map)
        {
            var result = new FeatureTableCommand()
            {
                PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarVectorPenAdd,
                //Layer = layer.AssociatedLayer,
                ToolTip = "انتقال به ترسیم‌ها"
            };

            result.Command = new RelayCommand((param) =>
            {
                var layer = param as ISelectedLayer;

                if (layer == null || map == null)
                    return;

                var features = layer.GetHighlightedFeatures();

                if (features.IsNullOrEmpty())
                {
                    return;
                }

                foreach (var feature in features)
                {
                    map.AddDrawingItem(feature.TheSqlGeometry.AsGeometry());
                }

                //
                //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

                //foreach (var item in features)
                //{
                //    if (item is SqlFeature feature)
                //    {
                //        rows.Add(feature.Attributes);
                //    }
                //}

                ////گرفتن مسیر فایل
                //var fileName = map.SaveFile("*.xlsx|*.xlsx");

                //if (string.IsNullOrWhiteSpace(fileName))
                //    return;

                //Ket.OfficeFormat.ExcelHelper.WriteDictionary(rows, fileName, "Sheet1", null, null);

            });

            return result;
        }

        #endregion

       

        internal static List<Func<MapPresenter, IFeatureTableCommand>> GetDefaultVectorLayerCommands<T>() where T : class, ISqlGeometryAware
        {
            return new List<Func<MapPresenter, IFeatureTableCommand>>()
            {
                CreateZoomToExtentCommand,
                CreateExportToExcelCommand,
                CreateExportAsDrawingLayersCommand
            };
        }
    }
}
