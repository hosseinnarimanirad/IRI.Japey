using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Msh.Common.Primitives;
using IRI.Jab.Common.Model;
using IRI.Jab.Common;
using spatialBase = IRI.Msh.Common;
using IRI.Ket.DataManagement.DataSource;
using IRI.Jab.Common.Model;

using IRI.Jab.Common.Model;

namespace IRI.Jab.Common
{
    public class ClusteredPointLayer : BaseLayer
    {
        ClusteredGeoTaggedImageSource _source;

        //System.Windows.Media.ImageSource imageSymbol;

        //System.Windows.Media.Geometry shapeSymbol;

        Func<string, System.Windows.FrameworkElement> _viewMaker;

        private ClusteredPointLayer()
        {

        }


        public static ClusteredPointLayer Create(string imageDirectory, Func<string, System.Windows.FrameworkElement> viewMaker)
        {
            ClusteredPointLayer result = new ClusteredPointLayer();

            result.Id = Guid.NewGuid();

            //result.imageSymbol = symbol;
            result._viewMaker = viewMaker;

            result.VisualParameters = new VisualParameters(null, null, 0, 1);

            result._source = ClusteredGeoTaggedImageSource.Create(imageDirectory);

            return result;
        }

        public SpecialPointLayer GetLayer(double scale)
        {
            var images = _source.Get(scale);

            var locatables = new List<Locateable>();

            foreach (var imageGroup in images)
            {
                var locatable = new Locateable();

                locatable.AncherFunction = AncherFunctionHandlers.CenterCenter;

                locatable.X = imageGroup.Center.WebMercatorLocation.X;

                locatable.Y = imageGroup.Center.WebMercatorLocation.Y;

                //locatable.Element = new Common.View.MapMarkers.CountableImageMarker(imageSymbol, imageGroup.Frequency.ToString());

                locatable.Element = _viewMaker(imageGroup.Frequency.ToString());

                locatable.OnRequestHandleMouseDown += (sender, e) => { this.OnRequestMouseDownHandle.SafeInvoke(imageGroup); };

                locatables.Add(locatable);
            }

            return new SpecialPointLayer(LayerName, locatables);
        }

        //private ScaleInterval _visibleRange;

        //public ScaleInterval VisibleRange
        //{
        //    get { return _visibleRange; }
        //    set
        //    {
        //        _visibleRange = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public Guid Id { get; private set; }

        //private string _layerName;

        //public string LayerName
        //{
        //    get { return _layerName; }
        //    set
        //    {
        //        _layerName = value;
        //        RaisePropertyChanged();
        //    }
        //}

        public override BoundingBox Extent
        {
            get
            {
                return _source.Extent;
            }
            protected set
            {
                throw new NotImplementedException();
                //_extent = value;
                //OnPropertyChanged("Extent");
            }
        }

        public override RenderingApproach Rendering
        {
            get { return RenderingApproach.Default; }
            protected set { }
        }

        //public RasterizationApproach ToRasterTechnique { get { return RasterizationApproach.None; } }

        //public bool IsValid { get; set; }

        //public override void Invalidate() => IsValid = false;

        //private LayerType _type;
        //private VisualParameters _visualParameters;

        //public VisualParameters VisualParameters
        //{
        //    get { return _visualParameters; }
        //    set
        //    {
        //        _visualParameters = value;
        //        RaisePropertyChanged();
        //    }
        //}

        public override LayerType Type
        {
            get { return LayerType.Complex; }
            protected set { }
            //private set
            //{
            //    if (value == LayerType.Complex || value == LayerType.RightClickOption || value == LayerType.GridAndGraticule)
            //    {
            //        this._type = value;
            //    }
            //    else
            //    {
            //        throw new NotImplementedException();
            //    }
            //}
        }

        //public int ZIndex { get; set; }

        public event EventHandler OnRequestMouseDownHandle;

    }
}
