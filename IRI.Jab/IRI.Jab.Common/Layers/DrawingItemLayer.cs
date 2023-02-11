using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Model;
using IRI.Jab.Common.Model.Legend;
using IRI.Jab.Common.Model.Symbology; 
using IRI.Extensions;
using IRI.Msh.Common.Primitives;
using Microsoft.SqlServer.Types;
using Geometry = IRI.Msh.Common.Primitives.Geometry<IRI.Msh.Common.Primitives.Point>;
using IRI.Ket.DataManagement.DataSource;

namespace IRI.Jab.Common
{
    public class DrawingItemLayer : VectorLayer, IIdentifiable
    {
        private const string _removeToolTip = "حذف";

        private const string _editToolTip = "ویرایش";

        private const string _zoomToolTip = "بزرگ‌نمایی";

        private const string _saveToolTip = "ذخیره‌سازی";

        public int Id { get; set; }

        public FeatureDataSource OriginalSource { get; set; }

        //public VisualParameters OriginalSymbology { get; set; }

        private Geometry _shape;

        public Geometry Geometry
        {
            get { return _shape; }
            set
            {
                _shape = value;
                RaisePropertyChanged();

                this.DataSource = new MemoryDataSource(new List<Geometry<Point>>() { value });
            }
        }

        //private ILayer _associatedLayer;

        //public ILayer AssociatedLayer
        //{
        //    get { return _associatedLayer; }
        //    set
        //    {
        //        _associatedLayer = value;
        //        RaisePropertyChanged();
        //    }
        //}


        //public VisualParameters VisualParameters
        //{
        //    get { return AssociatedLayer?.VisualParameters; }
        //    set
        //    {
        //        if (AssociatedLayer != null)
        //        {
        //            AssociatedLayer.VisualParameters = value;
        //        }

        //        RaisePropertyChanged();
        //    }
        //}



        //private string _title;

        //public string Title
        //{
        //    get { return _title; }
        //    set
        //    {
        //        _title = value;
        //        RaisePropertyChanged();
        //    }
        //}

        public bool CanShowHighlightGeometry()
        {
            return this.IsSelectedInToc && VisualParameters?.Visibility == System.Windows.Visibility.Visible;
        }

        public Action<DrawingItemLayer> RequestHighlightGeometry;

        //public Action<DrawingItemLayer> RequestChangeVisibilityForHighlightGeometry;

        public Action<DrawingItemLayer> RequestChangeVisibility;

        public Guid HighlightGeometryKey { get; private set; }

        internal DrawingItemLayer(
            string title,
            Geometry geometry,
            VisualParameters visualParameters = null,
            int id = int.MinValue,
            FeatureDataSource source = null)
        {
            this.Extent = geometry.GetBoundingBox();

            this.VisualParameters = visualParameters ?? VisualParameters.GetDefaultForDrawingItems();

            //this.VisualParameters.OnVisibilityChanged += (sender, e) => { this.RequestChangeVisibility?.Invoke(this); };

            HighlightGeometryKey = Guid.NewGuid();

            //this.OriginalSymbology = VisualParameters.Clone();

            var featureType =
                (geometry.Type == GeometryType.Point || geometry.Type == GeometryType.MultiPoint) ? LayerType.Point :
                ((geometry.Type == GeometryType.LineString || geometry.Type == GeometryType.MultiLineString) ? LayerType.Polyline :
                (geometry.Type == GeometryType.Polygon || geometry.Type == GeometryType.MultiPolygon) ? LayerType.Polygon : LayerType.None);

            this.Type = LayerType.Drawing | featureType;

            this.Rendering = RenderingApproach.Default;

            this.ToRasterTechnique = RasterizationApproach.GdiPlus;

            this.Id = id;

            this.OriginalSource = source;

            this.ZIndex = int.MaxValue;

            //this.DataSource = new MemoryDataSource(new List<SqlGeometry>() { geometry.AsSqlGeometry() });

            //this.Title = title;

            this.LayerName = title;

            this.Geometry = geometry;

            this.Commands = new List<ILegendCommand>();

            this.OnIsSelectedInTocChanged += (sender, e) =>
            {
                this.RequestHighlightGeometry?.Invoke(this);
            };

            this.OnVisibilityChanged += (sender, e) =>
            {
                this.RequestChangeVisibility?.Invoke(this);
            };
        }



    }
}
