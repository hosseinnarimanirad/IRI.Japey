using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Sta.Spatial.Primitives; using IRI.Sta.Common.Primitives;
using IRI.Jab.Common.Model;
using IRI.Jab.Common;
using IRI.Ket.Persistence.DataSources;
 

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

            result.LayerId = Guid.NewGuid();

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

        public override BoundingBox Extent
        {
            get
            {
                return _source.WebMercatorExtent;
            }
            protected set
            {
                throw new NotImplementedException(); 
            }
        }

        public override RenderingApproach Rendering
        {
            get { return RenderingApproach.Default; }
            protected set { }
        }
          
        public override LayerType Type
        {
            get { return LayerType.Complex; }
            protected set { _ = value; }            
        } 

        public event EventHandler OnRequestMouseDownHandle;
    }
}
